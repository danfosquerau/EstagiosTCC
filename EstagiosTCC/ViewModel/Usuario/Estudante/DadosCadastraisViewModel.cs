using EstagiosTCC.Dao;
using EstagiosTCC.Util;
using EstagiosTCC.View.Usuario.Estudante;
using System;
using System.IO;
using System.Windows.Input;
using Xamarin.Forms;

namespace EstagiosTCC.ViewModel.Usuario.Estudante
{
    public class DadosCadastraisViewModel : BaseViewModel
    {
        public ICommand CameraCommand { get; set; }
        public ICommand GaleriaCommand { get; set; }
        public ICommand RemoverFotoCommand { get; set; }
        public ICommand SalvarCommand { get; set; }
        public ICommand CancelarCommand { get; set; }

        private readonly Page page;

        private Stream imgPerfilStream { get; set; }

        private string _imgPerfil = string.Empty;
        public string ImgPerfil
        {
            get { return _imgPerfil; }
            set { SetProperty(ref _imgPerfil, value); }
        }
        private Model.Estudante _estudante;
        public Model.Estudante Estudante
        {
            get { return _estudante; }
            set { SetProperty(ref _estudante, value); }
        }

        public DadosCadastraisViewModel(Page page)
        {
            Title = "Dados Cadastrais";
            this.page = page;
            imgPerfilStream = null;
            GetDadosUsuario();
            CameraCommand = new Command(() => OnCamera());
            GaleriaCommand = new Command(() => OnGaleria());
            RemoverFotoCommand = new Command(() => OnRemoverFoto());
            SalvarCommand = new Command(() => OnSalvar());
            CancelarCommand = new Command(() => OnCancelar());
        }

        private void GetDadosUsuario()
        {
            Estudante = App.EstudanteDados;

            ImgPerfil = Device.RuntimePlatform.Equals(Device.UWP)? "Resources/sem_imagem.png" : "sem_imagem.png";

            if (!string.IsNullOrEmpty(Estudante.FotoPerfilUrl))
                ImgPerfil = Estudante.FotoPerfilUrl;
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
                    ImgPerfil = arquivo.Path;
                    imgPerfilStream = arquivo.GetStream();
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
                    ImgPerfil = arquivo.Path;
                    imgPerfilStream = arquivo.GetStream();
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
                ImgPerfil = Device.RuntimePlatform.Equals(Device.UWP) ? "Resources/sem_imagem.png" : "sem_imagem.png";
                imgPerfilStream = null;
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
                if (!ValidationHelper.IsFormValid(Estudante, page))
                    return;

                if (Estudante.FotoPerfilUrl != ImgPerfil)
                    Estudante.FotoPerfilUrl = string.Empty;

                if (await EstudanteDao.Alterar(Estudante, imgPerfilStream))
                    (Application.Current.MainPage as MenuEstudantePage).AtualizarDadosUsuario();
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
    }
}