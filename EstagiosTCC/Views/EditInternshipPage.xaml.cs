using EstagiosTCC.Models;
using EstagiosTCC.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EstagiosTCC.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditInternshipPage : ContentPage
    {
        private readonly EditInternshipViewModel ViewModel;

        public EditInternshipPage(Internship intership)
        {
            InitializeComponent();

            if(intership == null)
            {
                BindingContext = ViewModel = new EditInternshipViewModel();
                ToolbarItems.Remove(tbiDelete);
            }
            else
            {
                BindingContext = ViewModel = new EditInternshipViewModel(intership);
            }
                
            ViewModel.DisplaySuccessPrompt += () => DisplayAlert("Sucesso", "Comando executado com êxito.", "OK");
            ViewModel.DisplayFailedPrompt += () => DisplayAlert("Erro", "Tente novamente mais tarde.", "OK");
            ViewModel.DisplayDeniedPrompt += () => DisplayAlert("Ação negada", "Não foi possível executar essa ação.", "OK");
        }

        private void LvwCourses_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var tapped = e.Item as Course;
            if(!ViewModel.ListCoursesOfInternship.Contains(tapped))
                ViewModel.ListCoursesOfInternship.Add(tapped);

            lvwCourses.SelectedItem = null;
        }

        private void LvwCoursesOfInternship_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var tapped = e.Item as Course;
            ViewModel.ListCoursesOfInternship.Remove(tapped);
            lvwCoursesOfInternship.SelectedItem = null;
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