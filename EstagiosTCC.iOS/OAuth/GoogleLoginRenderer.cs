using EstagiosTCC.iOS.OAuth;
using EstagiosTCC.Util.OAuth;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(GoogleLoginPage), typeof(GoogleLoginRenderer))]
namespace EstagiosTCC.iOS.OAuth
{
    public class GoogleLoginRenderer : PageRenderer
    {
        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {
            base.OnElementChanged(e);

            OAuthProviderSetting oauth = new OAuthProviderSetting();
            var auth = oauth.LoginWithProvider("Google");

            PresentViewController(auth.GetUI(), true, null);
        }
    }
}