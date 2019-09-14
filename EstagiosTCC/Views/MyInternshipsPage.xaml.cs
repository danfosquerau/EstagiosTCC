using EstagiosTCC.Models;
using EstagiosTCC.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EstagiosTCC.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MyInternshipsPage : ContentPage
    {
        private bool isPostBack = false;

        private readonly MyInternshipsViewModel ViewModel;

        public MyInternshipsPage()
        {
            InitializeComponent();

            BindingContext = ViewModel = new MyInternshipsViewModel();

            pkrCourse.SelectedIndex = 0;
        }

        private async void LvwInternships_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var tapped = e.Item as Internship;
            await Navigation.PushAsync(new EditInternshipPage(tapped));
        }

        private void BtnAllInternship_Clicked(object sender, System.EventArgs e)
        {
            ViewModel.AllInternshipCommand.Execute(null);
            pkrCourse.SelectedIndex = 0;
        }

        private void PkrCourse_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (pkrCourse.SelectedIndex != 0)
            {
                var item = sender as Picker;
                var selectedItem = item.SelectedItem as Course;
                string uid = selectedItem.Uid;
                if (!string.IsNullOrEmpty(uid))
                    ViewModel.UpdateListInternship(uid);
                else
                    return;
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (isPostBack)
            {
                if (pkrCourse.SelectedIndex != 0)
                {
                    var selectedItem = pkrCourse.SelectedItem as Course;
                    string uid = selectedItem.Uid;
                    if (!string.IsNullOrEmpty(uid))
                        ViewModel.UpdateListInternship(uid);
                    else
                        ViewModel.AllInternshipCommand.Execute(null);
                }
                else
                {
                    ViewModel.AllInternshipCommand.Execute(null);
                }
            }
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            isPostBack = true;
        }
    }
}