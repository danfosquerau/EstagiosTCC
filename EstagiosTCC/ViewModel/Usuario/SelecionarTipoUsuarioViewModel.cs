using EstagiosTCC.Dao;
using EstagiosTCC.Util;
using EstagiosTCC.View.Usuario.Empresa;
using EstagiosTCC.View.Usuario.Estudante;
using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace EstagiosTCC.ViewModel.Usuario
{
    public class SelecionarTipoUsuarioViewModel : BaseViewModel
    {
        public ICommand EstudanteCommand { get; set; }
        public ICommand EmpresaCommand { get; set; }

        public SelecionarTipoUsuarioViewModel()
        {
            Title = "Selecione uma opção";
            EstudanteCommand = new Command(() => OnEstudante());
            EmpresaCommand = new Command(() => OnEmpresa());
        }

        private async void OnEstudante()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                Model.Estudante estudante = new Model.Estudante()
                {
                    Codigo = App.UsuarioLogadoAuth.User.LocalId,
                    Email = App.UsuarioLogadoAuth.User.Email,
                    Nome = App.UsuarioLogadoAuth.User.DisplayName,
                    FotoPerfilUrl = App.UsuarioLogadoAuth.User.PhotoUrl
                };
                App.UsuarioLogado.Tipo = Model.Tipo.Estudante;
                await UsuarioDao.Alterar(App.UsuarioLogado);
                await EstudanteDao.Inserir(estudante, null);

                Application.Current.MainPage = new MenuEstudantePage(View.Usuario.Estudante.MenuItemTipo.DadosCadastrais);
            }
            catch(Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Erro", ex.Message, "Ok");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async void OnEmpresa()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                Model.Empresa empresa = new Model.Empresa()
                {
                    Codigo = App.UsuarioLogadoAuth.User.LocalId,
                    Email = App.UsuarioLogadoAuth.User.Email,
                    NomeEmpresa = App.UsuarioLogadoAuth.User.DisplayName,
                    LogoEmpresa = App.UsuarioLogadoAuth.User.PhotoUrl
                };
                App.UsuarioLogado.Tipo = Model.Tipo.Empresa;
                await UsuarioDao.Alterar(App.UsuarioLogado);
                await EmpresaDao.Inserir(empresa, null) ;

                Application.Current.MainPage = new MenuEmpresaPage(View.Usuario.Empresa.MenuItemTipo.DadosCadastrais);
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
    }
}