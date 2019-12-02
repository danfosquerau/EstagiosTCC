using EstagiosTCC.Util;
using EstagiosTCC.View;
using EstagiosTCC.View.Usuario;
using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace EstagiosTCC.ViewModel
{
    public class IntroducaoViewModel : BaseViewModel
    {
        public ICommand EntrarComFacebookCommand { get; set; }
        public ICommand EntrarComGoogleCommand { get; set; }
        public ICommand EntrarCommand { get; set; }
        public ICommand CadastroUsuarioCommand { get; set; }

        public IntroducaoViewModel()
        {
            Title = "Introdução";

            EntrarComFacebookCommand = new Command(() => OnFacebook());
            EntrarComGoogleCommand = new Command(() => OnGoogle());
            EntrarCommand = new Command(() => OnEntrar());
            CadastroUsuarioCommand = new Command(() => OnCadastroUsuario());
        }

        private void OnFacebook()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {

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

        private void OnGoogle()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {

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

        private void OnEntrar()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                Application.Current.MainPage.Navigation.PushAsync(new EntrarPage());
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

        private void OnCadastroUsuario()
        {
            if (IsBusy)
                return;

            IsBusy = true;
            try
            {
                Application.Current.MainPage.Navigation.PushAsync(new CadastrarUsuarioPage());
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