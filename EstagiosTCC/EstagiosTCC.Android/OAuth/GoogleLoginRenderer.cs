using Android.App;
using Android.Content;
using EstagiosTCC.Droid.OAuth;
using EstagiosTCC.Util.OAuth;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(GoogleLoginPage), typeof(GoogleLoginRenderer))]
namespace EstagiosTCC.Droid.OAuth
{
    public class GoogleLoginRenderer : PageRenderer
    {
        public GoogleLoginRenderer(Context context) : base(context) { }

        protected override void OnElementChanged(ElementChangedEventArgs<Page> e)
        {
            base.OnElementChanged(e);

            OAuthProviderSetting oauth = new OAuthProviderSetting();
            var auth = oauth.LoginWithProvider("Google");

            var activity = Context as Activity;
            activity.StartActivity(auth.GetUI(activity));
        }
    }
}