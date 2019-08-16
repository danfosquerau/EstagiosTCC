using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EstagiosTCC.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private async void btnBuscarEstagio(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new BuscarEstagio());
        }

        private async void btnNovoEstagio(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new NovoEstagio());
        }

        private async void btnNovoCurso(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new NovoCurso());
        }
    }
}