using EstagiosTCC.Dao;
using EstagiosTCC.Util;
using System.Collections.ObjectModel;

namespace EstagiosTCC.ViewModel.Estagio
{
    public class FavoritosViewModel : BaseViewModel
    {
        public ObservableCollection<Model.Estagio> ListaEstagios { get; set; }

        public FavoritosViewModel()
        {
            Title = "Favoritos";
            ListaEstagios = new ObservableCollection<Model.Estagio>();
            CarregarEstagios();
        }

        public async void CarregarEstagios()
        {
            ListaEstagios.Clear();

            if (App.EstudanteDados.Favoritos.Count > 0)
            {
                for (int i = 0; i < App.EstudanteDados.Favoritos.Count; i++)
                {
                    var estagio = await EstagioDao.BuscarPeloCodigo(App.EstudanteDados.Favoritos[i]);
                    ListaEstagios.Add(estagio);
                }
            }
        }
    }
}