using EstagiosTCC.Model;
using EstagiosTCC.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EstagiosTCC.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DetalhesEstagioPage : ContentPage
    {
        private readonly DetalhesEstagioViewModel viewModel;

        public DetalhesEstagioPage()
        {
            InitializeComponent();

            BindingContext = viewModel = new DetalhesEstagioViewModel();
        }

        public DetalhesEstagioPage(Estagio estagio)
        {
            InitializeComponent();

            BindingContext = viewModel = new DetalhesEstagioViewModel(estagio);
        }
    }
}