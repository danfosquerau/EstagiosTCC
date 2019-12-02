using EstagiosTCC.ViewModel.Usuario;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EstagiosTCC.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RecuperarSenhaPage : ContentPage
    {
        private readonly RecuperarSenhaViewModel viewModel;

        public RecuperarSenhaPage()
        {
            InitializeComponent();

            BindingContext = viewModel = new RecuperarSenhaViewModel();
        }
    }
}