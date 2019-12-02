using EstagiosTCC.Util;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EstagiosTCC.View.Usuario.Empresa
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MenuEmpresaPage : MasterDetailPage 
    {
        private List<MainPageMenuItem> items;

        public MenuEmpresaPage()
        {
            InitializeComponent();

            CarregarMenu(MenuItemTipo.Inicio);
        }

        public MenuEmpresaPage(MenuItemTipo menuItem)
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
                new MainPageMenuItem {Codigo = MenuItemTipo.NovoEstagio, Titulo="Novo Estágio" },
                new MainPageMenuItem {Codigo = MenuItemTipo.MeusEstagios, Titulo="Meus Estágios" },
                new MainPageMenuItem {Codigo = MenuItemTipo.DadosCadastrais, Titulo = "Dados Cadastrais" },
                new MainPageMenuItem {Codigo = MenuItemTipo.Sobre, Titulo="Sobre" },
                new MainPageMenuItem {Codigo = MenuItemTipo.Sair, Titulo="Sair" }
            };
            lvwMenu.ItemSelected += (sender, e) =>
            {
                if (e.SelectedItem == null)
                    return;

                var item = lvwMenu.SelectedItem as MainPageMenuItem;
                NavigateFromMenu(item.Codigo);
                lvwMenu.SelectedItem = null;
            };
            lvwMenu.SelectedItem = items.Find(x => x.Codigo == menuItem);

            imgLogo.Source = string.IsNullOrEmpty(App.EmpresaDados.LogoEmpresa) ? 
                Device.RuntimePlatform.Equals(Device.UWP)? "Resources/sem_imagem.png" : "sem_imagem.png" : App.EmpresaDados.LogoEmpresa;
            lblEmpresa.Text = App.EmpresaDados.NomeEmpresa;
            lblEmail.Text = App.EmpresaDados.Email;
        }

        private void NavigateFromMenu(MenuItemTipo menu)
        {
            Page newPage = null;

            switch (menu)
            {
                case MenuItemTipo.Inicio:
                    newPage = new InicioPage();
                    break;
                case MenuItemTipo.Sobre:
                    newPage = new SobrePage();
                    break;
                case MenuItemTipo.DadosCadastrais:
                    newPage = new DadosCadastraisPage();
                    break;
                case MenuItemTipo.Sair:
                    OnLogoutAction();
                    break;
                case MenuItemTipo.NovoEstagio:
                    newPage = new CadastrarEstagioPage(null);
                    break;
                case MenuItemTipo.MeusEstagios:
                    newPage = new MeusEstagiosPage();
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
            App.EmpresaDados = null;

            Properties.RemoveLogin();

            Application.Current.MainPage = new NavigationPage(new IntroducaoPage());
        }

        public void IrParaTela(MenuItemTipo page)
        {
            lvwMenu.SelectedItem = items.Find(x => x.Codigo == page);
        }

        public void AtualizarDadosUsuario()
        {
            imgLogo.Source = string.IsNullOrEmpty(App.EmpresaDados.LogoEmpresa) ?
                Device.RuntimePlatform.Equals(Device.UWP) ? "Resources/sem_imagem.png" : "sem_imagem.png" : App.EmpresaDados.LogoEmpresa;
            lblEmpresa.Text = App.EmpresaDados.NomeEmpresa;
            lblEmail.Text = App.EmpresaDados.Email;

            DisplayAlert("Sucesso", "Dados Alterados", "Ok");
        }

    }

    public enum MenuItemTipo
    {
        Inicio,
        NovoEstagio,
        MeusEstagios,
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