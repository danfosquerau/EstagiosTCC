using EstagiosTCC.ViewModel.Estagio;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EstagiosTCC.View.Estagio
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BuscarEstagioPage : ContentPage
    {
        private readonly BuscarEstagioViewModel viewModel;

        private bool flag = false;

        public BuscarEstagioPage()
        {
            InitializeComponent();

            BindingContext = viewModel = new BuscarEstagioViewModel();

            pkrCursos.SelectedIndex = 0;
        }

        private void pkrCursos_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (pkrCursos.SelectedIndex != 0)
            {
                var item = sender as Picker;
                var selectedItem = item.SelectedItem as Model.Curso;
                string codigo = selectedItem.Codigo;

                if (!string.IsNullOrEmpty(codigo))
                    viewModel.AtualizarListaEstagios(codigo);
                else
                    return;
            }
        }

        private void btnTodosEstagios_Clicked(object sender, System.EventArgs e)
        {
            viewModel.TodosEstagiosCommand.Execute(null);
            pkrCursos.SelectedIndex = 0;
        }

        private async void lvwEstagios_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var tapped = e.Item as Model.Estagio;
            await Navigation.PushAsync(new DetalhesEstagioPage(tapped));
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (flag)
            {
                flag = false; 

                if (pkrCursos.SelectedIndex == 0)
                    viewModel.TodosEstagiosCommand.Execute(null);
                else
                    viewModel.AtualizarListaEstagios((pkrCursos.SelectedItem as Model.Curso).Codigo);
            }
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            flag = true;
        }
    }
}