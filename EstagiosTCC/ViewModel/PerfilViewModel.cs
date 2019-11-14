using EstagiosTCC.Dao;
using EstagiosTCC.Model;
using EstagiosTCC.Util;
using EstagiosTCC.View;
using System;
using System.IO;
using System.Windows.Input;
using Xamarin.Forms;

namespace EstagiosTCC.ViewModel
{
    public class PerfilViewModel : BaseViewModel
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
        private Usuario _usuario;
        public Usuario Usuario
        {
            get { return _usuario; }
            set { SetProperty(ref _usuario, value); }
        }

        public PerfilViewModel(Page page)
        {
            Title = "Meu Perfil";
            this.page = page;
            imgPerfilStream = null;
            ImgPerfil = "img_profile.jpg";
            GetDadosUsuario();
            CameraCommand = new Command(() => OnCamera());
            GaleriaCommand = new Command(() => OnGaleria());
            RemoverFotoCommand = new Command(() => OnRemoverFoto());
            SalvarCommand = new Command(() => OnSalvar());
            CancelarCommand = new Command(() => OnCancelar());
        }

        private void GetDadosUsuario()
        {
            Usuario = App.UsuarioLogadoDados;

            if (!string.IsNullOrEmpty(Usuario.FotoPerfilUrl))
                ImgPerfil = Usuario.FotoPerfilUrl;
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
                ImgPerfil = "img_profile.jpg";
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
                if (!ValidationHelper.IsFormValid(Usuario, page))
                    return;

                if (Usuario.FotoPerfilUrl != ImgPerfil)
                    Usuario.FotoPerfilUrl = string.Empty;

                if (await UsuarioDao.Alterar(Usuario, imgPerfilStream))
                    (Application.Current.MainPage as MenuPrincipalPage).AtualizarDadosUsuario();
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

                if (string.IsNullOrEmpty(Usuario.FotoPerfilUrl))
                {
                    ImgPerfil = "img_profile.jpg";
                    imgPerfilStream = null;
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
    }
}