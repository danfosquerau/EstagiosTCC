using EstagiosTCC.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EstagiosTCC.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CreateAccountPage : ContentPage
    {
        private readonly CreateAccountViewModel ViewModel;

        public CreateAccountPage()
        {
            InitializeComponent();

            BindingContext = ViewModel = new CreateAccountViewModel();
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