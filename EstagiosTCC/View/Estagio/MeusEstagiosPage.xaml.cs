using EstagiosTCC.ViewModel.Estagio;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EstagiosTCC.View.Usuario.Empresa
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MeusEstagiosPage : ContentPage
    {
        private readonly MeusEstagiosViewModel viewModel;

        private bool flag = false;

        public MeusEstagiosPage()
        {
            InitializeComponent();

            BindingContext = viewModel = new MeusEstagiosViewModel();
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