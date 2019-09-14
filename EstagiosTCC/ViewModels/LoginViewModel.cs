using EstagiosTCC.Views;
using System;
using System.Diagnostics;
using System.Windows.Input;
using Xamarin.Forms;

namespace EstagiosTCC.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        public ICommand LoginCommand { get; set; }
        public ICommand CreateAccountCommand { get; set; }
        public ICommand RecoveryPasswordCommand { get; set; }

        public Action DisplayInvalidLoginPrompt;

        private string email = string.Empty;
        public string Email
        {
            get { return email; }
            set { SetProperty(ref email, value); }
        } 
        private string password = string.Empty;
        public string Password
        {
            get { return password; }
            set { SetProperty(ref password, value); }
        }

        public LoginViewModel()
        {
            Title = "Login";
            LoginCommand = new Command(() => OnLogin());
            CreateAccountCommand = new Command(() => OnCreateAccount());
            RecoveryPasswordCommand = new Command(() => OnRecoveryPassword());
        }


        public void OnLogin()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                //Login Valido
                MessagingCenter.Send<object>(this, App.EVENT_LOGIN);
                //Login Invalido
                //DisplayInvalidLoginPrompt();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }

        }

        public async void OnCreateAccount()
        {
            await Application.Current.MainPage.Navigation.PushAsync(new CreateAccountPage());
        }

        public async void OnRecoveryPassword()
        {
            await Application.Current.MainPage.Navigation.PushAsync(new RecoveryPasswordPage());
        }
    }
}