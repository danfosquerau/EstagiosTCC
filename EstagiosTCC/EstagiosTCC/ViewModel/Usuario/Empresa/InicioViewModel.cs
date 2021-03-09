using EstagiosTCC.Dao;
using EstagiosTCC.Util;
using EstagiosTCC.View.Usuario.Empresa;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Xamarin.Forms;

namespace EstagiosTCC.ViewModel.Usuario.Empresa
{
    public class InicioViewModel : BaseViewModel
    {
        public ICommand CadastrarCommand { get; set; }

        public ObservableCollection<Model.Estagio> ListaEstagios { get; set; }

        public InicioViewModel()
        {
            Title = "Início";
            ListaEstagios = new ObservableCollection<Model.Estagio>();
            CarregarEstagios();

            CadastrarCommand = new Command(() => OnCadastrar());
        }

        public async void CarregarEstagios()
        {
            ListaEstagios.Clear();

            if (App.EmpresaDados.MeusEstagios.Count > 0)
            {
                for (int i = 0; i < App.EmpresaDados.MeusEstagios.Count; i++)
                {
                    var estagio = await EstagioDao.BuscarPeloCodigo(App.EmpresaDados.MeusEstagios[i]);
                    ListaEstagios.Add(estagio);
                }
            }
        }

        private void OnCadastrar()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                (Application.Current.MainPage as MenuEmpresaPage).IrParaTela(MenuItemTipo.NovoEstagio);
            }
            catch (Exception ex)
            {
                Application.Current.MainPage.DisplayAlert("Erro", ex.Message, "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}