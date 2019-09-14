using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace EstagiosTCC.ViewModels
{
    public class AboutViewModel : BaseViewModel
    {
        public ICommand OpenWebCommand { get; set; }

        private string _appName = string.Empty;
        public string AppName
        {
            get { return _appName; }
            set { SetProperty(ref _appName, value); }
        }
        private string _appVersion = string.Empty;
        public string AppVersion
        {
            get { return _appVersion; }
            set { SetProperty(ref _appVersion, value); }
        }

        public AboutViewModel()
        {
            Title = "Sobre";
            AppName = "Estágios TCC";
            AppVersion = "1.0beta";

            OpenWebCommand = new Command(() => Device.OpenUri(new Uri("https://xamarin.com/platform")));
        }
    }
}