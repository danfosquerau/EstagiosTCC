using EstagiosTCC.iOS.OAuth;
using EstagiosTCC.Util.OAuth;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(FacebookLoginPage), typeof(FacebookLoginRenderer))]
namespace EstagiosTCC.iOS.OAuth
{
    public class FacebookLoginRenderer : PageRenderer
    {
        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {
            base.OnElementChanged(e);

            OAuthProviderSetting oauth = new OAuthProviderSetting();
            var auth = oauth.LoginWithProvider("Facebook");

            PresentViewController(auth.GetUI(), true, null);
        }
    }
}