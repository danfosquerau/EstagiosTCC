using EstagiosTCC.ViewModel.Usuario;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EstagiosTCC.View.Usuario
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CadastrarUsuarioPage : ContentPage
    {
        private readonly CadastrarUsuarioViewModel viewModel;

        public CadastrarUsuarioPage()
        {
            InitializeComponent();

            BindingContext = viewModel = new CadastrarUsuarioViewModel(this);

            txtEmail.Completed += (object sender, EventArgs e) =>
            {
                txtSenha.Focus();
            };
            txtSenha.Completed += (object sender, EventArgs e) =>
            {
                txtConfirmarSenha.Focus();
            };
            txtConfirmarSenha.Completed += (object sender, EventArgs e) =>
            {
                viewModel.ContinuarCommand.Execute(null);
            };
        }
    }
}