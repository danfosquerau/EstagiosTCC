using EstagiosTCC.ViewModel.Estagio;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EstagiosTCC.View.Estagio
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

        public DetalhesEstagioPage(Model.Estagio estagio)
        {
            InitializeComponent();

            BindingContext = viewModel = new DetalhesEstagioViewModel(estagio);
        }
    }
}