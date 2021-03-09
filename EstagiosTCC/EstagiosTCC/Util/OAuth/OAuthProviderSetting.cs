using EstagiosTCC.Dao;
using EstagiosTCC.Model;
using EstagiosTCC.View.Curso;
using EstagiosTCC.View.Usuario;
using EstagiosTCC.View.Usuario.Empresa;
using EstagiosTCC.View.Usuario.Estudante;
using System;
using Xamarin.Auth;
using Xamarin.Forms;

namespace EstagiosTCC.Util.OAuth
{
    public class OAuthProviderSetting
    {
        public OAuth2Authenticator LoginWithProvider(string provider)
        {
            OAuth2Authenticator auth = null;

            switch (provider)
            {
                case "Google":
                    string clientID = string.Empty;
                    string redirectUrl = string.Empty;
                    if(Device.RuntimePlatform == Device.iOS)
                    {
                        clientID = "366075336365-7hd7au784gckb44v26mdm7eocqb95qth.apps.googleusercontent.com";
                        redirectUrl = "com.googleusercontent.apps.366075336365-7hd7au784gckb44v26mdm7eocqb95qth:/oauth2redirect";
                    }
                    else
                    {
                        clientID = "366075336365-5jab4cd9qs8ivocrh713oatopsu0ffhg.apps.googleusercontent.com";
                        redirectUrl = "com.googleusercontent.apps.366075336365-5jab4cd9qs8ivocrh713oatopsu0ffhg:/oauth2redirect";
                    }

                    auth = new OAuth2Authenticator(
                        clientId: clientID,
                        clientSecret: string.Empty,
                        scope: "https://www.googleapis.com/auth/userinfo.email",
                        authorizeUrl: new Uri("https://accounts.google.com/o/oauth2/auth"),
                        redirectUrl: new Uri(redirectUrl),
                        accessTokenUrl: new Uri("https://www.googleapis.com/oauth2/v4/token"),
                        isUsingNativeUI: true
                        );
                    auth.Completed += Auth_GoogleCompleted;
                    break;
                case "Facebook":
                    auth = new OAuth2Authenticator(
                        clientId: "666453210508831",
                        scope: "email",
                        authorizeUrl: new Uri("https://m.facebook.com/dialog/oauth/"),
                        redirectUrl: new Uri("https://www.facebook.com/connect/login_success.html")
                        );
                    auth.Completed += Auth_FacebookCompleted;
                    break;
            }

            auth.Error += Auth_Error;

            return auth;
        }

        private void Auth_Error(object sender, AuthenticatorErrorEventArgs e)
        {
            Application.Current.MainPage.Navigation.PopToRootAsync();
            Application.Current.MainPage.DisplayAlert("Erro", "Erro na requisição, verifique sua conexão", "Ok");
        }

        private async void Auth_FacebookCompleted(object sender, AuthenticatorCompletedEventArgs e)
        {
            if (e.IsAuthenticated)
            {
                string accessToken = e.Account.Properties["access_token"];

                if (await UsuarioDao.EntrarComOAuth(Firebase.Auth.FirebaseAuthType.Facebook, accessToken))
                    GetUserType();
            }
            else
            {
                await Application.Current.MainPage.Navigation.PopToRootAsync();
                await Application.Current.MainPage.DisplayAlert("Cancelado", "Requisição cancelada", "Ok");
            }
        }

        private async void Auth_GoogleCompleted(object sender, AuthenticatorCompletedEventArgs e)
        {
            if (e.IsAuthenticated)
            {
                string accessToken = e.Account.Properties["access_token"];

                if (await UsuarioDao.EntrarComOAuth(Firebase.Auth.FirebaseAuthType.Google, accessToken))
                    GetUserType();
            }
            else
            {
                await Application.Current.MainPage.Navigation.PopToRootAsync();
                await Application.Current.MainPage.DisplayAlert("Cancelado", "Requisição cancelada", "Ok");
            }
        }
        
        private async void GetUserType()
        {
            switch (App.UsuarioLogado.Tipo)
            {
                case Tipo.NaoDefinido:
                    _ = App.Current.MainPage.Navigation.PushModalAsync(new SelecionarTipoUsuarioPage());
                    break;
                case Tipo.Empresa:
                    App.EmpresaDados = await EmpresaDao.BuscarPeloCodigo(App.UsuarioLogadoAuth.User.LocalId);
                    App.Current.MainPage = new MenuEmpresaPage();
                    break;
                case Tipo.Estudante:
                    App.EstudanteDados = await EstudanteDao.BuscarPeloCodigo(App.UsuarioLogadoAuth.User.LocalId);
                    App.Current.MainPage = new MenuEstudantePage();
                    break;
            }
        }
    }
}