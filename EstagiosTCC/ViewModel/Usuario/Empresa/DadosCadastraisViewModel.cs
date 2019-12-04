using EstagiosTCC.Dao;
using EstagiosTCC.Util;
using EstagiosTCC.View.Usuario.Empresa;
using System;
using System.IO;
using System.Windows.Input;
using Xamarin.Forms;

namespace EstagiosTCC.ViewModel.Usuario.Empresa
{
    class DadosCadastraisViewModel : BaseViewModel
    {
        public ICommand CameraCommand { get; set; }
        public ICommand GaleriaCommand { get; set; }
        public ICommand RemoverFotoCommand { get; set; }
        public ICommand SalvarCommand { get; set; }
        public ICommand CancelarCommand { get; set; }
        public ICommand BuscarEnderecoCommand { get; set; }
        public ICommand BuscarLocalizacaoCommand { get; set; }

        private readonly Page page;

        private Stream imgLogoEmpresaStream { get; set; }

        private string _imgLogoEmpresa = string.Empty;
        public string ImgLogoEmpresa
        {
            get { return _imgLogoEmpresa; }
            set { SetProperty(ref _imgLogoEmpresa, value); }
        }

        private Model.Empresa _empresa;
        public Model.Empresa Empresa
        {
            get { return _empresa; }
            set { SetProperty(ref _empresa, value); }
        }

        private Model.Endereco _endereco;
        public Model.Endereco Endereco
        {
            get { return _endereco; }
            set { SetProperty(ref _endereco, value); }
        }

        public DadosCadastraisViewModel(Page page)
        {
            Title = "Dados Cadastrais";
            this.page = page;
            imgLogoEmpresaStream = null;
            GetDadosUsuario();
            CameraCommand = new Command(() => OnCamera());
            GaleriaCommand = new Command(() => OnGaleria());
            RemoverFotoCommand = new Command(() => OnRemoverFoto());
            SalvarCommand = new Command(() => OnSalvar());
            CancelarCommand = new Command(() => OnCancelar());
            BuscarEnderecoCommand = new Command(() => OnBuscarEndereco());
            BuscarLocalizacaoCommand = new Command(() => OnBuscarLocalizacao());
        }

        private void GetDadosUsuario()
        {
            Empresa = App.EmpresaDados;
            Endereco = Empresa.Endereco;

            ImgLogoEmpresa = Device.RuntimePlatform.Equals(Device.UWP) ? "Resources/sem_imagem.png" : "sem_imagem.png";

            if (!string.IsNullOrEmpty(Empresa.LogoEmpresa))
                ImgLogoEmpresa = Empresa.LogoEmpresa;
        }

        private async void OnCamera()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                var arquivo = await PickMedia.GetPhotoFromCamera();

                if (arquivo != null)
                {
                    ImgLogoEmpresa = arquivo.Path;
                    imgLogoEmpresaStream = arquivo.GetStream();
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

        private async void OnGaleria()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                var arquivo = await PickMedia.GetPhotoFromGallery();

                if (arquivo != null)
                {
                    ImgLogoEmpresa = arquivo.Path;
                    imgLogoEmpresaStream = arquivo.GetStream();
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

        private void OnRemoverFoto()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                ImgLogoEmpresa = Device.RuntimePlatform.Equals(Device.UWP) ? "Resources/sem_imagem.png" : "sem_imagem.png";
                imgLogoEmpresaStream = null;
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

        private async void OnSalvar()
        {
            if (IsBusy)
                return;

            IsBusy = true;
            try
            {
                Empresa.Endereco = Endereco;

                if (!ValidationHelper.IsFormValid(Empresa, page, true))
                    return;

                if (!ValidationHelper.IsFormValid(Endereco, page, false))
                    return;

                if (Empresa.LogoEmpresa != ImgLogoEmpresa)
                    Empresa.LogoEmpresa = string.Empty;

                if (await EmpresaDao.Alterar(Empresa, imgLogoEmpresaStream))
                    (Application.Current.MainPage as MenuEmpresaPage).AtualizarDadosUsuario();
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

        private void OnCancelar()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                GetDadosUsuario();
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
                var e = Location.GetCEPPosition(Endereco.Cep);

                if (e != null)
                    Endereco = e;
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
                var e = await Location.GetGPSPosition();
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
    }
}