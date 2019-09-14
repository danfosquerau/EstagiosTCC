using EstagiosTCC.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EstagiosTCC.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RecoveryPasswordPage : ContentPage
    {
        private readonly RecoveryPasswordViewModel ViewModel;

        public RecoveryPasswordPage()
        {
            InitializeComponent();

            BindingContext = ViewModel = new RecoveryPasswordViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
        }
    }
}