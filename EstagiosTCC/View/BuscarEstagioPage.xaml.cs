using EstagiosTCC.Model;
using EstagiosTCC.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EstagiosTCC.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BuscarEstagioPage : ContentPage
    {
        private readonly BuscarEstagioViewModel viewModel;

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
                var selectedItem = item.SelectedItem as Curso;
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
            var tapped = e.Item as Estagio;
            await Navigation.PushAsync(new DetalhesEstagioPage(tapped));
        }
    }
}