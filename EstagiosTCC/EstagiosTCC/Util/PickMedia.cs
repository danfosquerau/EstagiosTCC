using Plugin.Media;
using Plugin.Media.Abstractions;
using Plugin.Permissions.Abstractions;
using System;
using System.Threading.Tasks;

namespace EstagiosTCC.Util
{
    public static class PickMedia
    {
        public static async Task<MediaFile> GetPhotoFromGallery()
        {
            if (!await GetPermissions())
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
            if (!await GetPermissions())
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
    
        private static async Task<bool> GetPermissions()
        {
            var camera = await Permissions.GetPermission(Permission.Camera);
            var storage = await Permissions.GetPermission(Permission.Storage);
            
            if (camera == true && storage == true)
                return true;
            else
                return false;
        }
    }
}