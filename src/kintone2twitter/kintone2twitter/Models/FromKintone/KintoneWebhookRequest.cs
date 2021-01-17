using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace kintone2twitter.Models.FromKintone
{
    public class KintoneWebhookRequest
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("app")]
        public App App { get; set; }

        [JsonPropertyName("record")]
        public Record Record { get; set; }

        [JsonPropertyName("recordTitle")]
        public string RecordTitle { get; set; }

        [JsonPropertyName("url")]
        public string Url { get; set; }

    }
    public class App
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }
    }

    public class Twitter投稿日時
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("value")]
        public string Value { get; set; }
    }

    public class レコード番号
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("value")]
        public string Value { get; set; }
    }

    public class 作業者
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("value")]
        public List<object> Value { get; set; }
    }

    public class Value
    {
        [JsonPropertyName("code")]
        public string Code { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }
    }

    public class 更新者
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("value")]
        public Value Value { get; set; }
    }

    public class Value2
    {
        [JsonPropertyName("code")]
        public string Code { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }
    }

    public class 作成者
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("value")]
        public Value2 Value { get; set; }
    }

    public class ステータス
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("value")]
        public string Value { get; set; }
    }

    public class Revision
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("value")]
        public string Value { get; set; }
    }

    public class 更新日時
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("value")]
        public DateTime Value { get; set; }
    }

    public class Twitter投稿済
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("value")]
        public List<string> Value { get; set; }
    }

    public class Twitter投稿メッセージ
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("value")]
        public string Value { get; set; }
    }

    public class Value3
    {
        [JsonPropertyName("fileKey")]
        public string FileKey { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("contentType")]
        public string ContentType { get; set; }

        [JsonPropertyName("size")]
        public string Size { get; set; }
    }

    public class 添付ファイル
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("value")]
        public List<Value3> Value { get; set; }
    }

    public class 投稿内容
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("value")]
        public string Value { get; set; }
    }

    public class 作成日時
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("value")]
        public DateTime Value { get; set; }
    }

    public class Id
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("value")]
        public string Value { get; set; }
    }

    public class Record
    {
        [JsonPropertyName("twitter投稿日時")]
        public Twitter投稿日時 Twitter投稿日時 { get; set; }

        [JsonPropertyName("レコード番号")]
        public レコード番号 レコード番号 { get; set; }

        [JsonPropertyName("作業者")]
        public 作業者 作業者 { get; set; }

        [JsonPropertyName("更新者")]
        public 更新者 更新者 { get; set; }

        [JsonPropertyName("作成者")]
        public 作成者 作成者 { get; set; }

        [JsonPropertyName("ステータス")]
        public ステータス ステータス { get; set; }

        [JsonPropertyName("$revision")]
        public Revision Revision { get; set; }

        [JsonPropertyName("更新日時")]
        public 更新日時 更新日時 { get; set; }

        [JsonPropertyName("twitter投稿済")]
        public Twitter投稿済 Twitter投稿済 { get; set; }

        [JsonPropertyName("twitter投稿メッセージ")]
        public Twitter投稿メッセージ Twitter投稿メッセージ { get; set; }

        [JsonPropertyName("添付ファイル")]
        public 添付ファイル 添付ファイル { get; set; }

        [JsonPropertyName("投稿内容")]
        public 投稿内容 投稿内容 { get; set; }

        [JsonPropertyName("作成日時")]
        public 作成日時 作成日時 { get; set; }

        [JsonPropertyName("$id")]
        public Id Id { get; set; }
    }

}
