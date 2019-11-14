using EstagiosTCC.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EstagiosTCC.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SobrePage : ContentPage
    {
        private readonly SobreViewModel viewModel;

        public SobrePage()
        {
            InitializeComponent();

            BindingContext = viewModel = new SobreViewModel();
        }
    }
}