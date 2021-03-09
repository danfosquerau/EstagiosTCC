using EstagiosTCC.Util;
using System;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace EstagiosTCC.ViewModel
{
    public class SobreViewModel : BaseViewModel
    {
        public ICommand OpenWebCommand { get; set; }

        private string _nomeAplicativo = string.Empty;
        public string NomeAplicativo
        {
            get { return _nomeAplicativo; }
            set { SetProperty(ref _nomeAplicativo, value); }
        }
        
        private string _versao = string.Empty;
        public string Versao
        {
            get { return _versao; }
            set { SetProperty(ref _versao, value); }
        }

        public SobreViewModel()
        {
            Title = "Sobre";
            NomeAplicativo = "Estágios";
            Versao = "1.0";

            OpenWebCommand = new Command(() => Launcher.TryOpenAsync(new Uri("https://xamarin.com/platform")));
        }
    }
}