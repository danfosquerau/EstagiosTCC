using EstagiosTCC.Dao;
using EstagiosTCC.Util;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Input;
using Xamarin.Forms;

namespace EstagiosTCC.ViewModel.Curso
{
    public class CursoViewModel : BaseViewModel
    {
        public ICommand SalvarCommand { get; set; }
        public ICommand LimparCommand { get; set; }

        public ObservableCollection<Model.Curso> ListaCursos { get; set; }

        private Model.Curso _curso;
        public Model.Curso Curso
        {
            get { return _curso; }
            set { SetProperty(ref _curso, value); }
        }

        public CursoViewModel()
        {
            Title = "Cursos";
            CarregarCursos();
            SalvarCommand = new Command(() => OnSalvar());
            LimparCommand = new Command(() => OnLimpar());
        }

        private async void CarregarCursos()
        {
            ListaCursos = new ObservableCollection<Model.Curso>();
            Curso = new Model.Curso();
            var response = await CursoDao.Buscar();
            foreach (Model.Curso item in response)
                ListaCursos.Add(item);
        }

        private async System.Threading.Tasks.Task AtualizarTela()
        {
            ListaCursos.Clear();
            Curso = new Model.Curso();
            var response = await CursoDao.Buscar();
            foreach (Model.Curso item in response)
                ListaCursos.Add(item);
        }

        private async void OnSalvar()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                if (string.IsNullOrEmpty(Curso.Nome))
                    await Application.Current.MainPage.DisplayAlert("Erro", "Preencha o campo", "Ok");
                else
                {
                    bool resultado = false;

                    if (string.IsNullOrEmpty(Curso.Codigo))
                        resultado = await CursoDao.Inserir(Curso);
                    else
                        resultado = await CursoDao.Alterar(Curso);

                    if (resultado)
                        await Application.Current.MainPage.DisplayAlert("Sucesso", "Salvo", "Ok");
                    else
                        await Application.Current.MainPage.DisplayAlert("Erro", "Algo deu errado", "Ok");
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Erro", ex.Message, "Ok");
            }
            finally
            {
                await AtualizarTela();
                IsBusy = false;
            }
        }

        public async void OnLimpar()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                await AtualizarTela();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
