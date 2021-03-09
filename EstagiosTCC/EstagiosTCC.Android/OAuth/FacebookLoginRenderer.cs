using Android.App;
using Android.Content;
using EstagiosTCC.Droid.OAuth;
using EstagiosTCC.Util.OAuth;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(FacebookLoginPage), typeof(FacebookLoginRenderer))]
namespace EstagiosTCC.Droid.OAuth
{
    public class FacebookLoginRenderer : PageRenderer
    {
        public FacebookLoginRenderer(Context context) : base(context) { }

        protected override void OnElementChanged(ElementChangedEventArgs<Page> e)
        {
            base.OnElementChanged(e);

            OAuthProviderSetting oauth = new OAuthProviderSetting();
            var auth = oauth.LoginWithProvider("Facebook");

            var activity = Context as Activity;
            activity.StartActivity(auth.GetUI(activity));
        }
    }
}