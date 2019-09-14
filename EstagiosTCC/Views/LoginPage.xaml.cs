using EstagiosTCC.ViewModels;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EstagiosTCC.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        private readonly LoginViewModel ViewModel;

        public LoginPage()
        {
            InitializeComponent();

            BindingContext = ViewModel = new LoginViewModel();

            ViewModel.DisplayFailedPrompt += () => DisplayAlert("Erro", "Login Inválido, Tente Denovo", "OK");

            txtEmail.Completed += (object sender, EventArgs e) =>
            {
                txtPassword.Focus();
            };
            txtPassword.Completed += (object sender, EventArgs e) =>
            {
                ViewModel.LoginCommand.Execute(null);
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