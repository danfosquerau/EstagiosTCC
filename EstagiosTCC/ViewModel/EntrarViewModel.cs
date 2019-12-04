using EstagiosTCC.Dao;
using EstagiosTCC.Util;
using EstagiosTCC.View;
using EstagiosTCC.View.Usuario;
using EstagiosTCC.View.Usuario.Empresa;
using EstagiosTCC.View.Usuario.Estudante;
using System;
using System.ComponentModel.DataAnnotations;
using System.Windows.Input;
using Xamarin.Forms;

namespace EstagiosTCC.ViewModel
{
    public class EntrarViewModel : BaseViewModel
    {
        private readonly Page page;

        public ICommand EntrarCommand { get; set; }
        public ICommand RecuperarSenhaCommand { get; set; }

        private CredencialAux _credencial;
        public CredencialAux Credencial
        {
            get { return _credencial; }
            set { SetProperty(ref _credencial, value); }
        }

        public EntrarViewModel(Page page)
        {
            Title = "Entrar";
            Credencial = new CredencialAux();
            this.page = page;
            EntrarCommand = new Command(() => OnEntrar());
            RecuperarSenhaCommand = new Command(() => OnRecuperarSenha());
        }

        private async void OnEntrar()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                if (!ValidationHelper.IsFormValid(Credencial, page))
                    return;

                if (await UsuarioDao.EntrarComEmailESenha(Credencial.Email, Credencial.Senha))
                {
                    if (Credencial.Lembrar)
                        Properties.SaveLogin(Credencial.Email, Credencial.Senha);

                    switch (App.UsuarioLogado.Tipo)
                    {
                        case Model.Tipo.NaoDefinido:
                            _ = Application.Current.MainPage.Navigation.PushModalAsync(new SelecionarTipoUsuarioPage());
                            break;
                        case Model.Tipo.Empresa:
                            App.EmpresaDados = await EmpresaDao.BuscarPeloCodigo(App.UsuarioLogadoAuth.User.LocalId);
                            Application.Current.MainPage = new MenuEmpresaPage();
                            break;
                        case Model.Tipo.Estudante:
                            App.EstudanteDados = await EstudanteDao.BuscarPeloCodigo(App.UsuarioLogadoAuth.User.LocalId);
                            Application.Current.MainPage = new MenuEstudantePage();
                            break;
                    }
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Erro", "Dados inválidos", "Ok");
                }
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

        private void OnRecuperarSenha()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                Application.Current.MainPage.Navigation.PushAsync(new RecuperarSenhaPage());
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

    public class CredencialAux
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Informe o email.")]
        [EmailAddress(ErrorMessage = "Email inválido.")]
        public string Email { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Informe a senha.")]
        [MinLength(6, ErrorMessage = "Tamanho de senha inválido. Tente entre 6 e 20 caracteres")]
        [MaxLength(20, ErrorMessage = "Tamanho de senha inválido. Tente entre 6 e 20 caracteres")]
        public string Senha { get; set; }
        public bool Lembrar { get; set; }

        public CredencialAux()
        {
            Email = string.Empty;
            Senha = string.Empty;
            Lembrar = false;
        }
    }
}