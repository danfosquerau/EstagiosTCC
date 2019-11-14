using EstagiosTCC.Util;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EstagiosTCC.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MenuPrincipalPage : MasterDetailPage
    {
        public MenuPrincipalPage()
        {
            InitializeComponent();
            MasterBehavior = MasterBehavior.Popover;
            lvwMenu.ItemsSource = new List<MainPageMenuItem>
            {
                new MainPageMenuItem {Codigo = MenuItemTipo.Inicio, Titulo="Início" },
                new MainPageMenuItem {Codigo = MenuItemTipo.NovoEstagio, Titulo="Novo Estágio" },
                new MainPageMenuItem {Codigo = MenuItemTipo.BuscarEstagio, Titulo="Buscar Estágios" },
                new MainPageMenuItem {Codigo = MenuItemTipo.MeusEstagios, Titulo="Meus Estágios" },
                new MainPageMenuItem {Codigo = MenuItemTipo.Favoritos, Titulo="Favoritos" },
                new MainPageMenuItem {Codigo = MenuItemTipo.MeuPerfil, Titulo = "Meu Perfil" },
                new MainPageMenuItem {Codigo = MenuItemTipo.Sobre, Titulo="Sobre" },
                new MainPageMenuItem {Codigo = MenuItemTipo.Sair, Titulo="Sair" }
            };
            lvwMenu.SelectedItem = 0;
            lvwMenu.ItemSelected += (sender, e) =>
            {
                if (e.SelectedItem == null)
                    return;

                var codigo = (int)((MainPageMenuItem)e.SelectedItem).Codigo;
                NavigateFromMenu(codigo);
            };
            imgPerfil.Source = string.IsNullOrEmpty(App.UsuarioLogadoDados.FotoPerfilUrl) ? "img_profile.jpg" : App.UsuarioLogadoDados.FotoPerfilUrl;
            lblNome.Text = App.UsuarioLogadoDados.Nome;
            lblEmail.Text = App.UsuarioLogadoDados.Email;
        }

        private void NavigateFromMenu(int codigo)
        {
            Page newPage = null;
            
            switch (codigo)
            {
                case (int)MenuItemTipo.Inicio:
                    newPage = new InicioPage();
                    break;
                case (int)MenuItemTipo.Sobre:
                    newPage = new SobrePage();
                    break;
                case (int)MenuItemTipo.MeuPerfil:
                    newPage = new PerfilPage();
                    break;
                case (int)MenuItemTipo.Sair:
                    OnLogoutAction();
                    break;
                case (int)MenuItemTipo.NovoEstagio:
                    newPage = new CadastrarEstagioPage(null);
                    break;
                case (int)MenuItemTipo.BuscarEstagio:
                    newPage = new BuscarEstagioPage();
                    break;
                case (int)MenuItemTipo.MeusEstagios:
                    newPage = new MeusEstagiosPage();
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
            App.UsuarioLogadoDados = null;
            Properties.RemoveLogin();
            Application.Current.MainPage = new NavigationPage(new IntroducaoPage());
        }

        public void AtualizarDadosUsuario()
        {
            imgPerfil.Source = string.IsNullOrEmpty(App.UsuarioLogadoDados.FotoPerfilUrl) ? "img_profile.jpg" : App.UsuarioLogadoDados.FotoPerfilUrl;
            lblNome.Text = App.UsuarioLogadoDados.Nome;
            lblEmail.Text = App.UsuarioLogadoDados.Email;

            DisplayAlert("Sucesso", "Dados Alterados","Ok");
        }

        public void IrParaTela(MenuItemTipo page)
        {
            NavigateFromMenu((int)page);
        }
    }

    public enum MenuItemTipo
    {
        Inicio,
        NovoEstagio,
        BuscarEstagio,
        Favoritos,
        MeusEstagios,
        MeuPerfil,
        Sobre,
        Sair
    }

    public class MainPageMenuItem
    {
        public MenuItemTipo Codigo { get; set; }
        public string Titulo { get; set; }
    }
}