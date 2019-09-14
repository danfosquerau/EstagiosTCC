using EstagiosTCC.Models;
using EstagiosTCC.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EstagiosTCC.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class InternshipDetailPage : ContentPage
    {
        private readonly InternshipDetailViewModel ViewModel;

        public InternshipDetailPage()
        {
            InitializeComponent();

            BindingContext = ViewModel = new InternshipDetailViewModel();
        }

        public InternshipDetailPage(Internship internship)
        {
            InitializeComponent();

            BindingContext = ViewModel = new InternshipDetailViewModel(internship);
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