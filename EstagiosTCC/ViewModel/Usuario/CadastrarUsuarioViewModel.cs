using EstagiosTCC.Dao;
using EstagiosTCC.Util;
using EstagiosTCC.View.Usuario;
using EstagiosTCC.View.Usuario.Empresa;
using EstagiosTCC.View.Usuario.Estudante;
using System;
using System.ComponentModel.DataAnnotations;
using System.Windows.Input;
using Xamarin.Forms;

namespace EstagiosTCC.ViewModel.Usuario
{
    public class CadastrarUsuarioViewModel : BaseViewModel
    {
        private readonly Page page;

        public ICommand ContinuarCommand { get; set; }
        public ICommand CancelarCommand { get; set; }

        private UsuarioAux _usuario;
        public UsuarioAux Usuario
        {
            get { return _usuario; }
            set { SetProperty(ref _usuario, value); }
        }

        public CadastrarUsuarioViewModel(Page page)
        {
            Title = "Cadastre-se";
            Usuario = new UsuarioAux();
            ContinuarCommand = new Command(() => OnContinuar());
            CancelarCommand = new Command(() => OnCancelar());
            this.page = page;
        }

        private async void OnContinuar()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                if (!ValidationHelper.IsFormValid(Usuario, page, true))
                    return;

                if (await UsuarioDao.Inserir(Usuario.Email, Usuario.Senha))
                {
                    if (Usuario.Lembrar)
                        Properties.SaveLogin(Usuario.Email, Usuario.Senha);

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

    public class UsuarioAux
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Informe o email.")]
        [EmailAddress(ErrorMessage = "Email inválido.")]
        public string Email { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Informe a senha.")]
        [StringLength(20, ErrorMessage = "Tamanho de senha inválido. Tente entre 6 e 20 caracteres.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Senha { get; set; }
        [Required(ErrorMessage = "Informe o confirmar senha.")]
        [StringLength(20, ErrorMessage = "Tamanho de senha inválido. Tente entre 6 e 20 caracteres.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Compare("Senha",ErrorMessage ="A senha não confere.")]
        public string ConfirmarSenha { get; set; }
        public bool Lembrar { get; set; }

        public UsuarioAux()
        {
            Email = string.Empty;
            Senha = string.Empty;
            ConfirmarSenha = string.Empty;
            Lembrar = false;
        }
    }
}