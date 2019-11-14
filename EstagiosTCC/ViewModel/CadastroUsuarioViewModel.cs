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
    public class CadastroUsuarioViewModel : BaseViewModel
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
        private bool _lembrar = false;
        public bool Lembrar
        {
            get { return _lembrar; }
            set { SetProperty(ref _lembrar, value); }
        }
        private Usuario _usuario;
        public Usuario Usuario
        {
            get { return _usuario; }
            set { SetProperty(ref _usuario, value); }
        }

        public CadastroUsuarioViewModel(Page page)
        {
            Title = "Registro";
            ImgPerfil = "img_profile.jpg";
            imgPerfilStream = null;
            Usuario = new Usuario();
            CameraCommand = new Command(() => OnCamera());
            GaleriaCommand = new Command(() => OnGaleria());
            RemoverFotoCommand = new Command(() => OnRemoverFoto());
            SalvarCommand = new Command(() => OnSalvar());
            CancelarCommand = new Command(() => OnCancelar());
            this.page = page;
        }
        
        private async void OnCamera()
        {
            if (IsBusy)
                return;

            IsBusy = true;
            
            try
            {
                var arquivo = await PickMedia.GetPhotoFromCamera();
                
                if(arquivo != null)
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

                if (await UsuarioDao.Inserir(Usuario, imgPerfilStream))
                {
                    if (Lembrar)
                        Properties.SaveLogin(Usuario.Email, Usuario.Senha);

                    Application.Current.MainPage = new MenuPrincipalPage();
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

        private void OnCancelar()
        {
            if (IsBusy)
                return;

            IsBusy = true;
           
            try
            {
                Application.Current.MainPage.Navigation.PopAsync();
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