using EstagiosTCC.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;

namespace EstagiosTCC.Views
{
    [DesignTimeVisible(false)]
    public partial class AboutPage : ContentPage
    {
        private readonly AboutViewModel ViewModel;

        public AboutPage()
        {
            InitializeComponent();

            BindingContext = ViewModel = new AboutViewModel();
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