using System.Collections.Generic;
using XamTweet.iOS.Authentication;
using XamTweet.Authentication;
using Microsoft.WindowsAzure.MobileServices;
using System.Threading.Tasks;
using XamTweet.Helpers;
using System;

[assembly: Xamarin.Forms.Dependency(typeof(SocialAuthentication))]
namespace XamTweet.iOS.Authentication
{
    public class SocialAuthentication : IAuthentication
    {
        public async Task<MobileServiceUser> LoginAsync(MobileServiceClient client, MobileServiceAuthenticationProvider provider, IDictionary<string, string> parameters = null)
        {
            try
            {
                var current = GetController();
                var user = await client.LoginAsync(current, provider);

                Settings.AuthToken = user?.MobileServiceAuthenticationToken ?? string.Empty;

                return user;
            }
            catch (Exception e)
            {
                //Todo log error
                throw;
            }
        }

        private UIKit.UIViewController GetController()
        {
            var window = UIKit.UIApplication.SharedApplication.KeyWindow;
            var root = window.RootViewController;

            if (root == null) return null;

            var current = root;
            while (current.PresentedViewController != null)
            {
                current = current.PresentedViewController;
            }

            return current;

        }
    }
}