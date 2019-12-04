using EstagiosTCC.Util;
using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace EstagiosTCC.ViewModel.Usuario
{
    public class RecuperarSenhaViewModel : BaseViewModel
    {
        public ICommand ContinuarCommand { get; set; }
        public ICommand CancelarCommand { get; set; }

        private string _email;
        public string Email
        {
            get { return _email; }
            set { SetProperty(ref _email, value); }
        }

        private string _emailError = string.Empty;
        public string EmailError
        {
            get { return _emailError; }
            set { SetProperty(ref _emailError, value); }
        }

        private bool _emailErrorVisible = false;
        public bool EmailErrorVisible
        {
            get { return _emailErrorVisible; }
            set { SetProperty(ref _emailErrorVisible, value); }
        }

        public RecuperarSenhaViewModel()
        {
            Title = "Recuperar a senha";
            ContinuarCommand = new Command(() => OnContinuar());
            CancelarCommand = new Command(() => OnCancelar());
        }

        private async void OnContinuar()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                EmailError = string.Empty;
                EmailErrorVisible = false;

                if (string.IsNullOrEmpty(Email))
                {
                    EmailError = "Informe o email.";
                    EmailErrorVisible = true;

                    return;
                }

                if (!Util.Validation.MyValidation.IsEmail(Email))
                {
                    EmailError = "Email inválido.";
                    EmailErrorVisible = true;

                    return;
                }

                await ConnectionDB.Authentication.SendPasswordResetEmailAsync(Email);

                await Application.Current.MainPage.DisplayAlert("Ok", "Email enviado.", "Ok");

                await Application.Current.MainPage.Navigation.PopAsync();
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Erro", ex.Message, "Ok");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private void OnCancelar()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                Application.Current.MainPage.Navigation.PopAsync();
            }
            catch (Exception ex)
            {
                Application.Current.MainPage.DisplayAlert("Erro", ex.Message, "Ok");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}