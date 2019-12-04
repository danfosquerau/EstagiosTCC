using EstagiosTCC.Dao;
using EstagiosTCC.Util;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;

namespace EstagiosTCC.ViewModel.Estagio
{
    public class BuscarEstagioViewModel : BaseViewModel
    {
        public ICommand TodosEstagiosCommand { get; set; }

        public ObservableCollection<Model.Curso> ListaCursos { get; set; }
        public ObservableCollection<Model.Estagio> ListaEstagios { get; set; }

        public BuscarEstagioViewModel(Picker picker)
        {
            Title = "Buscar Estágios";

            CarregarCursos(picker);
            CarregarEstagios();

            TodosEstagiosCommand = new Command(() => OnTodosEstagios());
        }

        private async void CarregarCursos(Picker picker)
        {
            ListaCursos = new ObservableCollection<Model.Curso>() { new Model.Curso() { Nome = "Selecione um curso..." } };
            var lista = await CursoDao.Buscar();

            foreach (Model.Curso item in lista)
                ListaCursos.Add(item);

            picker.ItemsSource = ListaCursos;
            if (!string.IsNullOrEmpty(App.EstudanteDados.Curso))
                picker.SelectedItem = ListaCursos.Where(x => x.Codigo == App.EstudanteDados.Curso).FirstOrDefault();
            else
                picker.SelectedIndex = 0;
        }

        private async void CarregarEstagios()
        {
            ListaEstagios = new ObservableCollection<Model.Estagio>();
            List<Model.Estagio> list;

            if(!string.IsNullOrEmpty(App.EstudanteDados.Curso))
                list = (await EstagioDao.Buscar()).Where(x => x.CodigosCursos.Contains(App.EstudanteDados.Curso)).ToList();
            else
                list = await EstagioDao.Buscar();

            foreach (Model.Estagio item in list)
                ListaEstagios.Add(item);
        }

        public async void AtualizarListaEstagios(string codigo)
        {
            ListaEstagios.Clear();

            var listaEstagios = (await EstagioDao.Buscar()).Where(x => x.CodigosCursos.Contains(codigo));

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