using EstagiosTCC.ViewModel.Usuario.Empresa;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EstagiosTCC.View.Usuario.Empresa
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

        private void lvwEstagios_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var tapped = e.Item as Model.Estagio;
            Navigation.PushAsync(new CadastrarEstagioPage(tapped));
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