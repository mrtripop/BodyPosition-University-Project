namespace BodyPosition.MVVM.Model.UserModel
{
    using System;
    using System.Collections.Generic;

    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public partial class UserModel
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("firstName")]
        public string FirstName { get; set; }

        [JsonProperty("lastName")]
        public string LastName { get; set; }

        [JsonProperty("gender")]
        public string Gender { get; set; }

        [JsonProperty("tel")]
        public string Tel { get; set; }

        [JsonProperty("weight")]
        public double Weight { get; set; }

        [JsonProperty("height")]
        public double Height { get; set; }

        [JsonProperty("date")]
        public string Date { get; set; }

        [JsonProperty("time")]
        public string Time { get; set; }

        [JsonProperty("age")]
        public int Age { get; set; }
    }

    public partial class UserModel
    {
        public static Dictionary<string, UserModel> FromJson(string json) => JsonConvert.DeserializeObject<Dictionary<string, UserModel>>(json, Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this Dictionary<string, UserModel> self) => JsonConvert.SerializeObject(self, Converter.Settings);
    }

    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }
}

