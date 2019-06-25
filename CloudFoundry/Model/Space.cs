using System;
using Newtonsoft.Json;

namespace PushReport.CloudFoundry.Model
{
    public class Space
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("organization_guid")]
        public string OrganizationGuid { get; set; }
        [JsonProperty("space_quota_definition_guid")]
        public string SpaceQuotaDefinitionGuid { get; set; }
        [JsonProperty("isolation_segment_guid")]
        public string IsolationSegmentGuid { get; set; }
        [JsonProperty("allow_ssh")]
        public bool AllowSSH { get; set; }
        [JsonProperty("organization_url")]
        public string OrganizationUrl { get; set; }
        [JsonProperty("developers_url")]
        public string DevelopersUrl { get; set; }
        [JsonProperty("managers_url")]
        public string ManagersUrl { get; set; }
        [JsonProperty("auditors_url")]
        public string AuditorsUrl { get; set; }
        [JsonProperty("apps_url")]
        public string AppsUrl { get; set; }
        [JsonProperty("routes_url")]
        public string RoutesUrl { get; set; }
        [JsonProperty("domains_url")]
        public string DomainsUrl { get; set; }
        [JsonProperty("service_instances_url")]
        public string ServiceInstancesUrl { get; set; }
        [JsonProperty("app_events_url")]
        public string AppEventsUrl { get; set; }
        [JsonProperty("events_url")]
        public string EventsUrl { get; set; }
        [JsonProperty("security_groups_url")]
        public string SecurityGroupsUrl { get; set; }
        [JsonProperty("staging_security_groups_url")]
        public string StagingSecurityGroupsUrl { get; set; }
    }
}