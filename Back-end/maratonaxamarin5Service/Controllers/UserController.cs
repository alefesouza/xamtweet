using maratonaxamarin5Service.DataObjects;
using maratonaxamarin5Service.Services;
using Microsoft.Azure.Mobile.Server.Config;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace maratonaxamarin5Service.Controllers
{
    [MobileAppController]
    public class UserController : ApiController
    {
        private string max_id = null;

        public async Task<Response> GetUser(string user)
        {
            string accessToken = Request.Headers.GetValues("X-MS-TOKEN-TWITTER-ACCESS-TOKEN").FirstOrDefault();
            string secret = Request.Headers.GetValues("X-MS-TOKEN-TWITTER-ACCESS-TOKEN-SECRET").FirstOrDefault();

            TwitterService twitterService = new TwitterService(accessToken, secret);

            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("count", "11");
            parameters.Add("screen_name", user);

            if (max_id != null)
            {
                parameters.Add("max_id", max_id);
            }

            var json = await twitterService.GetUser(user, parameters);

            List<Tweet> tweets = JsonConvert.DeserializeObject<List<Tweet>>(json);

            Response response = new Response();
            response.More_tweets = tweets.Count > 10;
            response.Tweets = tweets;

            return response;
        }

        public async Task<Response> GetUser(string user, string max_id)
        {
            this.max_id = max_id;

            return await GetUser(user);
        }
    }
}