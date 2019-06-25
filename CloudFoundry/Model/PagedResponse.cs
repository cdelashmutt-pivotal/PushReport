using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace PushReport.CloudFoundry.Model
{
    public class PagedResponse<T>
    {
        [JsonProperty("total_results")]
        public int TotalResults { get; set; }

        [JsonProperty("total_pages")]
        public int TotalPages { get; set; }

        [JsonProperty("prev_url", NullValueHandling = NullValueHandling.Ignore)]
        public Uri PrevUrl { get; set; }

        [JsonProperty("next_url", NullValueHandling = NullValueHandling.Ignore)]
        public Uri NextUrl { get; set; }

        [JsonProperty("resources")]
        public List<Resource<T>> Resources { get; set; }
    }
}