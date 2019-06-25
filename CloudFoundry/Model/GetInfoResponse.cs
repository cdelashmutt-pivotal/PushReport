using System;
using Newtonsoft.Json;

namespace PushReport.CloudFoundry.Model
{
    public class GetInfoResponse
    {
        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name
        { get; set; }

        [JsonProperty("build", NullValueHandling = NullValueHandling.Ignore)]
        public string Build
        { get; set; }

        [JsonProperty("support", NullValueHandling = NullValueHandling.Ignore)]
        public string Support
        { get; set; }

        [JsonProperty("version", NullValueHandling = NullValueHandling.Ignore)]
        public int? Version
        { get; set; }

        [JsonProperty("description", NullValueHandling = NullValueHandling.Ignore)]
        public string Description
        { get; set; }

        [JsonProperty("authorization_endpoint", NullValueHandling = NullValueHandling.Ignore)]
        public string AuthorizationEndpoint
        { get; set; }

        [JsonProperty("token_endpoint", NullValueHandling = NullValueHandling.Ignore)]
        public string TokenEndpoint
        { get; set; }

        [JsonProperty("min_cli_version", NullValueHandling = NullValueHandling.Ignore)]
        public dynamic MinCliVersion
        { get; set; }

        [JsonProperty("min_recommended_cli_version", NullValueHandling = NullValueHandling.Ignore)]
        public dynamic MinRecommendedCliVersion
        { get; set; }

        [JsonProperty("app_ssh_endpoint", NullValueHandling = NullValueHandling.Ignore)]
        public string AppSshEndpoint
        { get; set; }

        [JsonProperty("app_ssh_host_key_fingerprint", NullValueHandling = NullValueHandling.Ignore)]
        public string AppSshHostKeyFingerprint
        { get; set; }

        [JsonProperty("app_ssh_oauth_client", NullValueHandling = NullValueHandling.Ignore)]
        public dynamic AppSshOauthClient
        { get; set; }

        [JsonProperty("doppler_logging_endpoint", NullValueHandling = NullValueHandling.Ignore)]
        public string LoggingEndpoint
        { get; set; }

        [JsonProperty("api_version", NullValueHandling = NullValueHandling.Ignore)]
        public string ApiVersion
        { get; set; }

        [JsonProperty("osbapi_version", NullValueHandling = NullValueHandling.Ignore)]
        public string OsbApiVersion
        { get; set; }

        [JsonProperty("routing_endpoint", NullValueHandling = NullValueHandling.Ignore)]
        public string RoutingEndpoint
        { get; set; }
    }
}