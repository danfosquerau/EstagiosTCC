using EstagiosTCC.Dao;
using EstagiosTCC.Util;
using EstagiosTCC.Util.Validation;
using EstagiosTCC.View.Usuario.Empresa;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Input;
using Xamarin.Essentials;
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
        public ICommand BuscarEnderecoCommand { get; set; }
        public ICommand BuscarLocalizacaoCommand { get; set; }
        public ICommand UrlTesteCommand { get; set; }

        private Stream anexoStream { get; set; }

        public ObservableCollection<Model.Curso> ListaCursos { get; set; }
        public ObservableCollection<Model.Curso> ListaCursosDoEstagio { get; set; }
        public ObservableCollection<Model.StatusEstagio> ListaStatus { get; set; }

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

        private Model.Endereco _endereco;
        public Model.Endereco Endereco
        {
            get { return _endereco; }
            set { SetProperty(ref _endereco, value); }
        }

        private string _cepError = string.Empty;
        public string CepError
        {
            get { return _cepError; }
            set { SetProperty(ref _cepError, value); }
        }

        private bool _cepErrorVisible = false;
        public bool CepErrorVisible
        {
            get { return _cepErrorVisible; }
            set { SetProperty(ref _cepErrorVisible, value); }
        }

        public CadastrarEstagioViewModel(Page page)
        {
            Title = "Novo Estágio";
            this.page = page;

            Estagio = new Model.Estagio()
            {
                Empresa = App.EmpresaDados.NomeEmpresa,
                LogoEmpresa = App.EmpresaDados.LogoEmpresa,
                Endereco = App.EmpresaDados.Endereco
            };
            Endereco = Estagio.Endereco;

            CarregarRecursos();
            AcoesDosBotoes();
        }

        public CadastrarEstagioViewModel(Page page, Model.Estagio estagio)
        {
            Title = "Editar Estágio";
            this.page = page;

            Estagio = estagio;
            Estagio.Empresa = App.EmpresaDados.NomeEmpresa;
            Estagio.LogoEmpresa = App.EmpresaDados.LogoEmpresa;
            Endereco = Estagio.Endereco;

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

            ListaStatus = new ObservableCollection<Model.StatusEstagio>()
            {
                 { new Model.StatusEstagio() { Id = Model.Status.Disponivel, Nome = "Disponível" } },
                 { new Model.StatusEstagio() { Id = Model.Status.Ocupado, Nome = "Ocupado" } },
                 { new Model.StatusEstagio() { Id = Model.Status.Desativado, Nome = "Desativado" } }
            };
        }

        private void AcoesDosBotoes()
        {
            SalvarCommand = new Command(() => OnSalvar());
            CancelarCommand = new Command(() => OnCancelar());
            EscolherAnexoCommand = new Command(() => OnEscolherAnexo());
            RemoverAnexoCommand = new Command(() => OnRemoverAnexo());
            TirarFotoCommand = new Command(() => OnTirarFoto());
            LimparCommand = new Command(() => OnLimpar());
            BuscarEnderecoCommand = new Command(() => OnBuscarEndereco());
            BuscarLocalizacaoCommand = new Command(() => OnBuscarLocalizacao());
            UrlTesteCommand = new Command(() => OnUrlTeste());
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
                Estagio.Endereco = Endereco;

                if (!ValidationHelper.IsFormValid(Estagio, page, true))
                    return;

                if (!ValidationHelper.IsFormValid(Endereco, page, false))
                    return;

                if (ListaCursosDoEstagio.Count > 0)
                {
                    for (var i = 0; i < ListaCursosDoEstagio.Count; i++)
                        Estagio.CodigosCursos.Add(ListaCursosDoEstagio[i].Codigo);
                }
                else
                    return;

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

        private void OnBuscarEndereco()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                Endereco.Cep = RemoveCarecteres.ObterStringSemAcentosECaracteresEspeciais(Endereco.Cep);
                CepError = string.Empty;
                CepErrorVisible = false;
                if (MyValidation.ValidarCep(Endereco.Cep))
                {
                    var e = Util.Location.GetCEPPosition(Endereco.Cep);

                    if (e != null)
                        Endereco = e;
                }
                else
                {
                    CepError = "CEP inválido.";
                    CepErrorVisible = true;
                }
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

        private async void OnBuscarLocalizacao()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                var e = await Util.Location.GetGPSPosition();
                if (e != null)
                    Endereco = e;
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
    
        private async void OnUrlTeste()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                bool response = await Launcher.TryOpenAsync(new Uri(Estagio.LinkParaInformacoes));

                if(!response)
                    await Application.Current.MainPage.DisplayAlert("Erro", "Não foi possivel abrir o link", "Ok");
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
    }
}