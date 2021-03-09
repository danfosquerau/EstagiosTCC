using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System.Threading.Tasks;

namespace EstagiosTCC.Util
{
    public static class Permissions
    {
        public static async Task<bool> GetPermission(Permission permission)
        {
            var status = await CrossPermissions.Current.CheckPermissionStatusAsync(permission);

            if (status != PermissionStatus.Granted)
            {
                var results = await CrossPermissions.Current
                    .RequestPermissionsAsync(new[] { permission });

                status = results[permission];
            }

            if (status == PermissionStatus.Granted)
                return true;
            else
                return false;
        }
    }
}