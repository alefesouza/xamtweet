using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Diagnostics;
using Xamarin.Forms;

namespace XamTweet.ViewModels
{
    public class AboutPageViewModel : BindableBase
    {
        public DelegateCommand<string> LinkCommand => new DelegateCommand<string>(ExecuteLinkCommand);

        public AboutPageViewModel()
        {

        }

        private void ExecuteLinkCommand(string param)
        {
            switch(param)
            {
                case "AlefeSouza":
                    Device.OpenUri(new Uri("http://alefesouza.com"));
                    break;
                case "Angelo":
                    Device.OpenUri(new Uri("https://github.com/angelobelchior"));
                    break;
                case "Alexandre":
                    Device.OpenUri(new Uri("https://github.com/azchohfi"));
                    break;
                case "Valerio":
                    Device.OpenUri(new Uri("https://github.com/Sylix"));
                    break;
                case "WilliamB":
                    Device.OpenUri(new Uri("https://github.com/willsb"));
                    break;
                case "WilliamS":
                    Device.OpenUri(new Uri("https://github.com/williamsrz"));
                    break;
                case "Prism":
                    Device.OpenUri(new Uri("https://www.nuget.org/packages/Prism.Core"));
                    break;
                case "ImageCircle":
                    Device.OpenUri(new Uri("https://www.nuget.org/packages/Xam.Plugins.Forms.ImageCircle"));
                    break;
                case "Settings":
                    Device.OpenUri(new Uri("https://www.nuget.org/packages/Xam.Plugins.Settings/"));
                    break;
                case "Newtonsoft":
                    Device.OpenUri(new Uri("https://www.nuget.org/packages/Newtonsoft.Json"));
                    break;
                case "Azure":
                    Device.OpenUri(new Uri("https://www.nuget.org/packages/Microsoft.Azure.Mobile.Client"));
                    break;
            }
        }
    }
}
