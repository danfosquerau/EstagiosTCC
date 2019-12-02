using EstagiosTCC.ViewModel.Usuario.Empresa;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EstagiosTCC.View.Usuario.Empresa
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class InicioPage : ContentPage
    {
        private readonly InicioViewModel viewModel;

        public InicioPage()
        {
            InitializeComponent();

            BindingContext = viewModel = new InicioViewModel();
        }
    }
}