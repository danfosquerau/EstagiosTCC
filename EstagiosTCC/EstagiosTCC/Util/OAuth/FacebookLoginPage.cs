using Xamarin.Forms;

namespace EstagiosTCC.Util.OAuth
{
    public class FacebookLoginPage : ContentPage
    {
        public FacebookLoginPage()
        {
            Content = new StackLayout
            {
                Children = {
                    new Label { Text = "Aguarde..." }
                }
            };
        }
    }
}