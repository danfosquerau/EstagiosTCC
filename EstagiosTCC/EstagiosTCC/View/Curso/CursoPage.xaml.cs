using EstagiosTCC.ViewModel.Curso;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EstagiosTCC.View.Curso
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CursoPage : ContentPage
    {
        private readonly CursoViewModel viewModel;

        public CursoPage()
        {
            InitializeComponent();

            BindingContext = viewModel = new CursoViewModel();

            txtNome.Completed += (object sender, EventArgs e) =>
            {
                viewModel.SalvarCommand.Execute(null);
            };
        }
    }
}