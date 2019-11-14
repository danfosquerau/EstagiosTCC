using EstagiosTCC.Dao;
using EstagiosTCC.Model;
using EstagiosTCC.Util;
using EstagiosTCC.View;
using Firebase.Auth;
using System;
using Xamarin.Forms;

namespace EstagiosTCC
{
    public partial class App : Application
    {
        /// <summary>
        /// Dados de autenticação usuário logado no sistema.
        /// </summary>
        public static FirebaseAuthLink UsuarioLogadoAuth;
        /// <summary>
        /// Dados gerais do usuario logado no sistema.
        /// </summary>
        public static Usuario UsuarioLogadoDados;

        public App()
        {
            InitializeComponent();
            UsuarioLogadoAuth = null;
            UsuarioLogadoDados = null;
            ConnectionDB.InitializeService();
            MainPage = new Page();
            ExisteUsuarioLogado();
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }

        /// <summary>
        /// Verifica se existe algum usuario logado atraves do Properties e ja realiza a Autenticação 
        /// e inicia o todos os serviços do Firebase
        /// </summary>
        private async void ExisteUsuarioLogado()
        {
            bool flag = false;
            
            try
            {
                string[] credencial = Util.Properties.GetLogin();
                
                if (credencial != null)
                {
                    if (await UsuarioDao.EntrarComEmailESenha(credencial[0], credencial[1]))
                        flag = true;
                }
                else
                    Util.Properties.RemoveLogin();
            }
            catch (Exception e)
            {
                await Current.MainPage.DisplayAlert("Erro", e.Message, "Ok");
            }
            finally
            {
                if (flag)
                    MainPage = new MenuPrincipalPage();
                else
                    MainPage = new NavigationPage(new IntroducaoPage());
            }
        }
    }
}