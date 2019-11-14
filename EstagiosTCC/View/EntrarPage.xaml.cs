using EstagiosTCC.ViewModel;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EstagiosTCC.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EntrarPage : ContentPage
    {
        private readonly EntrarViewModel viewModel;

        public EntrarPage()
        {
            InitializeComponent();

            BindingContext = viewModel = new EntrarViewModel(this);

            txtEmail.Completed += (object sender, EventArgs e) =>
            {
                txtSenha.Focus();
            };
            txtSenha.Completed += (object sender, EventArgs e) =>
            {
                viewModel.EntrarCommand.Execute(null);
            };
        }
    }
}