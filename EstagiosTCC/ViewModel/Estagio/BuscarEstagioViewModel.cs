using EstagiosTCC.Dao;
using EstagiosTCC.Util;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Xamarin.Forms;

namespace EstagiosTCC.ViewModel.Estagio
{
    public class BuscarEstagioViewModel : BaseViewModel
    {
        public ICommand TodosEstagiosCommand { get; set; }

        public ObservableCollection<Model.Curso> ListaCursos { get; set; }
        public ObservableCollection<Model.Estagio> ListaEstagios { get; set; }

        public BuscarEstagioViewModel()
        {
            Title = "Buscar Estágios";

            CarregarCursos();
            CarregarEstagios();

            TodosEstagiosCommand = new Command(() => OnTodosEstagios());
        }

        private async void CarregarCursos()
        {
            ListaCursos = new ObservableCollection<Model.Curso>() { new Model.Curso() { Nome = "Selecione um curso..." } };
            var lista = await CursoDao.Buscar();

            foreach (Model.Curso item in lista)
                ListaCursos.Add(item);
        }

        private async void CarregarEstagios()
        {
            ListaEstagios = new ObservableCollection<Model.Estagio>();
            var listaEstagios = await EstagioDao.Buscar();

            foreach (Model.Estagio item in listaEstagios)
                ListaEstagios.Add(item);
        }

        public async void AtualizarListaEstagios(string codigo)
        {
            ListaEstagios.Clear();

            var listaEstagios = await EstagioDao.Buscar();

            foreach (Model.Estagio item in listaEstagios)
                ListaEstagios.Add(item);
        }


        private async void OnTodosEstagios()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                ListaEstagios.Clear();

                var listaEstagios = await EstagioDao.Buscar();

                foreach (Model.Estagio item in listaEstagios)
                    ListaEstagios.Add(item);

            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Erro", ex.Message, "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}