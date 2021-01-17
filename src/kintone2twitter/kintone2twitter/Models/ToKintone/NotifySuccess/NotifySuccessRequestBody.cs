using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace kintone2twitter.Models.ToKintone.NotifySuccess
{
    public class NotifySuccessRequestBody
    {
        public NotifySuccessRequestBody(string app, string recordNo)
        {
            App = app;
            Id = recordNo;
            Record = new Record();
        }
        [JsonPropertyName("app")]
        public string App { get; private set; }

        [JsonPropertyName("id")]
        public string Id { get; private set; }

        [JsonPropertyName("record")]
        public Record Record { get; private set; }
    }
    public class Twitter投稿日時
    {
        [JsonPropertyName("value")]
        public DateTime Value { get; private set; } = DateTime.UtcNow;
    }

    public class Twitter投稿済
    {
        [JsonPropertyName("value")]
        public List<string> Value { get; private set; } = new string[] { "投稿済" }.ToList();
    }

    public class Twitter投稿メッセージ
    {
        [JsonPropertyName("value")]
        public string Value { get; private set; } = "正常終了";
    }

    public class Record
    {
        [JsonPropertyName("twitter投稿日時")]
        public Twitter投稿日時 Twitter投稿日時 { get; private set; } = new Twitter投稿日時();

        [JsonPropertyName("twitter投稿済")]
        public Twitter投稿済 Twitter投稿済 { get; private set; } = new Twitter投稿済();

        [JsonPropertyName("twitter投稿メッセージ")]
        public Twitter投稿メッセージ Twitter投稿メッセージ { get; private set; } = new Twitter投稿メッセージ();
        public Record()
        {
        }
    }
}
