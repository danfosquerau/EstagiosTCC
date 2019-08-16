using EstagiosTCC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EstagiosTCC.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BuscarEstagio : ContentPage
    {
        public BuscarEstagio()
        {
            InitializeComponent();
            onStartEstagios();
            onStartCursos();
        }

        //Retorna todos os cursos
        private async Task<List<Estagio>> carregarEstagios()
        {
            Estagio e = new Estagio();
            return await e.listarEstagios();
        }
        //Retorna lista de cursos que possuem o id do curso selecionado
        private async Task<List<Estagio>> carregarEstagios(string idCursoSelecionado)
        {
            Estagio e = new Estagio();
            return await e.listarEstagios(idCursoSelecionado);
        }

        private async void onStartEstagios()
        {
            Estagio e = new Estagio();
            var responseEstagios = await e.listarEstagios();
            listaEstagios.ItemsSource = responseEstagios;
        }

        private async void onStartCursos()
        {
            Curso c = new Curso();
            var responseCursos = await c.listarCursos();
            rdbtnCursos.ItemsSource = responseCursos;
        }

        private async void estagioTapped(object sender, ItemTappedEventArgs e)
        {
            var tapped = e.Item as Estagio;
            await Navigation.PushAsync(new NovoEstagio(tapped));
        }

        private async void RdbtnCursos_SelectedIndexChanged(object sender, EventArgs e)
        {
            var item = sender as Picker;
            var selectedItem = item.SelectedItem as Curso;
            string idCursoSelecionado = selectedItem.idCurso;
            if (!idCursoSelecionado.Equals("0"))
            {
                List<Estagio> responseList = await carregarEstagios(idCursoSelecionado);
                listaEstagios.ItemsSource = responseList;
            }
            else
            {
                return;
            }
        }

        private async void btnTodosOsEstagios(object sender, System.EventArgs e)
        {
            List<Estagio> responseList = await carregarEstagios();
            listaEstagios.ItemsSource = responseList;
        }

        protected override void OnAppearing()
        {
            onStartEstagios();
            base.OnAppearing();
        }

    }
}