using EstagiosTCC.ViewModels;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EstagiosTCC.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CoursePage : ContentPage
    {
        private readonly CourseViewModel ViewModel;

        public CoursePage()
        {
            InitializeComponent();

            BindingContext = ViewModel = new CourseViewModel();

            ViewModel.DisplaySuccessPrompt += () => DisplayAlert("Sucesso", "Comando executado com êxito.", "OK");
            ViewModel.DisplayFailedPrompt += () => DisplayAlert("Erro", "Tente novamente mais tarde.", "OK");
            ViewModel.DisplayDeniedPrompt += () => DisplayAlert("Ação negada", "Não foi possível executar essa ação.", "OK");

            txtCourseName.Completed += (object sender, EventArgs e) =>
            {
                ViewModel.SaveCommand.Execute(null);
            };
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