using EstagiosTCC.ViewModel;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EstagiosTCC.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PerfilPage : ContentPage
    {
        private readonly PerfilViewModel viewModel;

        public PerfilPage()
        {
            InitializeComponent();
            txtNome.Focus();
            BindingContext = viewModel = new PerfilViewModel(this);
            txtNome.Completed += (object sender, EventArgs e) =>
            {
                viewModel.SalvarCommand.Execute(null);
            };
        }
    }
}