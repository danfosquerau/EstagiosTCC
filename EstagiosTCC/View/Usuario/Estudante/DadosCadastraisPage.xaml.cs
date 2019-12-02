using EstagiosTCC.ViewModel.Usuario.Estudante;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EstagiosTCC.View.Usuario.Estudante
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