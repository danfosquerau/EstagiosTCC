using EstagiosTCC.Dao;
using EstagiosTCC.Model;
using EstagiosTCC.Util;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;

namespace EstagiosTCC.ViewModel
{
    public class MeusEstagiosViewModel : BaseViewModel
    {
        public ICommand TodosEstagiosCommand { get; set; }

        private List<Estagio> listaEstagios;

        public ObservableCollection<Curso> ListaCursos { get; set; }
        public ObservableCollection<Estagio> ListaEstagios { get; set; }

        public MeusEstagiosViewModel()
        {
            Title = "Meus Estágios";
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
            listaEstagios = new List<Estagio>();

            if (App.UsuarioLogadoDados.MeusEstagios.Count > 0)
            {
                for (int i = 0; i < App.UsuarioLogadoDados.MeusEstagios.Count; i++)
                {
                    var estagio = await EstagioDao.BuscarPeloCodigo(App.UsuarioLogadoDados.MeusEstagios[i]);
                    listaEstagios.Add(estagio);
                    ListaEstagios.Add(estagio);
                }  
            }
        }

        public void AtualizarListaEstagios(string codigo)
        {
            ListaEstagios.Clear();
            var lista = listaEstagios.Where(x => x.CodigosCursos.Contains(codigo));

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

                foreach (Estagio item in listaEstagios)
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