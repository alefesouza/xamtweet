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
    public class PostController : ApiController
    {
        public async Task<Tweet> GetPost(string status)
        {
            string accessToken = Request.Headers.GetValues("X-MS-TOKEN-TWITTER-ACCESS-TOKEN").FirstOrDefault();
            string secret = Request.Headers.GetValues("X-MS-TOKEN-TWITTER-ACCESS-TOKEN-SECRET").FirstOrDefault();

            TwitterService twitterService = new TwitterService(accessToken, secret);

            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("status", status);

            var json = await twitterService.PostTweet(parameters);

            Tweet tweet = JsonConvert.DeserializeObject<Tweet>(json);

            return tweet;
        }
    }
}