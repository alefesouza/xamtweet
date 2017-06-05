using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using XamTweet.Helpers;
using XamTweet.Models;
using XamTweet.Services;

namespace XamTweet.ViewModels
{
    public class MainPageViewModel : BindableBase, INavigationAware
    {
        private AzureService _azureService;
        private INavigationService _navigationService;
        private IPageDialogService _pageDialogService;

        private string _maxId = "";
        private bool _isLoadingMore;

        #region Properties
        private string _tweetText = "";
        public string TweetText
        {
            get { return _tweetText; }
            set { SetProperty(ref _tweetText, value); }
        }

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

        private bool _isNewTweetVisible;

        public bool IsNewTweetVisible
        {
            get { return _isNewTweetVisible; }
            set { SetProperty(ref _isNewTweetVisible, value); }
        }

        private TwitterUser _user;

        public TwitterUser User
        {
            get { return _user; }
            set { SetProperty(ref _user, value); }
        }

        public ObservableCollection<Tweet> Tweets { get; }
        public ObservableCollection<string> LateralItems { get; }
        #endregion

        #region Commands
        public DelegateCommand NewTweetCommand => new DelegateCommand(ExecuteNewTweetCommand);
        public DelegateCommand SendTweetCommand => new DelegateCommand(ExecuteSendTweetCommand, () => TweetText != null && TweetText.Length > 0).ObservesProperty(() => TweetText);
        public DelegateCommand CancelTweetCommand => new DelegateCommand(ExecuteCancelTweetCommand);
        public DelegateCommand RefreshCommand => new DelegateCommand(ExecuteRefreshCommand);
        public DelegateCommand<Tweet> MoreTweetsCommand => new DelegateCommand<Tweet>(ExecuteMoreTweetsCommand);
        public DelegateCommand<Tweet> TweetTappedCommand => new DelegateCommand<Tweet>(ExecuteTweetTappedCommand);
        public DelegateCommand<TwitterUser> TweetProfileTappedCommand => new DelegateCommand<TwitterUser>(ExecuteTweetProfileTappedCommand);
        public DelegateCommand<string> LateralItemTappedCommand => new DelegateCommand<string>(ExecuteLateralItemTappedCommand);
        #endregion

        public MainPageViewModel(INavigationService navigationService, PageDialogService pageDialogService, IDependencyService dependencyService)
        {
            _navigationService = navigationService;
            _pageDialogService = pageDialogService;
            _azureService = dependencyService.Get<AzureService>();

            _azureService.Initialize();

            Tweets = new ObservableCollection<Tweet>();
            LateralItems = new ObservableCollection<string>()
            {
                "Sobre", "Sair"
            };

            GetUser();
        }

        private async void GetUser()
        {
            try
            {
                User = await _azureService.Client.InvokeApiAsync<TwitterUser>("me", HttpMethod.Get, null);
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

        private async void ExecuteLateralItemTappedCommand(string item)
        {
            switch(item)
            {
                case "Sobre":
                    await _navigationService.NavigateAsync("NavigationPage/AboutPage", null, true);
                    break;
                case "Sair":
                    await _azureService.Client.LogoutAsync();
                    Settings.AuthToken = "";
                    Settings.UserId = "";
                    await _pageDialogService.DisplayAlertAsync("Logout realizado com sucesso", "", "OK");
                    await _navigationService.NavigateAsync("app:///NavigationPage/LoginPage");
                    break;
            }
        }

        private void ExecuteNewTweetCommand()
        {
            IsNewTweetVisible = true;
        }

        private async void ExecuteSendTweetCommand()
        {
            IsLoading = true;
            IsNewTweetVisible = false;

            Response response = await _azureService.Client.InvokeApiAsync<Response>("post?status=" + Uri.EscapeDataString(TweetText), HttpMethod.Get, null);

            TweetText = "";

            RefreshCommand.Execute();
        }

        private void ExecuteCancelTweetCommand()
        {
            IsNewTweetVisible = false;
            TweetText = "";
        }

        private async void ExecuteRefreshCommand()
        {
            IsLoading = true;

            Tweets.Clear();
            _maxId = "";

            await GetTweets();

            IsLoading = false;
        }

        private async void ExecuteTweetTappedCommand(Tweet tweet)
        {
            NavigationParameters parameters = new NavigationParameters();
            parameters.Add("tweet", tweet);

            await _navigationService.NavigateAsync("NavigationPage/TweetPage", parameters, true);
        }

        private async void ExecuteTweetProfileTappedCommand(TwitterUser user)
        {
            NavigationParameters parameters = new NavigationParameters();
            parameters.Add("user", user);

            await _navigationService.NavigateAsync("NavigationPage/UserProfilePage", parameters, true);
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
                IsLoading = true;

                await GetTweets();

                IsLoading = false;
            }
        }

        private async Task GetTweets()
        {
            string maxId = "";

            if(!_maxId.Equals(""))
            {
                maxId = "?max_id=" + _maxId;
            }

            try
            {
                Response response = await _azureService.Client.InvokeApiAsync<Response>("timeline" + maxId, HttpMethod.Get, null);

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

                if(retry)
                {
                    await GetTweets();
                }
            }
        }
    }
}
