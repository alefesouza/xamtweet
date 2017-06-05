using XamTweet.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using XamTweet.UWP.Authentication;
using XamTweet.Helpers;

[assembly: Xamarin.Forms.Dependency(typeof(SocialAuthentication))]
namespace XamTweet.UWP.Authentication
{
    class SocialAuthentication : IAuthentication
    {
        public async Task<MobileServiceUser> LoginAsync(MobileServiceClient client, MobileServiceAuthenticationProvider provider, IDictionary<string, string> parameters = null)
        {
            try
            {
                var user = await client.LoginAsync(provider);

                Settings.AuthToken = user?.MobileServiceAuthenticationToken ?? string.Empty;
                Settings.UserId = user?.UserId;

                return user;
            }
            catch (Exception e)
            {
                //todo log error
                throw;
            }
        }
    }
}