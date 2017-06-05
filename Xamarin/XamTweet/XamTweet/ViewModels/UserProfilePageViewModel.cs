using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using XamTweet.Models;
using XamTweet.Services;

namespace XamTweet.ViewModels
{
    public class UserProfilePageViewModel : BindableBase, INavigationAware
    {
        private AzureService _azureService;
        private INavigationService _navigationService;
        private IPageDialogService _pageDialogService;

        private string _maxId = "";
        private bool _isLoadingMore;

        #region Properties
        private bool _isLoading;
        public bool IsLoading
        {
            get { return _isLoading; }
            set { SetProperty(ref _isLoading, value); }
        }

        private bool _isNoMoreTweets;

        public bool IsNoMoreTweets
        {
            get { return _isNoMoreTweets; }
            set { SetProperty(ref _isNoMoreTweets, value); }
        }

        private TwitterUser _user;

        public TwitterUser User
        {
            get { return _user; }
            set { SetProperty(ref _user, value); }
        }

        public ObservableCollection<Tweet> Tweets { get; }
        #endregion

        #region Commands
        public DelegateCommand<Tweet> MoreTweetsCommand => new DelegateCommand<Tweet>(ExecuteMoreTweetsCommand);
        public DelegateCommand<Tweet> TweetTappedCommand => new DelegateCommand<Tweet>(ExecuteTweetTappedCommand);
        #endregion

        public UserProfilePageViewModel(INavigationService navigationService, PageDialogService pageDialogService, IDependencyService dependencyService)
        {
            _navigationService = navigationService;
            _pageDialogService = pageDialogService;
            _azureService = dependencyService.Get<AzureService>();

            _azureService.Initialize();

            Tweets = new ObservableCollection<Tweet>();
            User = new TwitterUser();
        }

        private async void ExecuteTweetTappedCommand(Tweet tweet)
        {
            NavigationParameters parameters = new NavigationParameters();
            parameters.Add("tweet", tweet);

            await _navigationService.NavigateAsync("TweetPage", parameters);
        }

        private async void ExecuteMoreTweetsCommand(Tweet lastTweet)
        {
            if (_isLoadingMore || Tweets.Count == 0 || IsNoMoreTweets)
                return;

            if (lastTweet.Equals(Tweets.Last()))
            {
                _isLoadingMore = true;

                await GetTweets();

                _isLoadingMore = false;
            }
        }

        public void OnNavigatedFrom(NavigationParameters parameters)
        {

        }

        public void OnNavigatingTo(NavigationParameters parameters)
        {

        }

        public async void OnNavigatedTo(NavigationParameters parameters)
        {
            if (parameters.GetNavigationMode() == NavigationMode.New)
            {
                User = (TwitterUser)parameters["user"];

                IsLoading = true;

                await GetTweets();

                IsLoading = false;
            }
        }

        private async Task GetTweets()
        {
            string maxId = "";

            if (!_maxId.Equals(""))
            {
                maxId = "&max_id=" + _maxId;
            }

            try
            {
                Response response = await _azureService.Client.InvokeApiAsync<Response>("user?user=" + User.Screen_name.Replace("@", "") + maxId, HttpMethod.Get, null);

                IsNoMoreTweets = !response.More_tweets;

                List<Tweet> tweets = response.Tweets;

                _maxId = tweets.Last().Id_str;

                tweets = tweets.Take(10).ToList();

                foreach (Tweet tweet in tweets)
                {
                    Tweets.Add(tweet);
                }
            }
            catch
            {
                bool retry = await _pageDialogService.DisplayAlertAsync("Ops", "Houve um erro, deseja tentar novamente?", "Sim", "Não");

                if (retry)
                {
                    await GetTweets();
                }
            }
        }
    }
}
