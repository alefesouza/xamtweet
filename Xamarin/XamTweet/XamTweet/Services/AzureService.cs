using Microsoft.WindowsAzure.MobileServices;
using XamTweet.Helpers;
using Xamarin.Forms;
using System.Threading.Tasks;
using XamTweet.Authentication;

[assembly: Dependency(typeof(XamTweet.Services.AzureService))]
namespace XamTweet.Services
{
    public class AzureService
    {
        public const string APP_URL = "https://maratonaxamarin5.azurewebsites.net/";

        public MobileServiceClient Client { get; set; } = null;

        public static bool UseAuth { get; set; } = false;

        public void Initialize()
        {
            Client = new MobileServiceClient(APP_URL);

            if (!string.IsNullOrWhiteSpace(Settings.AuthToken) && !string.IsNullOrWhiteSpace(Settings.UserId))
            {
                Client.CurrentUser = new MobileServiceUser(Settings.UserId)
                {
                    MobileServiceAuthenticationToken = Settings.AuthToken
                };
            }
        }

        public async Task<bool> LoginAsync()
        {
            Initialize();

            try
            {
                var auth = DependencyService.Get<IAuthentication>();
                var user = await auth.LoginAsync(Client, MobileServiceAuthenticationProvider.Twitter);

                if (user == null)
                {
                    Settings.AuthToken = string.Empty;
                    Settings.UserId = string.Empty;

                    return false;
                }
                else
                {
                    Settings.AuthToken = user.MobileServiceAuthenticationToken;
                    Settings.UserId = user.UserId;
                }

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}