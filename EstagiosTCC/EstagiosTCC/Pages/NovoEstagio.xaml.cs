using EstagiosTCC.Models;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EstagiosTCC.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NovoEstagio : ContentPage
    {
        List<Curso> cursos;
        Stream fotoEstagio = null;

        public NovoEstagio()
        {
            InitializeComponent();
            carregarCursos();
            limparCampos();
            cursos = new List<Curso>();
            txtID.Text = "0";
        }
        public NovoEstagio(Estagio e)
        {
            InitializeComponent();
            preencherCampos(e);
            statusCampos(false);
            cursos = new List<Curso>();
        }

        private async void preencherCampos(Estagio e)
        {
            var responseCursos = await new Curso().listarCursos();
            listaCursos.ItemsSource = responseCursos;
            txtID.Text = e.idEstagio;
            txtEmpresa.Text = e.empresa;
            txtSetor.Text = e.setor;
            txtEndereco.Text = e.endereco;
            txtHorario.Text = e.horario;
            txtBolsa.Text = e.bolsa;
            txtAuxilioExtra.Text = e.auxilioExtra;
            txtDescricaoAtividades.Text = e.descricaoAtividades;
            imgTela.Source = e.imgAnexoURL;
            for (int i = 0; i < e.idCursos.Length; i++)
            {
                try
                {
                    foreach (Curso c in listaCursos.ItemsSource)
                    {
                        if (c.idCurso == e.idCursos[i])
                        {
                            cursos.Add(c);
                            listaCursosEstagio.ItemsSource = null;
                            listaCursosEstagio.ItemsSource = cursos;
                            break;
                        }
                    }
                }
                catch
                {
                }
            }
        }

        private void statusCampos(bool editar)
        {
            txtID.IsEnabled = editar;
            txtEmpresa.IsEnabled = editar;
            txtSetor.IsEnabled = editar;
            txtEndereco.IsEnabled = editar;
            txtHorario.IsEnabled = editar;
            txtBolsa.IsEnabled = editar;
            txtAuxilioExtra.IsEnabled = editar;
            txtDescricaoAtividades.IsEnabled = editar;
            imgTela.IsEnabled = editar;
            listaCursosEstagio.IsEnabled = editar;

            buttonSalvar.IsVisible = editar;
            buttonRemover.IsVisible = editar;
            buttonEscolher.IsVisible = editar;
            lblInfoImg.IsVisible = editar;
            listaCursos.IsVisible = editar;
            layoutListCursos.IsVisible = editar;
            btnEditar.IsVisible = editar ? false : true;
            btnExcluir.IsVisible = editar ? false : true;

            if (!editar)
            {
                gridCursos.ColumnDefinitions.Clear();
                gridCursos.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Absolute) });
                gridCursos.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) });
            }
            else
            {
                gridCursos.ColumnDefinitions.Clear();
                gridCursos.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            }
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
            txtEmpresa.Text = string.Empty;
            txtSetor.Text = string.Empty;
            txtEndereco.Text = string.Empty;
            txtBolsa.Text = string.Empty;
            txtAuxilioExtra.Text = string.Empty;
            txtDescricaoAtividades.Text = string.Empty;
            txtHorario.Text = string.Empty;
            listaCursosEstagio.ItemsSource = null;
            imgTela.Source = null;
            fotoEstagio = null;
        }

        public async void voltar()
        {
            await Navigation.PopAsync();
        }

        //MÉTODO APARENTEMENTE FUNCIONA, MAS AO EXECUTAR ELE PARA O APP
        public async void escolherTirarFoto()
        {
            //Tirar Foto
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                await DisplayAlert("Falha", "Sem câmeras disponíveis", "OK");
                return;
            }

            var foto = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
            {
                PhotoSize = PhotoSize.Full,
                CompressionQuality = 40,
                Directory = "AnexosEstagiosUnifacear",
                Name = "fotoEstagio.jpg"
            });

            if (foto == null)
            {
                imgTela.Source = null;
                fotoEstagio = null;
                return;
            }
            else
            {
                await DisplayAlert("Foto salva em", foto.Path, "OK");
                imgTela.Source = ImageSource.FromStream(() => foto.GetStream());
                fotoEstagio = foto.GetStream();
            }
        }

        //FUNCIONAL
        public async void escolherGaleria()
        {
            //Pegar foto da Galeria
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsPickPhotoSupported)
            {
                await DisplayAlert("Galeria", "Imagem não suportada", "OK");
                return;
            }

            var foto = await CrossMedia.Current.PickPhotoAsync(new PickMediaOptions
            {
                PhotoSize = PhotoSize.Full,
                CompressionQuality = 40
            });

            if (foto == null)
            {
                return;
            }
            else
            {
                imgTela.Source = ImageSource.FromStream(() => foto.GetStream());
                fotoEstagio = foto.GetStream();
            }
        }

        public async Task<bool> verificarPermissao(Plugin.Permissions.Abstractions.Permission permissaoParametro)
        {
            try
            {
                //MÉTODO GENÉRICO DE VERIFICAR PERMISSÃO

                //VERIFICA SE O APP POSSUI PERMISSÃO JÁ CONCEDIDA
                var permissionStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(permissaoParametro);

                //SE A PERMISSÃO NÃO ESTÁ CONCEDIDA
                if (permissionStatus != PermissionStatus.Granted)
                {
                    //PEDE A PERMISSÃO NA TELA > RESPOSTA=TRUE/FALSE
                    if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(permissaoParametro))
                    {
                        //PERMITIU
                        return true;
                    }
                    else
                    {
                        //NÃO PERMITIU
                        await DisplayAlert("Permissão negada", "Não foi possível conceder esta permissão", "OK");
                        return false;
                    }
                }
                //SE A PERMISSÃO ESTÁ CONCEDIDA
                else
                {
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async void btnEscolher(object sender, System.EventArgs e)
        {
            //Permissão de armazenamento
            bool resposta = await verificarPermissao(Permission.Storage);
            if (resposta)
            {
                escolherGaleria();
            }
            else
            {
                return;
            }
        }

        public void btnRemover(object sender, System.EventArgs e)
        {
            imgTela.Source = "";
            fotoEstagio = null;
        }

        public async void btnVoltar(object sender, System.EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private async void btnSalvar(object sender, System.EventArgs e)
        {
            Estagio estagio = new Estagio();
            estagio.idEstagio = txtID.Text;
            estagio.empresa = txtEmpresa.Text;
            estagio.setor = txtSetor.Text;
            estagio.endereco = txtEndereco.Text;
            estagio.bolsa = txtBolsa.Text;
            estagio.auxilioExtra = txtAuxilioExtra.Text;
            estagio.descricaoAtividades = txtDescricaoAtividades.Text;
            estagio.horario = txtHorario.Text;
            try
            {
                if (fotoEstagio == null)
                {
                    estagio.imgAnexoURL = imgTela.Source.IsEmpty ? "" : imgTela.Source.ToString().Replace("Uri: ", "");
                }
            }
            catch
            {
                estagio.imgAnexoURL = "";
            }
            if (cursos.Count > 0)
            {
                estagio.idCursos = new string[cursos.Count];
                for (var i = 0; i < cursos.Count; i++)
                {
                    estagio.idCursos[i] = cursos[i].idCurso;
                }
            }
            if (txtID.Text == "0")
            {
                //passar por parâmetro o valor do STREAM(Tem foto > passa valor / Não tem foto > null)
                bool resultado = await estagio.cadastrarEstagio(fotoEstagio);
                if (resultado)
                {
                    await DisplayAlert("Sucesso", "Novo Estagio foi cadastrado!", "OK");
                    voltar();
                }
                else
                {
                    await DisplayAlert("Falha", "Não foi possível cadastrar o Estágio, verifique sua conexão!", "OK");
                    voltar();
                }
            }
            else
            {
                //passar por parâmetro o valor do STREAM(Tem foto > passa valor / Não tem foto > null)
                bool resultado = await estagio.alterarEstagio(fotoEstagio);
                if (resultado)
                {
                    await DisplayAlert("Sucesso", "As informações do Estagio foram alteradas!", "OK");
                    voltar();
                }
                else
                {
                    await DisplayAlert("Falha", "Não foi possível alterar o Estágio, verifique sua conexão!", "OK");
                    voltar();
                }
            }
        }

        private void ListaCursosEstagio_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var tapped = e.Item as Curso;
            foreach (Curso c in cursos.ToList())
            {
                if (c.idCurso == tapped.idCurso)
                {
                    cursos.Remove(c);
                    listaCursosEstagio.ItemsSource = null;
                    listaCursosEstagio.ItemsSource = cursos;
                    break;
                }
            }
        }

        private void ListaCursos_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var tapped = e.Item as Curso;
            bool i = false;
            foreach (Curso c in cursos.ToList())
            {
                if (c.idCurso == tapped.idCurso)
                {
                    i = true;
                    break;
                }
            }
            if (!i)
            {
                cursos.Add(tapped);
                listaCursosEstagio.ItemsSource = null;
                listaCursosEstagio.ItemsSource = cursos;
            }
        }

        private void btnEditar_Clicked(object sender, EventArgs e)
        {
            statusCampos(true);
        }


        private async void BtnExcluir_Clicked(object sender, EventArgs e)
        {
            Estagio estagio = new Estagio();
            estagio.idEstagio = txtID.Text;
            bool resultado = await estagio.deletarEstagio();
            if (resultado)
            {
                await DisplayAlert("Sucesso", "Estágio foi excluído com sucesso!", "OK");
                await Navigation.PopAsync();
            }
            else
            {
                await DisplayAlert("Falha", "Estágio não foi excluído, verifique a sua conexão.", "OK");
                await Navigation.PopAsync();
            }
        }
    }
}