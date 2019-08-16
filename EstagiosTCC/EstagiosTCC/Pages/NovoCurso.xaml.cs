using EstagiosTCC.Models;
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
    public partial class NovoCurso : ContentPage
    {
        public NovoCurso()
        {
            InitializeComponent();
            carregarCursos();
            limparCampos();

        }
        private async void carregarCursos()
        {
            Curso c = new Curso();
            var responseCursos = await c.listarCursos();
            listaCursos.ItemsSource = responseCursos;
        }

        public void limparCampos()
        {
            txtID.Text = "0";
            txtNomeCurso.Text = string.Empty;
            txtCoordenador.Text = string.Empty;
            carregarCursos();
        }

        private void cursoTapped(object sender, ItemTappedEventArgs e)
        {
            var tapped = e.Item as Curso;
            txtID.Text = tapped.idCurso;
            txtNomeCurso.Text = tapped.nomeCurso;
            txtCoordenador.Text = tapped.coordenador;
        }

        private async void btnExcluir(object sender, System.EventArgs e)
        {
            if (txtID.Text == "0")
            {
                await DisplayAlert("Falha", "Você deve selecionar um curso para excluí-lo!", "OK");
                limparCampos();
            }
            else
            {
                Curso c = new Curso();
                c.idCurso = txtID.Text;
                bool resultado = await c.deletarCurso();
                if (resultado)
                {
                    await DisplayAlert("Sucesso", "Curso foi excluído com sucesso!", "OK");
                    limparCampos();
                }
                else
                {
                    await DisplayAlert("Falha", "Curso não foi excluído, verifique a sua conexão.", "OK");
                    limparCampos();
                }
            }
        }

        private void btnLimpar(object sender, System.EventArgs e)
        {
            limparCampos();
        }

        private async void btnSalvar(object sender, System.EventArgs e)
        {
            Curso curso = new Curso();
            curso.idCurso = txtID.Text;
            curso.nomeCurso = txtNomeCurso.Text;
            curso.coordenador = txtCoordenador.Text;
            if (txtID.Text == "0")
            {
                bool resultado = await curso.cadastrarCurso();
                if (resultado)
                {
                    await DisplayAlert("Sucesso", "Novo curso foi cadastrado!", "OK");
                    limparCampos();
                }
                else
                {
                    await DisplayAlert("Falha", "Novo curso não foi cadastrado, verifique a sua conexão.", "OK");
                    limparCampos();
                }
            }
            else
            {
                bool resultado = await curso.alterarCurso();
                if (resultado)
                {
                    await DisplayAlert("Sucesso", "As informações do Curso foram alteradas!", "OK");
                    limparCampos();
                }
                else
                {
                    await DisplayAlert("Falha", "As informações do Curso não foram alteradas, verifique a sua conexão.", "OK");
                    limparCampos();
                }
            }
        }

    }
}