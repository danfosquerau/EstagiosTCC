using EstagiosTCC.Dao;
using EstagiosTCC.Model;
using EstagiosTCC.Util;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Xamarin.Forms;

namespace EstagiosTCC.ViewModel
{
    public class BuscarEstagioViewModel :BaseViewModel
    {
        public ICommand TodosEstagiosCommand { get; set; }

        public ObservableCollection<Curso> ListaCursos { get; set; }
        public ObservableCollection<Estagio> ListaEstagios { get; set; }

        public BuscarEstagioViewModel()
        {
            Title = "Pesquisar Estágios";
            CarregarCursos();
            CarregarEstagios();
            TodosEstagiosCommand = new Command(() => OnTodosEstagios());
        }

        private async void CarregarCursos()
        {
            ListaCursos = new ObservableCollection<Curso>() { new Curso() { Nome = "Selecione um curso..." } };
            var lista = await CursoDao.Buscar();
            
            foreach (Curso item in lista)
                ListaCursos.Add(item);
        }

        private async void CarregarEstagios()
        {
            ListaEstagios = new ObservableCollection<Estagio>();
            var lista = await EstagioDao.Buscar();
            
            foreach (Estagio item in lista)
                ListaEstagios.Add(item);
        }

        public async void AtualizarListaEstagios(string codigo)
        {
            ListaEstagios.Clear();
            var lista = await EstagioDao.BuscarPeloCurso(codigo);
            
            foreach (Estagio item in lista)
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
                var lista = await EstagioDao.Buscar();
                
                foreach (Estagio item in lista)
                    ListaEstagios.Add(item);
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Erro",ex.Message,"OK");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
