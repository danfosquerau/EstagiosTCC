using EstagiosTCC.Models;
using System.Collections.Generic;
using System.ComponentModel;
using Xamarin.Forms;

namespace EstagiosTCC.Views
{
    [DesignTimeVisible(false)]
    public partial class MainPage : MasterDetailPage
    {
        public MainPage()
        {
            InitializeComponent();

            MasterBehavior = MasterBehavior.Popover;

            lvwMenu.ItemsSource = new List<MainPageMenuItem>
            {
                new MainPageMenuItem {Uid = MenuItemType.Home, Title="Início" },
                new MainPageMenuItem {Uid = MenuItemType.Course, Title="Cursos" },
                new MainPageMenuItem {Uid = MenuItemType.NewInternship, Title="Novo Estágio" },
                new MainPageMenuItem {Uid = MenuItemType.SearchInternship, Title="Procurar Estágio" },
                new MainPageMenuItem {Uid = MenuItemType.MyInterships, Title="Meus Estágios" },
                new MainPageMenuItem {Uid = MenuItemType.MyProfile, Title="Meu Perfil" },
                new MainPageMenuItem {Uid = MenuItemType.About, Title="Sobre" },
                new MainPageMenuItem {Uid = MenuItemType.Logout, Title="Sair" }
            };

            lvwMenu.SelectedItem = 0;
            lvwMenu.ItemSelected += (sender, e) =>
            {
                if (e.SelectedItem == null)
                    return;

                var uid = (int)((MainPageMenuItem)e.SelectedItem).Uid;
                NavigateFromMenu(uid);
            };
        }

        private void NavigateFromMenu(int id)
        {
            Page newPage = null;
            switch (id)
            {
                case (int)MenuItemType.Home:
                    newPage = new HomePage();
                    break;
                case (int)MenuItemType.Course:
                    newPage = new CoursePage();
                    break;
                case (int)MenuItemType.NewInternship:
                    newPage = new EditInternshipPage(null);
                    break;
                case (int)MenuItemType.SearchInternship:
                    newPage = new SearchInternshipPage();
                    break;
                case (int)MenuItemType.MyInterships:
                    newPage = new MyInternshipsPage();
                    break;
                case (int)MenuItemType.MyProfile:
                    newPage = new MyProfilePage();
                    break;
                case (int)MenuItemType.About:
                    newPage = new AboutPage();
                    break;
                case (int)MenuItemType.Logout:
                    MessagingCenter.Send<object>(this, App.EVENT_LOGOUT);
                    break;
            }
            if (newPage != null && Detail != newPage)
            {
                Detail = new NavigationPage(newPage);
                IsPresented = false;
            }
        }
    }
}