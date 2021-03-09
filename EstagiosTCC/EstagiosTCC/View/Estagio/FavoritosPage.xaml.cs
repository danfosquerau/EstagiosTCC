using EstagiosTCC.View.Estagio;
using EstagiosTCC.ViewModel.Estagio;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EstagiosTCC.View.Usuario.Estudante
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FavoritosPage : ContentPage
    {
        private readonly FavoritosViewModel viewModel;

        private bool flag = false;

        public FavoritosPage()
        {
            InitializeComponent();

            BindingContext = viewModel = new FavoritosViewModel();
        }

        private void lvwEstagios_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var tapped = e.Item as Model.Estagio;
            Navigation.PushAsync(new DetalhesEstagioPage(tapped));
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (flag)
            {
                flag = false;

                viewModel.CarregarEstagios();
            }
                
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            flag = true;
        }
    }
}