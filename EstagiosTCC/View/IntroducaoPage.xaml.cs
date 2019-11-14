using EstagiosTCC.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EstagiosTCC.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class IntroducaoPage : ContentPage
    {
        private readonly IntroducaoViewModel viewModel;

        public IntroducaoPage()
        {
            InitializeComponent();

            BindingContext = viewModel = new IntroducaoViewModel();
        }
    }
}