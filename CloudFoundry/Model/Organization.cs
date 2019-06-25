using System;
using Newtonsoft.Json;

namespace PushReport.CloudFoundry.Model
{
    public class Organization
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("billing_enabled")]
        public bool BillingEnabled { get; set; }

        [JsonProperty("quota_definition_guid")]
        public string QuotaDefinitionGuid { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("default_isolation_segment_guid")]
        public string DefaultIsolationSegmentGuid { get; set; }

        [JsonProperty("quota_definition_url")]
        public string QuotaDefinitionUrl { get; set; }

        [JsonProperty("spaces_url")]
        public string SpacesUrl { get; set; }

        [JsonProperty("domains_url")]
        public string DomainsUrl { get; set; }

        [JsonProperty("private_domains_url")]
        public string PrivateDomainsUrl { get; set; }

        [JsonProperty("users_url")]
        public string UsersUrl { get; set; }

        [JsonProperty("managers_url")]
        public string ManagersUrl { get; set; }

        [JsonProperty("billing_managers_url")]
        public string BillingManagersUrl { get; set; }

        [JsonProperty("auditors_url")]
        public string AuditorsUrl { get; set; }

        [JsonProperty("app_events_url")]
        public string AppEventsUrl { get; set; }

        [JsonProperty("space_quota_definitions_url")]
        public string SpaceQuotaDefinitionsUrl { get; set; }
    }
}