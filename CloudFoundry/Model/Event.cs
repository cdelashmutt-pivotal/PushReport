using System;
using Newtonsoft.Json;

namespace PushReport.CloudFoundry.Model
{
    public class Event
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("actor")]
        public string Actor { get; set; }

        [JsonProperty("actor_type")]
        public string ActorType { get; set; }

        [JsonProperty("actor_name")]
        public string ActorName { get; set; }

        [JsonProperty("actor_username")]
        public string ActorUsername { get; set; }

        [JsonProperty("actee")]
        public string Actee { get; set; }

        [JsonProperty("actee_type")]
        public string ActeeType { get; set; }

        [JsonProperty("actee_name")]
        public string ActeeName { get; set; }

        [JsonProperty("timestamp")]
        public DateTime Timestamp { get; set; }

        [JsonProperty("metadata")]
        public object Metadata { get; set; }

        [JsonProperty("space_guid")]
        public string SpaceGuid { get; set; }

        [JsonProperty("organization_guid")]
        public string OrganizationGuid { get; set; }
    }
}