using EstagiosTCC.Model;
using EstagiosTCC.ViewModel.Estagio;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EstagiosTCC.View.Usuario.Empresa
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CadastrarEstagioPage : ContentPage
    {
        private readonly CadastrarEstagioViewModel viewModel;

        public CadastrarEstagioPage(Model.Estagio estagio)
        {
            InitializeComponent();

            if (estagio == null)
            {
                BindingContext = viewModel = new CadastrarEstagioViewModel(this, pkrStatus);
            }
            else
                BindingContext = viewModel = new CadastrarEstagioViewModel(this, estagio, picker: pkrStatus);
        }

        private void lvwCursos_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var tapped = e.Item as Model.Curso;

            if (!viewModel.ListaCursosDoEstagio.Contains(tapped))
                viewModel.ListaCursosDoEstagio.Add(tapped);

            lvwCursos.SelectedItem = null;
        }

        private void lvwCursosDoEstagio_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var tapped = e.Item as Model.Curso;
            viewModel.ListaCursosDoEstagio.Remove(tapped);
            lvwCursosDoEstagio.SelectedItem = null;
        }

        private void pkrStatus_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            var item = sender as Picker;
            var selectedItem = item.SelectedItem as StatusEstagio;
            if(selectedItem != null)
                viewModel.Estagio.Status = selectedItem.Codigo;
        }
    }
}