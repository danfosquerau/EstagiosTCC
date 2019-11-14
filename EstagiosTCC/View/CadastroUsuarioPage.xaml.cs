using EstagiosTCC.ViewModel;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EstagiosTCC.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CadastroUsuarioPage : ContentPage
    {
        private readonly CadastroUsuarioViewModel viewModel;

        public CadastroUsuarioPage()
        {
            InitializeComponent();

            BindingContext = viewModel = new CadastroUsuarioViewModel(this);

            txtNome.Completed += (object sender, EventArgs e) =>
            {
                txtEmail.Focus();
            };
            txtEmail.Completed += (object sender, EventArgs e) =>
            {
                txtSenha.Focus();
            };
            txtSenha.Completed += (object sender, EventArgs e) =>
            {
                viewModel.SalvarCommand.Execute(null);
            };
        }
    }
}