using EstagiosTCC.Dao;
using EstagiosTCC.Model;
using EstagiosTCC.Util;
using EstagiosTCC.View;
using EstagiosTCC.View.Usuario;
using EstagiosTCC.View.Usuario.Empresa;
using EstagiosTCC.View.Usuario.Estudante;
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
        public static Usuario UsuarioLogado;
        /// <summary>
        /// Dados gerais do usuario logado no sistema.
        /// </summary>
        public static Estudante EstudanteDados;
        /// <summary>
        /// Dados gerais do usuario logado no sistema.
        /// </summary>
        public static Empresa EmpresaDados;

        public App()
        {
            InitializeComponent();

            UsuarioLogadoAuth = null;
            UsuarioLogado = null;
            EstudanteDados = null;
            EmpresaDados = null;

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
                    switch (UsuarioLogado.Tipo)
                    {
                        case Tipo.NaoDefinido:
                            _ = Current.MainPage.Navigation.PushModalAsync(new SelecionarTipoUsuarioPage());
                            break;
                        case Tipo.Empresa:
                            EmpresaDados = await EmpresaDao.BuscarPeloCodigo(UsuarioLogadoAuth.User.LocalId);
                            Current.MainPage = new MenuEmpresaPage();
                            break;
                        case Tipo.Estudante:
                            EstudanteDados = await EstudanteDao.BuscarPeloCodigo(UsuarioLogadoAuth.User.LocalId);
                            Current.MainPage = new MenuEstudantePage();
                            break;
                    }
                else
                    MainPage = new NavigationPage(new IntroducaoPage());
            }
        }
    }
}