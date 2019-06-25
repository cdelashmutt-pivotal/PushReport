using System;
using Newtonsoft.Json;

namespace PushReport.CloudFoundry.Model
{
    public class Resource<T>
    {
        [JsonProperty("metadata")]
        public Metadata Metadata { get; set; }

        [JsonProperty("entity")]
        public T Entity { get; set; }

    }

    public class Metadata
    {
        [JsonProperty("guid")]
        public string Guid { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("created_at")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("updated_at")]
        public DateTime UpdatedAt { get; set; }

    }
}