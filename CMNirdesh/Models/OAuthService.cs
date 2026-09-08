using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;


namespace CMNirdesh.Models
{
    public class OAuthService
    {
        private static string _token;
        private static DateTime _expiry;

        public async Task<string> GetTokenAsync()
        {
            if (!string.IsNullOrEmpty(_token) && DateTime.Now < _expiry)
                return _token;

            using (var client = new HttpClient())
            {
                client.BaseAddress =
                    new Uri(ConfigurationManager.AppSettings["OAuthBaseUrl"]);

                string basic = Convert.ToBase64String(
                    Encoding.UTF8.GetBytes(
                        ConfigurationManager.AppSettings["OAuthClientId"] + ":" +
                        ConfigurationManager.AppSettings["OAuthClientSecret"]
                    )
                );

                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Basic", basic);

                var form = new FormUrlEncodedContent(new[]
                {
                new KeyValuePair<string,string>("username", ConfigurationManager.AppSettings["OAuthUsername"]),
                new KeyValuePair<string,string>("password", ConfigurationManager.AppSettings["OAuthPassword"]),
                new KeyValuePair<string,string>("grant_type","password"),
                new KeyValuePair<string,string>("scope", ConfigurationManager.AppSettings["OAuthScope"]),
                new KeyValuePair<string,string>("tenantId", ConfigurationManager.AppSettings["OAuthTenantId"]),
                new KeyValuePair<string,string>("userType", ConfigurationManager.AppSettings["OAuthUserType"])
               });

                var response = await client.PostAsync("user/oauth/token", form);
                response.EnsureSuccessStatusCode();

                dynamic json = JsonConvert.DeserializeObject(
                    await response.Content.ReadAsStringAsync());

                _token = json.access_token;
                _expiry = DateTime.Now.AddSeconds((int)json.expires_in - 300);

                return _token;
            }
        }
    }
}