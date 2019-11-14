using EstagiosTCC.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EstagiosTCC.View
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