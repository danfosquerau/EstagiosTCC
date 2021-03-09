using EstagiosTCC.ViewModel.Usuario;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EstagiosTCC.View.Usuario
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SelecionarTipoUsuarioPage : ContentPage
    {
        private readonly SelecionarTipoUsuarioViewModel viewModel;

        public SelecionarTipoUsuarioPage()
        {
            InitializeComponent();

            BindingContext = viewModel = new SelecionarTipoUsuarioViewModel();
        }

        protected override bool OnBackButtonPressed()
        {
            DisplayAlert("Selecione","Selecione uma opção","Ok");
            return true;
        }
    }
}