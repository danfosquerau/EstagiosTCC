using Plugin.Media;
using Plugin.Media.Abstractions;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System;
using System.Threading.Tasks;

namespace EstagiosTCC.Util
{
    public static class PickMedia
    {
        public static async Task<MediaFile> GetPhotoFromGallery()
        {
            bool response = await GetPermission();
            
            if (!response)
                throw new Exception("Permissão negada!");

            await CrossMedia.Current.Initialize();
            
            if (!CrossMedia.Current.IsPickPhotoSupported)
                throw new Exception("Ação não suportada!");

            var file = await CrossMedia.Current.PickPhotoAsync(new PickMediaOptions
            {
                PhotoSize = PhotoSize.Full,
                CompressionQuality = 50
            });

            if (file == null)
                return null;
            else
                return file as MediaFile;
        }

        public static async Task<MediaFile> GetPhotoFromCamera()
        {
            bool response = await GetPermission();
            
            if (!response)
                throw new Exception("Permissão negada!");

            await CrossMedia.Current.Initialize();
            
            if (!CrossMedia.Current.IsTakePhotoSupported || !CrossMedia.Current.IsCameraAvailable)
                throw new Exception("Ação não suportada!");

            var file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
            {
                PhotoSize = PhotoSize.Full,
                CompressionQuality = 50,
                SaveToAlbum = false
            });

            if (file == null)
                return null;
            else
                return file as MediaFile;
        }

        private static async Task<bool> GetPermission()
        {
            var cameraStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Camera);
            var storageStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Storage);
            
            if (cameraStatus != PermissionStatus.Granted || storageStatus != PermissionStatus.Granted)
            {
                var results = await CrossPermissions.Current
                    .RequestPermissionsAsync(new[] { Permission.Camera, Permission.Storage });
                cameraStatus = results[Permission.Camera];
                storageStatus = results[Permission.Storage];
            }

            if (cameraStatus == PermissionStatus.Granted && storageStatus == PermissionStatus.Granted)
                return true;
            else
                return false;
        }
    }
}