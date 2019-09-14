using EstagiosTCC.Views;
using Xamarin.Forms;

namespace EstagiosTCC
{
    public partial class App : Application
    {
        public static string EVENT_RETURN_HOME_PAGE = "EVENT_RETURN_HOME_PAGE";
        public static string EVENT_LOGIN = "EVENT_LOGIN";
        public static string EVENT_LOGOUT = "EVENT_LOGOUT";

        private readonly MainPage mainPage; 

        public App()
        {
            InitializeComponent();

            MessagingCenter.Subscribe<object>(this, EVENT_RETURN_HOME_PAGE, ReturnHomePage);
            MessagingCenter.Subscribe<object>(this, EVENT_LOGOUT, SetLoginPageAsRootPage);
            MessagingCenter.Subscribe<object>(this, EVENT_LOGIN, SetMainPageAsRootPage);

            MainPage = new NavigationPage(new LoginPage());

            mainPage = new MainPage();
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }

        private async void ReturnHomePage(object sender)
        {
            await mainPage.Detail.Navigation.PopToRootAsync();
        }

        private void SetLoginPageAsRootPage(object sender)
        {
            MainPage = new NavigationPage(new LoginPage());
        }

        private void SetMainPageAsRootPage(object sender)
        {
            MainPage = mainPage;
        }
    }
}