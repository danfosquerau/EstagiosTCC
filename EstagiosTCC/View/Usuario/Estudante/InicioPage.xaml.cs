using EstagiosTCC.View.Estagio;
using EstagiosTCC.ViewModel.Usuario.Estudante;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EstagiosTCC.View.Usuario.Estudante
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class InicioPage : ContentPage
    {
        private readonly InicioViewModel viewModel;

        private bool flag = false;

        public InicioPage()
        {
            InitializeComponent();

            BindingContext = viewModel = new InicioViewModel();
        }

        private async void lvwEstagios_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var tapped = e.Item as Model.Estagio;
            await Navigation.PushAsync(new DetalhesEstagioPage(tapped));

            lvwEstagios.SelectedItem = null;
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