using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using XamTweet.Models;

namespace XamTweet.ViewModels
{
    public class TweetPageViewModel : BindableBase, INavigationAware
    {
        private INavigationService _navigationService;

        #region Properties
        private string _title;

        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        private Tweet _tweet;

        public Tweet Tweet
        {
            get { return _tweet; }
            set { SetProperty(ref _tweet, value); }
        }
        #endregion

        #region Commands
        public DelegateCommand<Tweet> TweetProfileTappedCommand => new DelegateCommand<Tweet>(ExecuteTweetProfileTappedCommand);
        #endregion

        public TweetPageViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        private async void ExecuteTweetProfileTappedCommand(Tweet tweet)
        {
            NavigationParameters parameters = new NavigationParameters();
            parameters.Add("user", tweet.User);

            await _navigationService.NavigateAsync("UserProfilePage", parameters);
        }

        public void OnNavigatedFrom(NavigationParameters parameters)
        {
        }

        public void OnNavigatingTo(NavigationParameters parameters)
        {
        }

        public void OnNavigatedTo(NavigationParameters parameters)
        {
            if (parameters.GetNavigationMode() == NavigationMode.New)
            {
                Tweet = (Tweet)parameters["tweet"];

                Title = "Tweet de " + Tweet.User.Name;
            }
        }
    }
}
