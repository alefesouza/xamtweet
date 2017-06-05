using maratonaxamarin5Service.DataObjects;
using maratonaxamarin5Service.Services;
using Microsoft.Azure.Mobile.Server.Config;
using Newtonsoft.Json;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace maratonaxamarin5Service.Controllers
{
    [MobileAppController]
    public class MeController : ApiController
    {
        public async Task<TwitterUser> GetMe()
        {
            string accessToken = Request.Headers.GetValues("X-MS-TOKEN-TWITTER-ACCESS-TOKEN").FirstOrDefault();
            string secret = Request.Headers.GetValues("X-MS-TOKEN-TWITTER-ACCESS-TOKEN-SECRET").FirstOrDefault();

            TwitterService twitterService = new TwitterService(accessToken, secret);

            var json = await twitterService.GetCredentials();

            TwitterUser user = JsonConvert.DeserializeObject<TwitterUser>(json);

            return user;
        }
    }
}