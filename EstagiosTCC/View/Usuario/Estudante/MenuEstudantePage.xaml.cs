using EstagiosTCC.Util;
using EstagiosTCC.View.Estagio;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EstagiosTCC.View.Usuario.Estudante
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MenuEstudantePage : MasterDetailPage
    {
        private List<MainPageMenuItem> items;

        public MenuEstudantePage()
        {
            InitializeComponent();

            CarregarMenu(MenuItemTipo.Inicio);
        }

        public MenuEstudantePage(MenuItemTipo menuItem)
        {
            InitializeComponent();

            CarregarMenu(menuItem);
        }
        private void CarregarMenu(MenuItemTipo menuItem)
        {
            MasterBehavior = MasterBehavior.Popover;
            lvwMenu.ItemsSource = items = new List<MainPageMenuItem>
            {
                new MainPageMenuItem {Codigo = MenuItemTipo.Inicio, Titulo="Início" },
                new MainPageMenuItem {Codigo = MenuItemTipo.BuscarEstagio, Titulo="Buscar Estágios" },
                new MainPageMenuItem {Codigo = MenuItemTipo.Favoritos, Titulo="Favoritos" },
                new MainPageMenuItem {Codigo = MenuItemTipo.DadosCadastrais, Titulo = "Dados Cadastrais" },
                new MainPageMenuItem {Codigo = MenuItemTipo.Sobre, Titulo="Sobre" },
                new MainPageMenuItem {Codigo = MenuItemTipo.Sair, Titulo="Sair" }
            };
            lvwMenu.ItemSelected += (sender, e) =>
            {
                if (e.SelectedItem == null)
                    return;

                var item = lvwMenu.SelectedItem as MainPageMenuItem;
                NavigateFromMenu((int)item.Codigo);
                lvwMenu.SelectedItem = null;
            };
            lvwMenu.SelectedItem = items.Find(x => x.Codigo == menuItem);

            imgPerfil.Source = string.IsNullOrEmpty(App.EstudanteDados.FotoPerfilUrl) ?
                Device.RuntimePlatform.Equals(Device.UWP) ? "Resources/sem_imagem.png" : "sem_imagem.png" : App.EstudanteDados.FotoPerfilUrl;
            lblNome.Text = App.EstudanteDados.Nome;
            lblEmail.Text = App.EstudanteDados.Email;
        }

        private void NavigateFromMenu(int menu)
        {
            Page newPage = null;

            switch (menu)
            {
                case (int)MenuItemTipo.Inicio:
                    newPage = new InicioPage();
                    break;
                case (int)MenuItemTipo.Sobre:
                    newPage = new SobrePage();
                    break;
                case (int)MenuItemTipo.DadosCadastrais:
                    newPage = new DadosCadastraisPage();
                    break;
                case (int)MenuItemTipo.Sair:
                    OnLogoutAction();
                    break;
                case (int)MenuItemTipo.BuscarEstagio:
                    newPage = new BuscarEstagioPage();
                    break;
                case (int)MenuItemTipo.Favoritos:
                    newPage = new FavoritosPage();
                    break;
            }

            if (newPage != null && Detail != newPage)
            {
                Detail = new NavigationPage(newPage);
                IsPresented = false;
            }
        }

        private void OnLogoutAction()
        {
            ConnectionDB.Logout();

            App.UsuarioLogadoAuth = null;
            App.UsuarioLogado = null;
            App.EstudanteDados = null;

            Properties.RemoveLogin();

            Application.Current.MainPage = new NavigationPage(new IntroducaoPage());
        }

        public void AtualizarDadosUsuario()
        {
            imgPerfil.Source = string.IsNullOrEmpty(App.EstudanteDados.FotoPerfilUrl) ?
                Device.RuntimePlatform.Equals(Device.UWP) ? "Resources/sem_imagem.png" : "sem_imagem.png" : App.EstudanteDados.FotoPerfilUrl;
            lblNome.Text = App.EstudanteDados.Nome;
            lblEmail.Text = App.EstudanteDados.Email;

            DisplayAlert("Sucesso", "Dados Alterados", "Ok");
        }
    }

    public enum MenuItemTipo
    {
        Inicio,
        BuscarEstagio,
        Favoritos,
        DadosCadastrais,
        Sobre,
        Sair
    }

    public class MainPageMenuItem
    {
        public MenuItemTipo Codigo { get; set; }
        public string Titulo { get; set; }
    }
}