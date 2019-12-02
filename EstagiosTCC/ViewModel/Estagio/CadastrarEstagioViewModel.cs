using EstagiosTCC.Dao;
using EstagiosTCC.Util;
using EstagiosTCC.View.Usuario.Empresa;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;

namespace EstagiosTCC.ViewModel.Estagio
{
    public class CadastrarEstagioViewModel : BaseViewModel
    {
        private readonly Page page;

        public ICommand SalvarCommand { get; set; }
        public ICommand CancelarCommand { get; set; }
        public ICommand EscolherAnexoCommand { get; set; }
        public ICommand RemoverAnexoCommand { get; set; }
        public ICommand TirarFotoCommand { get; set; }
        public ICommand LimparCommand { get; set; }

        private Stream anexoStream { get; set; }

        public ObservableCollection<Model.Curso> ListaCursos { get; set; }
        public ObservableCollection<Model.Curso> ListaCursosDoEstagio { get; set; }

        private string _anexoUrl = string.Empty;
        public string AnexoUrl
        {
            get { return _anexoUrl; }
            set { SetProperty(ref _anexoUrl, value); }
        }
        private Model.Estagio _estagio;
        public Model.Estagio Estagio
        {
            get { return _estagio; }
            set { SetProperty(ref _estagio, value); }
        }

        public CadastrarEstagioViewModel(Page page)
        {
            Title = "Novo Estágio";
            this.page = page;

            Estagio = new Model.Estagio()
            {
                Empresa = App.EmpresaDados.NomeEmpresa,
                LogoEmpresa = App.EmpresaDados.LogoEmpresa
            };

            CarregarRecursos();
            AcoesDosBotoes();
        }

        public CadastrarEstagioViewModel(Page page, Model.Estagio estagio)
        {
            Title = "Editar Estágio";
            this.page = page;

            Estagio = estagio;
            estagio.Empresa = App.EmpresaDados.NomeEmpresa;
            estagio.LogoEmpresa = App.EmpresaDados.LogoEmpresa;

            CarregarRecursos();
            AcoesDosBotoes();
        }

        private async void CarregarRecursos()
        {
            anexoStream = null;
            AnexoUrl = Estagio.AnexoUrl;
            ListaCursos = new ObservableCollection<Model.Curso>();
            ListaCursosDoEstagio = new ObservableCollection<Model.Curso>();
            var lista = await CursoDao.Buscar();

            foreach (Model.Curso item in lista)
                ListaCursos.Add(item);

            if (!string.IsNullOrEmpty(Estagio.Codigo))
            {
                for (int i = 0; i < Estagio.CodigosCursos.Count; i++)
                {
                    foreach (Model.Curso c in ListaCursos.ToList())
                    {
                        if (c.Codigo == Estagio.CodigosCursos[i])
                        {
                            ListaCursosDoEstagio.Add(c);
                            break;
                        }
                    }
                }
            }
        }

        private void AcoesDosBotoes()
        {
            SalvarCommand = new Command(() => OnSalvar());
            CancelarCommand = new Command(() => OnCancelar());
            EscolherAnexoCommand = new Command(() => OnEscolherAnexo());
            RemoverAnexoCommand = new Command(() => OnRemoverAnexo());
            TirarFotoCommand = new Command(() => OnTirarFoto());
            LimparCommand = new Command(() => OnLimpar());
        }

        public void OnCancelar()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                (Application.Current.MainPage as MenuEmpresaPage).IrParaTela(MenuItemTipo.Inicio);
            }
            catch (Exception ex)
            {
                Application.Current.MainPage.DisplayAlert("Erro", ex.Message, "Ok");
            }
            finally
            {
                IsBusy = false;
            }
        }

        public void OnRemoverAnexo()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                anexoStream = null;
                Estagio.AnexoUrl = AnexoUrl = string.Empty;
            }
            catch (Exception ex)
            {
                Application.Current.MainPage.DisplayAlert("Erro", ex.Message, "Ok");
            }
            finally
            {
                IsBusy = false;
            }
        }

        public async void OnSalvar()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                if (!ValidationHelper.IsFormValid(Estagio, page))
                    return;

                if (ListaCursosDoEstagio.Count > 0)
                {
                    for (var i = 0; i < ListaCursosDoEstagio.Count; i++)
                        Estagio.CodigosCursos.Add(ListaCursosDoEstagio[i].Codigo);
                }

                bool response;

                if (string.IsNullOrEmpty(Estagio.Codigo))
                    response = await EstagioDao.Inserir(Estagio, anexoStream);
                else
                    response = await EstagioDao.Alterar(Estagio, anexoStream);

                if (response)
                {
                    await Application.Current.MainPage.DisplayAlert("Sucesso", "Estágio salvado.", "Ok");
                    Title = "Editar Estágio";
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Erro", ex.Message, "Ok");
            }
            finally
            {
                IsBusy = false;
            }
        }

        public async void OnEscolherAnexo()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                var arquivo = await PickMedia.GetPhotoFromGallery();

                if (arquivo != null)
                {
                    AnexoUrl = arquivo.Path;
                    anexoStream = arquivo.GetStream();
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Erro", ex.Message, "Ok");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async void OnTirarFoto()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                var arquivo = await PickMedia.GetPhotoFromCamera();

                if (arquivo != null)
                {
                    AnexoUrl = arquivo.Path;
                    anexoStream = arquivo.GetStream();
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Erro", ex.Message, "Ok");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private void OnLimpar()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                Title = "Novo Estágio";
                Estagio = new Model.Estagio();
                CarregarRecursos();
            }
            catch (Exception ex)
            {
                Application.Current.MainPage.DisplayAlert("Erro", ex.Message, "Ok");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}