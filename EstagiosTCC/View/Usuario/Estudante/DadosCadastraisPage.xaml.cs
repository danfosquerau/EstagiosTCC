using EstagiosTCC.ViewModel.Usuario.Estudante;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EstagiosTCC.View.Usuario.Estudante
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DadosCadastraisPage : ContentPage
    {
        private readonly DadosCadastraisViewModel viewModel;

        public DadosCadastraisPage()
        {
            InitializeComponent();

            BindingContext = viewModel = new DadosCadastraisViewModel(this, pkrCursos);
        }

        private void pkrCursos_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            var item = sender as Picker;
                var selectedItem = item.SelectedItem as Model.Curso;
                string codigo = selectedItem.Codigo;

                if (!string.IsNullOrEmpty(codigo))
                    viewModel.Estudante.Curso = codigo;
                else
                    viewModel.Estudante.Curso = string.Empty;
        }
    }
}