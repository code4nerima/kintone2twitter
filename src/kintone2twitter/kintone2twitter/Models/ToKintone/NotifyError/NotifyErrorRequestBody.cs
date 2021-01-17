using System.Text.Json.Serialization;

namespace kintone2twitter.Models.ToKintone.NotifyError
{
    public class NotifyErrorRequestBody
    {
        public NotifyErrorRequestBody(string app, string recordNo, string errorMessage)
        {
            App = app;
            Id = recordNo;
            Record = new Record(errorMessage);
        }
        [JsonPropertyName("app")]
        public string App { get; private set; }

        [JsonPropertyName("id")]
        public string Id { get; private set; }

        [JsonPropertyName("record")]
        public Record Record { get; private set; }
    }
    public class Twitter投稿メッセージ
    {
        [JsonPropertyName("value")]
        public string Value { get; private set; }
        public Twitter投稿メッセージ(string errorMessage)
        {
            Value = errorMessage;
        }
    }

    public class Record
    {
        [JsonPropertyName("twitter投稿メッセージ")]
        public Twitter投稿メッセージ Twitter投稿メッセージ { get; private set; }
        public Record(string errorMessage)
        {
            Twitter投稿メッセージ = new Twitter投稿メッセージ(errorMessage);
        }
    }
}
