using CoreTweet;
using kintone2twitter.Models.FromKintone;
using kintone2twitter.Models.ToKintone.NotifyError;
using kintone2twitter.Models.ToKintone.NotifySuccess;
using kintone2twitter.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace kintone2twitter.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TweetController : ControllerBase
    {
        private readonly IOptions<KintoneSettings> _kintoneSettings;
        private readonly IOptions<TwitterSettings> _twitterSettings;
        private readonly ILogger<TweetController> _logger;
        public TweetController(IOptions<KintoneSettings> kintoneSettings, IOptions<TwitterSettings> twitterSettings, ILogger<TweetController> logger)
        {
            _kintoneSettings = kintoneSettings;
            _twitterSettings = twitterSettings;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] KintoneWebhookRequest kintoneRequest)
        {
            var requestContent = $"Url={kintoneRequest.Url},App.Id={kintoneRequest.App.Id}" +
                $",Type={kintoneRequest.Type},Twitter投稿済={string.Join(",", kintoneRequest.Record.Twitter投稿済.Value)}" +
                $",投稿内容={kintoneRequest.Record.投稿内容.Value}";
            _logger.LogInformation(requestContent);

            //Webhookで来たJsonのチェック。指定したサービス・アプリ・イベント以外はUnAuthorizedで返す
            if (!kintoneRequest.Url.StartsWith(_kintoneSettings.Value.ServiceUrl)) { return Unauthorized(); }
            if (kintoneRequest.App.Id != _kintoneSettings.Value.AppId) { return Unauthorized(); }
            if (kintoneRequest.Type != "UPDATE_STATUS") { return Unauthorized(); }

            //投稿済は何もしない
            if (kintoneRequest.Record.Twitter投稿済.Value.Contains("投稿済")) { return NoContent(); }

            //Twitterの認証情報セット
            var apiKey = _twitterSettings.Value.ApiKey;
            var apiSecret = _twitterSettings.Value.ApiSecret;
            var accessToken = _twitterSettings.Value.AccessToken;
            var accessTokenSecret = _twitterSettings.Value.AccessTokenSecret;
            var tokens = CoreTweet.Tokens.Create(apiKey, apiSecret, accessToken, accessTokenSecret);

            //Twitterへ投稿
            try
            {
                //メディアを先にアップロード
                var mediaIdList = await UploadMediaAsync(kintoneRequest, tokens);

                //メディア付きでTweet
                var mediaIds = mediaIdList.Any() ? mediaIdList.ToArray() : null;
                var tweetResponse = tokens.Statuses.Update(status: $"{kintoneRequest.Record.投稿内容.Value}", media_ids: mediaIds);
                _logger.LogInformation($"Twitter投稿OK。Id:{tweetResponse.Id},CreatedAt:{tweetResponse.CreatedAt}");
            }
            catch (TwitterException ex)
            {
                var error = ex.Errors.First();
                var errorMessage = $"{error.Code}:{error.Message}";
                _logger.LogWarning(errorMessage);

                //エラーをKintoneに反映
                var tweetErrorJson = JsonSerializer.Serialize(new NotifyErrorRequestBody(kintoneRequest.App.Id, kintoneRequest.Record.レコード番号.Value, errorMessage));
                await NotifyAsync(tweetErrorJson);

                //キャッチできるエラーなので、200で返す
                return Ok();
            }
            catch (Exception ex)
            {
                var errorMessage = ex.Message;

                //stacktraceはサーバにはstacktraceも出す
                _logger.LogError($"{errorMessage}\r\n{ex.StackTrace}");

                //エラーをKintoneに反映
                var jsonError = JsonSerializer.Serialize(new NotifyErrorRequestBody(kintoneRequest.App.Id, kintoneRequest.Record.レコード番号.Value, errorMessage));
                await NotifyAsync(jsonError);

                //想定していないエラーなので、500で返す
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }

            //投稿結果をKintoneに反映
            var json = JsonSerializer.Serialize(new NotifySuccessRequestBody(kintoneRequest.App.Id, kintoneRequest.Record.レコード番号.Value));
            if (await NotifyAsync(json))
            {
                return Ok();
            }
            return new StatusCodeResult(StatusCodes.Status500InternalServerError);
        }
        private async Task<bool> NotifyAsync(string json)
        {
            using var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Put, $"{_kintoneSettings.Value.ServiceUrl}k/v1/record.json");
            request.Headers.Add(@"X-Cybozu-API-Token", _kintoneSettings.Value.ApiToken);
            request.Content = new StringContent(json, Encoding.UTF8, @"application/json");
            try
            {
                var response = await client.SendAsync(request);
                var contentString = await response.Content.ReadAsStringAsync();
                _logger.LogInformation($"Kintone更新OK。StatusCode:{response.StatusCode},Content:{contentString}");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Kintone更新Error。Message:{ex.Message},StackTrace:{ex.StackTrace}");
                return false;
            }

        }
        private async Task<IEnumerable<long>> UploadMediaAsync(KintoneWebhookRequest kintoneRequest, Tokens tokens)
        {
            try
            {

                var list = new List<long>();

                foreach (var attachment in kintoneRequest.Record.添付ファイル.Value)
                {
                    var content = $"Name={attachment.Name},FileKey={attachment.FileKey},Size={attachment.Size},ContentType={attachment.ContentType}";
                    _logger.LogInformation(content);

                    var url = $"{_kintoneSettings.Value.ServiceUrl}k/v1/file.json?fileKey={attachment.FileKey}";
                    using var client = new HttpClient();
                    var request = new HttpRequestMessage(HttpMethod.Get, url);
                    request.Headers.Add("X-Cybozu-API-Token", _kintoneSettings.Value.ApiToken);
                    var response = await client.SendAsync(request);
                    var stream = await response.Content.ReadAsStreamAsync();

                    var mediaUploadResult = await tokens.Media.UploadAsync(stream);
                    list.Add(mediaUploadResult.MediaId);
                }
                return list;
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Message}");
                _logger.LogError($"{ex.StackTrace}");

                throw;
            }
        }
    }
}
