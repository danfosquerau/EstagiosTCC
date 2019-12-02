using EstagiosTCC.Dao;
using EstagiosTCC.Util;
using System.Collections.ObjectModel;

namespace EstagiosTCC.ViewModel.Estagio
{
    public class MeusEstagiosViewModel : BaseViewModel
    {
        public ObservableCollection<Model.Estagio> ListaEstagios { get; set; }

        public MeusEstagiosViewModel()
        {
            Title = "Meus Estágios";
            ListaEstagios = new ObservableCollection<Model.Estagio>();
            CarregarEstagios();
        }

        public async void CarregarEstagios()
        {
            ListaEstagios.Clear();

            if (App.EmpresaDados.MeusEstagios.Count > 0)
            {
                for (int i = 0; i < App.EmpresaDados.MeusEstagios.Count; i++)
                {
                    var estagio = await EstagioDao.BuscarPeloCodigo(App.EmpresaDados.MeusEstagios[i]);
                    ListaEstagios.Add(estagio);
                }
            }
        }
    }
}