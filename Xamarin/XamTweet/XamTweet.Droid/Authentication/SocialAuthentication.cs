using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using XamTweet.Authentication;
using XamTweet.Droid.Authentication;
using XamTweet.Helpers;
using Xamarin.Forms;

[assembly: Xamarin.Forms.Dependency(typeof(SocialAuthentication))]
namespace XamTweet.Droid.Authentication
{
    class SocialAuthentication : IAuthentication
    {
        public async Task<MobileServiceUser> LoginAsync(MobileServiceClient client, MobileServiceAuthenticationProvider provider, IDictionary<string, string> parameters = null)
        {
            try
            {
                var user = await client.LoginAsync(Forms.Context, provider);

                Settings.AuthToken = user?.MobileServiceAuthenticationToken ?? string.Empty;
                Settings.UserId = user?.UserId;

                return user;
            }
            catch (Exception e)
            {
                //Todo log error
                throw;
            }
        }
    }
}