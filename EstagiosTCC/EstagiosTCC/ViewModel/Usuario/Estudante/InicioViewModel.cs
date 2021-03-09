using EstagiosTCC.Dao;
using EstagiosTCC.Util;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace EstagiosTCC.ViewModel.Usuario.Estudante
{
    public class InicioViewModel : BaseViewModel
    {
        public ObservableCollection<Model.Estagio> ListaEstagios { get; set; }

        public InicioViewModel()
        {
            Title = "Início";
            ListaEstagios = new ObservableCollection<Model.Estagio>();
            CarregarEstagios();
        }

        public async void CarregarEstagios()
        {
            ListaEstagios.Clear();
            List<Model.Estagio> list;

            if (!string.IsNullOrEmpty(App.EstudanteDados.Curso))
                list = (await EstagioDao.Buscar()).Where(x => x.CodigosCursos.Contains(App.EstudanteDados.Curso)).ToList();
            else
                list = await EstagioDao.Buscar();

            foreach (Model.Estagio item in list)
                ListaEstagios.Add(item);
        }
    }
}