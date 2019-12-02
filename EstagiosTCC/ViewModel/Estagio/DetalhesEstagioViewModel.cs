using EstagiosTCC.Dao;
using EstagiosTCC.Model;
using EstagiosTCC.Util;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace EstagiosTCC.ViewModel.Estagio
{
    public class DetalhesEstagioViewModel : BaseViewModel
    {
        public ICommand OpenWebCommand { get; set; }
        public ICommand FavoritoCommand { get; set; }

        public ObservableCollection<Model.Curso> ListaCursosEstagio { get; set; }

        private Model.Estagio _estagio;
        public Model.Estagio Estagio
        {
            get { return _estagio; }
            set { SetProperty(ref _estagio, value); }
        }
        private string _anexoUrl = string.Empty;
        public string AnexoUrl
        {
            get { return _anexoUrl; }
            set { SetProperty(ref _anexoUrl, value); }
        }

        public DetalhesEstagioViewModel()
        {
            Title = "Detalhes do Estágio";
        }

        public DetalhesEstagioViewModel(Model.Estagio estagio)
        {
            Title = "Detalhes do Estágio";
            Estagio = estagio;
            CarregarRecursos();
            FavoritoCommand = new Command(() => OnFavorito());
            OpenWebCommand = new Command(() => Launcher.OpenAsync(
                new Uri(string.IsNullOrEmpty(Estagio.LinkParaInformacoes) ? "https://xamarin.com/platform" : Estagio.LinkParaInformacoes))); ;
        }

        private async void CarregarRecursos()
        {
            AnexoUrl = Estagio.AnexoUrl;
            ListaCursosEstagio = new ObservableCollection<Model.Curso>();
            var lista = await CursoDao.Buscar();
            if (!string.IsNullOrEmpty(Estagio.Codigo))
            {
                for (int i = 0; i < Estagio.CodigosCursos.Count; i++)
                {
                    foreach (Model.Curso c in lista)
                    {
                        if (c.Codigo == Estagio.CodigosCursos[i])
                        {
                            ListaCursosEstagio.Add(c);
                            break;
                        }
                    }
                }
            }
        }

        private void OnFavorito()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                if (App.EstudanteDados.Favoritos.Contains(Estagio.Codigo))
                {
                    App.EstudanteDados.Favoritos.Remove(Estagio.Codigo);
                    EstudanteDao.Favorito();
                    Application.Current.MainPage.DisplayAlert("Removido", "Estágio removido dos favoritos.", "Ok");
                }
                else
                {
                    App.EstudanteDados.Favoritos.Add(Estagio.Codigo);
                    EstudanteDao.Favorito();
                    Application.Current.MainPage.DisplayAlert("Adicionado", "Estágio adicionado aos favoritos.", "Ok");
                }
            }
            catch (Exception ex)
            {
                Application.Current.MainPage.DisplayAlert("Erro", ex.Message, "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}