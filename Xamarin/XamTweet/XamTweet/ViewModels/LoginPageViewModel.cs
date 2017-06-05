using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using System.Threading.Tasks;
using XamTweet.Helpers;
using XamTweet.Services;

namespace XamTweet.ViewModels
{
    public class LoginPageViewModel : BindableBase
    {
        private INavigationService _navigationService;
        private PageDialogService _pageDialogService;
        private AzureService _azureService;

        private bool _isBusy;

        public DelegateCommand LoginCommand => new DelegateCommand(async () => await ExecuteLoginCommandAsync());

        public LoginPageViewModel(INavigationService navigationService, PageDialogService pageDialogService, IDependencyService dependencyService)
        {
            _navigationService = navigationService;
            _pageDialogService = pageDialogService;
            _azureService = dependencyService.Get<AzureService>();
        }

        private async Task ExecuteLoginCommandAsync()
        {
            if (_isBusy || !(await LoginAsync()))
                return;
            else
            {
                await Task.Delay(300);
                await _navigationService.NavigateAsync("app:///MainPage");
            }
            _isBusy = false;
        }

        public async Task<bool> LoginAsync()
        {
            _isBusy = true;

            if (Settings.IsLoggedIn)
                return await Task.FromResult(true);

            bool result = await _azureService.LoginAsync();

            if(!result)
            {
                await _pageDialogService.DisplayAlertAsync("Houve um erro ao fazer login", "Tente novamente", "OK");
            }

            return result;
        }
    }
}
