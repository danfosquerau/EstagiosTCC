using EstagiosTCC.Models;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace EstagiosTCC.ViewModels
{
    public class EditInternshipViewModel : BaseViewModel
    {
        public ICommand DeleteCommand { get; set; }
        public ICommand SaveCommand { get; set; }
        public ICommand CancelCommand { get; set; }
        public ICommand ChooseImageCommand { get; set; }
        public ICommand RemoveImageCommand { get; set; }
        public ICommand TakePictureCommand { get; set; }

        private Stream streamAttachment { get; set; }
        
        public ObservableCollection<Course> ListCourses { get; set; }
        public ObservableCollection<Course> ListCoursesOfInternship { get; set; }

        private string _urlAtachment = string.Empty;
        public string UrlAttachment
        {
            get { return _urlAtachment; }
            set { SetProperty(ref _urlAtachment, value); }
        }
        private Internship _selectedInternship;
        public Internship SelectedInternship
        {
            get { return _selectedInternship; }
            set { SetProperty(ref _selectedInternship, value); }
        }

        public EditInternshipViewModel()
        {
            Title = "Novo Estágio";
            SelectedInternship = new Internship();
            LoadResoures();
            CommandActions();
        }

        public EditInternshipViewModel(Internship internship)
        {
            Title = "Editar Estágio";
            SelectedInternship = internship;
            LoadResoures();
            CommandActions();
        }

        private async void LoadResoures()
        {
            streamAttachment = null;
            UrlAttachment = SelectedInternship.UrlAttachment;
            ListCourses = new ObservableCollection<Course>();
            ListCoursesOfInternship = new ObservableCollection<Course>();
            var response = await new Course().Select();
            foreach (Course item in response)
                ListCourses.Add(item);

            if (!string.IsNullOrEmpty(SelectedInternship.Uid))
            {
                for (int i = 0; i < SelectedInternship.UidCourses.Length; i++)
                {
                    foreach (Course c in ListCourses.ToList())
                    {
                        if (c.Uid == SelectedInternship.UidCourses[i])
                        {
                            ListCoursesOfInternship.Add(c);
                            break;
                        }
                    }
                }
            }
            OnPropertyChanged();
        }

        private void CommandActions()
        {
            DeleteCommand = new Command(() => OnDelete());
            SaveCommand = new Command(() => OnSave());
            CancelCommand = new Command(() => OnCancel());
            ChooseImageCommand = new Command(() => OnChooseImage());
            RemoveImageCommand = new Command(() => OnRemoveImage());
            TakePictureCommand = new Command(() => OnTakePicture());
        }

        public async void OnDelete()
        {
            if (IsBusy)
                return;

            IsBusy = true;
            try
            {
                if (!string.IsNullOrEmpty(SelectedInternship.Uid))
                {
                    bool response = await SelectedInternship.Delete();
                    if (response)
                        DisplaySuccessPrompt();
                    else
                        DisplayFailedPrompt();
                }
                else
                    DisplayDeniedPrompt();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
                OnCancel();
            }
        }

        public void OnCancel()
        {
            MessagingCenter.Send<object>(this, App.EVENT_RETURN_HOME_PAGE);
        }

        public void OnRemoveImage()
        {
            if (IsBusy)
                return;

            IsBusy = true;
            try
            {
                streamAttachment = null;
                SelectedInternship.UrlAttachment = UrlAttachment = string.Empty;
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

        public async void OnSave()
        {
            if (IsBusy)
                return;

            IsBusy = true;
            try
            {
                if (ListCoursesOfInternship.Count > 0)
                {
                    SelectedInternship.UidCourses = new string[ListCoursesOfInternship.Count];
                    for (var i = 0; i < ListCoursesOfInternship.Count; i++)
                    {
                        SelectedInternship.UidCourses[i] = ListCoursesOfInternship[i].Uid;
                    }
                }
                bool response;
                if (string.IsNullOrEmpty(SelectedInternship.Uid))
                    response = await SelectedInternship.Insert(streamAttachment);
                else
                    response = await SelectedInternship.Updade(streamAttachment);

                if (response)
                    DisplaySuccessPrompt();
                else
                    DisplayFailedPrompt();
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

        public async void OnChooseImage()
        {
            if (IsBusy)
                return;

            IsBusy = true;
            try
            {
                bool response = await CheckPermission();
                if (response)
                    ChooseGalleryImage();
                else
                    DisplayDeniedPrompt();
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

        private async Task<bool> CheckPermission()
        {
            var cameraStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Camera);
            var storageStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Storage);

            if (cameraStatus != PermissionStatus.Granted || storageStatus != PermissionStatus.Granted)
            {
                var results = await CrossPermissions.Current
                    .RequestPermissionsAsync(new[] { Permission.Camera, Permission.Storage});
                cameraStatus = results[Permission.Camera];
                storageStatus = results[Permission.Storage];
            }

            if (cameraStatus == PermissionStatus.Granted && storageStatus == PermissionStatus.Granted)
                return true;
            else
                return false;
        }

        private async void ChooseGalleryImage()
        {
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsPickPhotoSupported)
            {
                DisplayFailedPrompt();

                return;
            }

            var file = await CrossMedia.Current.PickPhotoAsync(new PickMediaOptions
            {
                PhotoSize = PhotoSize.Full,
                CompressionQuality = 40
            });

            if (file == null)
                return;

            streamAttachment = file.GetStream();
            UrlAttachment = file.Path;
        }

        private async void OnTakePicture()
        {
            if (IsBusy)
                return;

            IsBusy = true;
            try
            {
                bool response = await CheckPermission();
                if (response)
                    TakePicture();
                else
                    DisplayDeniedPrompt();
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

        private async void TakePicture()
        {
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsTakePhotoSupported || !CrossMedia.Current.IsCameraAvailable)
            {
                DisplayDeniedPrompt();

                return;
            }

            var file = await CrossMedia.Current.TakePhotoAsync(
                new StoreCameraMediaOptions
                {
                    PhotoSize = PhotoSize.Full,
                    CompressionQuality = 40,
                    SaveToAlbum = true,
                    Directory = "Picture"
                });

            if (file == null)
                return;

            streamAttachment = file.GetStream();
            UrlAttachment = file.Path;
        }
    }
}