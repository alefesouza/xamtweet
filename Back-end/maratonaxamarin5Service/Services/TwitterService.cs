using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace maratonaxamarin5Service.Services
{
    public class TwitterService
    {
        private const string _consumerKey = "";
        private const string _consumerSecret = "";

        private string _accessToken;
        private string _secret;

        public TwitterService(string accessToken, string secret)
        {
            _accessToken = accessToken;
            _secret = secret;
        }

        public async Task<string> GetTimeline(Dictionary<string, string> parameters)
        {
            return await CallApi("statuses/home_timeline.json", parameters);
        }

        public async Task<string> GetUser(string screen_name, Dictionary<string, string> parameters)
        {
            return await CallApi($"statuses/user_timeline.json", parameters);
        }

        public async Task<string> PostTweet(Dictionary<string, string> parameters)
        {
            return await CallApi($"statuses/update.json", parameters, "POST");
        }

        public async Task<string> GetCredentials()
        {
            return await CallApi("account/verify_credentials.json");
        }

        private async Task<string> CallApi(string apiUrl, Dictionary<string, string> parameters = null, string method = "GET")
        {
            string url = $"https://api.twitter.com/1.1/{apiUrl}";

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = GetAuth(url, parameters, out string final_url, method);

                HttpResponseMessage response;

                if (method.Equals("GET"))
                    response = await client.GetAsync(final_url);
                else
                    response = await client.PostAsync(final_url, null);

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsStringAsync();
                }

                return null;
            }
        }

        private AuthenticationHeaderValue GetAuth(string url, Dictionary<string, string> parameters, out string final_url, string method = "GET")
        {
            var oauth_nonce = Convert.ToBase64String(
                new ASCIIEncoding().GetBytes(DateTime.Now.Ticks.ToString()));
            var timeSpan = DateTime.UtcNow
                - new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            var oauth_timestamp = Convert.ToInt64(timeSpan.TotalSeconds).ToString();

            SortedDictionary<string, string> ordered = new SortedDictionary<string, string>();

            ordered.Add("oauth_consumer_key", _consumerKey);
            ordered.Add("oauth_nonce", oauth_nonce);
            ordered.Add("oauth_signature_method", "HMAC-SHA1");
            ordered.Add("oauth_timestamp", oauth_timestamp);
            ordered.Add("oauth_token", _accessToken);
            ordered.Add("oauth_version", "1.0");
            
            final_url = url;

            if (parameters != null) {
                final_url += "?";

                int i = 0;

                foreach (KeyValuePair<string, string> item in parameters)
                {
                    if (i != 0)
                        final_url += "&";

                    final_url += $"{item.Key}={Uri.EscapeDataString(item.Value)}";

                    ordered.Add(item.Key, item.Value);

                    i++;
                }
            }

            string baseString = "";

            int count = 0;

            foreach (KeyValuePair<string, string> item in ordered)
            {
                if (count != 0)
                    baseString += "&";

                baseString += $"{item.Key}={Uri.EscapeDataString(item.Value)}";

                count++;
            }

            baseString = string.Concat(method + "&", Uri.EscapeDataString(url), "&", Uri.EscapeDataString(baseString));

            var compositeKey = string.Concat(Uri.EscapeDataString(_consumerSecret),
                                    "&", Uri.EscapeDataString(_secret));

            string oauth_signature;

            using (HMACSHA1 hasher = new HMACSHA1(ASCIIEncoding.ASCII.GetBytes(compositeKey)))
            {
                oauth_signature = Convert.ToBase64String(
                    hasher.ComputeHash(ASCIIEncoding.ASCII.GetBytes(baseString)));
            }

            var headerFormat = "OAuth oauth_nonce=\"{0}\", oauth_signature_method=\"{1}\", " +
                               "oauth_timestamp=\"{2}\", oauth_consumer_key=\"{3}\", " +
                               "oauth_token=\"{4}\", oauth_signature=\"{5}\", " +
                               "oauth_version=\"{6}\"";

            var authHeader = string.Format(headerFormat,
                                    Uri.EscapeDataString(oauth_nonce),
                                    Uri.EscapeDataString("HMAC-SHA1"),
                                    Uri.EscapeDataString(oauth_timestamp),
                                    Uri.EscapeDataString(_consumerKey),
                                    Uri.EscapeDataString(_accessToken),
                                    Uri.EscapeDataString(oauth_signature),
                                    Uri.EscapeDataString("1.0")
                            );

            return AuthenticationHeaderValue.Parse(authHeader);
        }
    }
}