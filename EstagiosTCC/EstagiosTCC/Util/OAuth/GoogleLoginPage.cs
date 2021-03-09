using Xamarin.Forms;

namespace EstagiosTCC.Util.OAuth
{
    public class GoogleLoginPage : ContentPage
    {
        public GoogleLoginPage()
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