using EstagiosTCC.Util.OAuth;
using EstagiosTCC.UWP.OAuth;
using Xamarin.Forms;
using Xamarin.Forms.Platform.UWP;

[assembly: ExportRenderer(typeof(FacebookLoginPage), typeof(FacebookLoginRenderer))]
namespace EstagiosTCC.UWP.OAuth
{
    public class FacebookLoginRenderer : PageRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Page> e)
        {
            base.OnElementChanged(e);

            OAuthProviderSetting oauth = new OAuthProviderSetting();
            var auth = oauth.LoginWithProvider("Facebook");

            Windows.UI.Xaml.Controls.Frame rootFrame = Windows.UI.Xaml.Window.Current.Content as Windows.UI.Xaml.Controls.Frame;
            rootFrame.Navigate(auth.GetUI(), auth);
        }
    }
}