using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Security;
using System.Threading;
using System.Threading.Tasks;
using CommandLine;
using CommandLine.Text;
using CsvHelper;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OAuth2ClientHandler;
using OAuth2ClientHandler.Authorizer;
using PushReport.CloudFoundry.Model;

namespace PushReport
{
    class Program
    {
        private static string DateFormat = "yyyy'-'MM'-'dd'T'HH':'mm':'ss'Z'";

        public class Options
        {
            [Option('a', "api", Required = true, HelpText = "The CF API Endpoint")]
            public string ApiEndpoint { get; set; }

            [Option('u', "user", Required = true, HelpText = "Sets the user name to use to authenticate to CF")]
            public string User { get; set; }

            [Option('p', "password", Required = false, HelpText = "Sets the password to use to authenticate to CF")]
            public string Password { get; set; }

            [Option('s', "start-date", Required = false, HelpText = "Sets start date for the report.  Default is beginning of the current month.")]
            public DateTime StartDate { get; set; } = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

            [Option('e', "end-date", Required = false, HelpText = "Sets end date for the report.  Default is the current date time.")]
            public DateTime EndDate { get; set; } = DateTime.Now;

            [Option('k', "skip-ssl-validation", Required = false, HelpText = "Skips SSL certificate validation.")]
            public bool SkipSSL { get; set; } = false;

        }
        public static int Main(string[] args)
        {
            int errorCode = 0;
            Parser.Default.ParseArguments<Options>(args)
            .WithParsed<Options>( o =>
            {
                if(String.IsNullOrEmpty(o.Password))
                {
                    o.Password = SecureStringToString(GetPasswordFromConsole("Password: "));
                }

                if(o.SkipSSL)
                {
                    System.Net.ServicePointManager.ServerCertificateValidationCallback = ((sender, certificate, chain, sslPolicyErrors) => true);
                }
                
                GetInfoResponse infoResponse = null;
                using(var httpClient = new HttpClient())
                {
                    var response = httpClient.GetAsync(String.Format("{0}/v2/info", o.ApiEndpoint)).Result;
                    infoResponse = DeserializeJson<GetInfoResponse>(response.Content.ReadAsStringAsync().Result);
                }

                var options = new OAuthHttpHandlerOptions
                {
                    AuthorizerOptions = new AuthorizerOptions
                    {
                        AuthorizeEndpointUrl = new Uri(infoResponse.AuthorizationEndpoint),
                        TokenEndpointUrl = new Uri(String.Format("{0}/oauth/token", infoResponse.TokenEndpoint)),
                        ClientId = "cf",
                        ClientSecret = "",
                        Username = o.User,
                        Password = o.Password,
                        GrantType = GrantType.ResourceOwnerPasswordCredentials,
                        CredentialTransportMethod = CredentialTransportMethod.FormAuthenticationCredentials
                    }
                };

                List<Tuple<string,string,string, DateTime>> report = new List<Tuple<string,string,string,DateTime>>();

                using(var httpClient = new HttpClient(new OAuthHttpHandler(options)))
                {
                    var builder = new UriBuilder(String.Format("{0}/v2/events", o.ApiEndpoint));
                    using(var content = new FormUrlEncodedContent(new [] {
                        new KeyValuePair<string,string> ("q", "timestamp>="+o.StartDate.ToUniversalTime().ToString(DateFormat)),
                        new KeyValuePair<string,string> ("q", "timestamp<"+o.EndDate.ToUniversalTime().ToString(DateFormat)),
                        new KeyValuePair<string,string> ("q", "type:audit.app.build.create"),
                    })) {
                        builder.Query = content.ReadAsStringAsync().Result;
                    }

                    PagedResponse<Event> pagedResponse = DeserializeJson<PagedResponse<Event>>(httpClient.GetAsync(builder.ToString()).Result.Content.ReadAsStringAsync().Result);

                    while(pagedResponse != null && pagedResponse.TotalResults != 0)
                    {
                        foreach (var resource in pagedResponse.Resources)
                        {

                            report.Add(
                                Tuple.Create(
                                    GetOrgNameForGuid(httpClient, o.ApiEndpoint, resource.Entity.OrganizationGuid), 
                                    GetSpaceNameForGuid(httpClient, o.ApiEndpoint, resource.Entity.SpaceGuid), 
                                    resource.Entity.ActeeName, 
                                    resource.Entity.Timestamp 
                                )
                            );
                        }
                        if(pagedResponse.NextUrl != null)
                        {
                            pagedResponse = DeserializeJson<PagedResponse<Event>>(httpClient.GetAsync(o.ApiEndpoint+pagedResponse.NextUrl).Result.Content.ReadAsStringAsync().Result);
                        }
                        else pagedResponse = null;
                    }
                }

                OutputReport(report);
            });

            return errorCode;
        }

        private static void OutputReport(List<Tuple<string,string,string, DateTime>> values)
        {
            using (var csvWriter = new CsvWriter(Console.Out))
            {
                csvWriter.Configuration.ShouldQuote = (field, context) => true;
                csvWriter.WriteField("Organization", true);
                csvWriter.WriteField("Space");
                csvWriter.WriteField("Application");
                csvWriter.WriteField("Push Date");
                csvWriter.NextRecord();

                foreach( var value in values )
                {
                    csvWriter.WriteField(value.Item1);
                    csvWriter.WriteField(value.Item2);
                    csvWriter.WriteField(value.Item3);
                    csvWriter.WriteField(value.Item4);
                    csvWriter.NextRecord();
                }
                
                Console.Out.Flush();
            }
        }

        private static Dictionary<string,Organization> orgCache = new Dictionary<string,Organization>();
        private static string GetOrgNameForGuid(HttpClient client, string apiEndpoint, string orgGuid)
        {
            Organization org = GetNameForEntityCached<Organization>(client, apiEndpoint, "/v2/organizations/", orgCache, orgGuid);
            if(org == null) return orgGuid;
            else return org.Name;
        }

        private static Dictionary<string,Space> spaceCache = new Dictionary<string,Space>();
        private static string GetSpaceNameForGuid(HttpClient client, string apiEndpoint, string spaceGuid)
        {
            Space space = GetNameForEntityCached<Space>(client, apiEndpoint, "/v2/spaces/", spaceCache, spaceGuid);
            if(space == null) return spaceGuid;
            else return space.Name;
        }

        private static T GetNameForEntityCached<T>(HttpClient client, string apiEndpoint, string apiCall, Dictionary<string, T> cache, string guid)
        {
            T entity;
            cache.TryGetValue(guid, out entity);
            if(entity == null)
            {
                entity = DeserializeJson<Resource<T>>(client.GetAsync(apiEndpoint+apiCall+guid).Result.Content.ReadAsStringAsync().Result).Entity;
                cache[guid] = entity;
            }
            return entity;
        }

        private static SecureString GetPasswordFromConsole(String displayMessage) {
            SecureString pass = new SecureString();
            Console.Write(displayMessage);
            ConsoleKeyInfo key;

            do {
                key = Console.ReadKey(true);

                // Backspace Should Not Work
                if (!char.IsControl(key.KeyChar)) {
                    pass.AppendChar(key.KeyChar);
                    Console.Write("*");
                } else {
                    if (key.Key == ConsoleKey.Backspace && pass.Length > 0) {
                        pass.RemoveAt(pass.Length - 1);
                        Console.Write("\b \b");
                    }
                }
            }
            // Stops Receving Keys Once Enter is Pressed
            while (key.Key != ConsoleKey.Enter);
            Console.WriteLine();
            return pass;
        }

        private static String SecureStringToString(SecureString value)
        {
            IntPtr bstr = Marshal.SecureStringToBSTR(value);

            try
            {
                return Marshal.PtrToStringBSTR(bstr);
            }
            finally
            {
                Marshal.FreeBSTR(bstr);
            }
        }

        private static JsonSerializerSettings jsonSettings = new JsonSerializerSettings
        {
            DateParseHandling = DateParseHandling.None
        };

        private static T DeserializeJson<T>(string value)
        {
            using (StringReader stringReader = new StringReader(value))
            {
                using (JsonReader reader = new JsonTextReader(stringReader))
                {
                    reader.DateParseHandling = DateParseHandling.None;
                    var obj = JObject.Load(reader);
                    return JsonConvert.DeserializeObject<T>(value.ToString(), jsonSettings);
                }
            }
        }
    }
}
