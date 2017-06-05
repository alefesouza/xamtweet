using Prism.Unity;
using XamTweet.Views;
using Xamarin.Forms;
using XamTweet.Services;
using XamTweet.Helpers;

namespace XamTweet
{
    public partial class App : PrismApplication
    {
        public App(IPlatformInitializer initializer = null) : base(initializer) { }

        protected override void OnInitialized()
        {
            InitializeComponent();

            if (Settings.IsLoggedIn)
            {
                NavigationService.NavigateAsync("MainPage");
            }
            else
            {
                NavigationService.NavigateAsync("NavigationPage/LoginPage");
            }

            DependencyService.Get<AzureService>().Initialize();
        }

        protected override void RegisterTypes()
        {
            Container.RegisterTypeForNavigation<NavigationPage>();
            Container.RegisterTypeForNavigation<MainPage>();
            Container.RegisterTypeForNavigation<TweetPage>();
            Container.RegisterTypeForNavigation<LoginPage>();
            Container.RegisterTypeForNavigation<UserProfilePage>();
            Container.RegisterTypeForNavigation<AboutPage>();
        }
    }
}
