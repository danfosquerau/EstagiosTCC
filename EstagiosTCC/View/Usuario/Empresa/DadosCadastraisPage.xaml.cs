using EstagiosTCC.ViewModel.Usuario.Empresa;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EstagiosTCC.View.Usuario.Empresa
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DadosCadastraisPage : ContentPage
    {
        private readonly DadosCadastraisViewModel viewModel;

        public DadosCadastraisPage()
        {
            InitializeComponent();

            BindingContext = viewModel = new DadosCadastraisViewModel(this);
        }
    }
}