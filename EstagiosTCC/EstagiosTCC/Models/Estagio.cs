using Firebase.Database;
using Firebase.Database.Query;
using Firebase.Storage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace EstagiosTCC.Models
{
    public class Estagio
    {
        private readonly string ENDERECO_DATABASE = "https://estagios-tcc-app.firebaseio.com/";
        private readonly string ENDERECO_STORAGE = "estagios-tcc-app.appspot.com";
        private readonly FirebaseClient database;
        private readonly FirebaseStorage storage;

        public Estagio()
        {
            database = new FirebaseClient(ENDERECO_DATABASE);
            storage = new FirebaseStorage(ENDERECO_STORAGE);
        }

        //Identificação do registro
        public string idEstagio { get; set; } = "";
        public string idPublicador { get; set; } = "";
        public string[] idCursos { get; set; } = { "" };
        public string imgAnexoURL { get; set; } = "";
        //Informações da vaga de estágio
        public string empresa { get; set; } = "";
        public string setor { get; set; } = "";
        public string endereco { get; set; } = "";
        public string descricaoAtividades { get; set; } = "";
        public string horario { get; set; } = "";
        public string bolsa { get; set; } = "";
        public string auxilioExtra { get; set; } = "";
        //Data da última atualização da publicação e alunos interessados
        public string ultimaAtualizacao { get; set; } = "";
        public string[] idAlunos { get; set; } = { "" };


        //Firebase

        //--TESTADO E FUNCIONANDO--
        public async Task<bool> cadastrarEstagio(Stream anexo)
        {
            try
            {
                if (anexo != null)
                {
                    //CADASTRA AS INFORMAÇÕES INICIAIS DO ESTÁGIO
                    var postEstagio = await database.Child("Estagio").PostAsync(new Estagio()
                    {
                        idEstagio = this.idEstagio,
                        idPublicador = this.idPublicador,
                        idCursos = this.idCursos,
                        imgAnexoURL = this.imgAnexoURL,
                        empresa = this.empresa,
                        setor = this.setor,
                        endereco = this.endereco,
                        descricaoAtividades = this.descricaoAtividades,
                        horario = this.horario,
                        bolsa = this.bolsa,
                        auxilioExtra = this.auxilioExtra,
                        ultimaAtualizacao = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"),
                        idAlunos = this.idAlunos
                    });

                    //CADASTRA A IMAGEM DO ANEXO NO STORAGE
                    var imgURL = await storage.Child("Anexos").Child(postEstagio.Key + ".jpg").PutAsync(anexo);
                    //PEGA O ENDEREÇO DA IMAGEM SALVA E O ID DO REGISTRO DO FIREBASE DO ESTÁGIO
                    this.imgAnexoURL = imgURL;
                    this.idEstagio = postEstagio.Key;

                    //PUT(UPDATE) NAS INFORMAÇÕES
                    await database.Child("Estagio").Child(postEstagio.Key).PutAsync(new Estagio()
                    {
                        idEstagio = this.idEstagio,
                        idPublicador = this.idPublicador,
                        idCursos = this.idCursos,
                        imgAnexoURL = this.imgAnexoURL,
                        empresa = this.empresa,
                        setor = this.setor,
                        endereco = this.endereco,
                        descricaoAtividades = this.descricaoAtividades,
                        horario = this.horario,
                        bolsa = this.bolsa,
                        auxilioExtra = this.auxilioExtra,
                        ultimaAtualizacao = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"),
                        idAlunos = this.idAlunos
                    });

                    return true;
                }
                else
                {
                    this.imgAnexoURL = "";

                    //CADASTRA AS INFORMAÇÕES INICIAIS DO ESTÁGIO
                    var postEstagio = await database.Child("Estagio").PostAsync(new Estagio()
                    {
                        idEstagio = this.idEstagio,
                        idPublicador = this.idPublicador,
                        idCursos = this.idCursos,
                        imgAnexoURL = this.imgAnexoURL,
                        empresa = this.empresa,
                        setor = this.setor,
                        endereco = this.endereco,
                        descricaoAtividades = this.descricaoAtividades,
                        horario = this.horario,
                        bolsa = this.bolsa,
                        auxilioExtra = this.auxilioExtra,
                        ultimaAtualizacao = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"),
                        idAlunos = this.idAlunos
                    });
                    //PEGA A KEY DO REGISTRO DO FIREBASE E SETA COMO ID DO ESTÁGIO
                    this.idEstagio = postEstagio.Key;
                    //PUT(UPDATE) NAS INFORMAÇÕES
                    await database.Child("Estagio").Child(postEstagio.Key).PutAsync(new Estagio()
                    {
                        idEstagio = this.idEstagio,
                        idPublicador = this.idPublicador,
                        idCursos = this.idCursos,
                        imgAnexoURL = this.imgAnexoURL,
                        empresa = this.empresa,
                        setor = this.setor,
                        endereco = this.endereco,
                        descricaoAtividades = this.descricaoAtividades,
                        horario = this.horario,
                        bolsa = this.bolsa,
                        auxilioExtra = this.auxilioExtra,
                        ultimaAtualizacao = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"),
                        idAlunos = this.idAlunos
                    });

                    return true;
                }
            }
            catch (FirebaseException)
            {
                return false;
            }
        }

        //--
        public async Task<bool> alterarEstagio(Stream anexo)
        {
            try
            {
                if (anexo != null)
                {
                    //PEGA O ESTÁGIO PASSADO PELO ID
                    var putEstagio = (await database.Child("Estagio").OnceAsync<Estagio>())
                                     .Where(a => a.Object.idEstagio == this.idEstagio).FirstOrDefault();
                    try
                    {
                        //REMOVE A IMAGEM ANTIGA DO ANEXO NO STORAGE SE TIVER
                        await storage.Child("Anexos").Child(putEstagio.Key + ".jpg").DeleteAsync();
                    }
                    catch { }

                    //CADASTRA A IMAGEM DO ANEXO NO STORAGE
                    var imgURL = await storage.Child("Anexos").Child(putEstagio.Key + ".jpg").PutAsync(anexo);

                    //PEGA O ENDEREÇO DA IMAGEM SALVA E O ID DO REGISTRO DO FIREBASE DO ESTÁGIO
                    this.imgAnexoURL = imgURL;
                    this.idEstagio = putEstagio.Key;

                    //PUT(UPDATE) NAS INFORMAÇÕES
                    await database.Child("Estagio").Child(putEstagio.Key).PutAsync(new Estagio()
                    {
                        idEstagio = this.idEstagio,
                        idPublicador = this.idPublicador,
                        idCursos = this.idCursos,
                        imgAnexoURL = this.imgAnexoURL,
                        empresa = this.empresa,
                        setor = this.setor,
                        endereco = this.endereco,
                        descricaoAtividades = this.descricaoAtividades,
                        horario = this.horario,
                        bolsa = this.bolsa,
                        auxilioExtra = this.auxilioExtra,
                        ultimaAtualizacao = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"),
                        idAlunos = this.idAlunos
                    });

                    return true;
                }
                else
                {
                    //PEGA O ESTÁGIO PASSADO PELO ID
                    var putEstagio = (await database.Child("Estagio").OnceAsync<Estagio>())
                                     .Where(a => a.Object.idEstagio == this.idEstagio).FirstOrDefault();

                    //PEGA O ID DO REGISTRO DO FIREBASE DO ESTÁGIO
                    this.idEstagio = putEstagio.Key;

                    //PUT(UPDATE) NAS INFORMAÇÕES
                    await database.Child("Estagio").Child(putEstagio.Key).PutAsync(new Estagio()
                    {
                        idEstagio = this.idEstagio,
                        idPublicador = this.idPublicador,
                        idCursos = this.idCursos,
                        imgAnexoURL = this.imgAnexoURL,
                        empresa = this.empresa,
                        setor = this.setor,
                        endereco = this.endereco,
                        descricaoAtividades = this.descricaoAtividades,
                        horario = this.horario,
                        bolsa = this.bolsa,
                        auxilioExtra = this.auxilioExtra,
                        ultimaAtualizacao = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"),
                        idAlunos = this.idAlunos
                    });

                    return true;
                }
            }
            catch (FirebaseException)
            {
                return false;
            }
        }

        //--
        public async Task<bool> deletarEstagio()
        {
            try
            {
                //PEGA ESTAGIO PELO ID
                var deleteEstagio = (await database.Child("Estagio").OnceAsync<Estagio>())
                                    .Where(a => a.Object.idEstagio == this.idEstagio).FirstOrDefault();

                //VERIFICA SE O ESTÁGIO POSSUI IMAGEM DE ANEXO
                if (!deleteEstagio.Object.imgAnexoURL.Equals(""))
                {
                    //DELETA A IMAGEM DE ANEXO DO ESTÁGIO
                    await storage.Child("Anexos").Child(deleteEstagio.Key + ".jpg").DeleteAsync();
                }

                //DELETA O REGISTRO DO ESTÁGIO
                await database.Child("Estagio").Child(deleteEstagio.Key).DeleteAsync();

                return true;
            }
            catch (FirebaseException)
            {
                return false;
            }
        }

        //--TESTADO E FUNCIONANDO--
        public async Task<List<Estagio>> listarEstagios()
        {
            try
            {
                return (await database.Child("Estagio").OnceAsync<Estagio>())
                       .Select(item => new Estagio
                       {
                           idEstagio = item.Object.idEstagio,
                           idPublicador = item.Object.idPublicador,
                           idCursos = item.Object.idCursos,
                           imgAnexoURL = item.Object.imgAnexoURL,
                           empresa = item.Object.empresa,
                           setor = item.Object.setor,
                           endereco = item.Object.endereco,
                           descricaoAtividades = item.Object.descricaoAtividades,
                           horario = item.Object.horario,
                           bolsa = item.Object.bolsa,
                           auxilioExtra = item.Object.auxilioExtra,
                           ultimaAtualizacao = item.Object.ultimaAtualizacao,
                           idAlunos = item.Object.idAlunos
                       }).ToList();
            }
            catch (FirebaseException)
            {
                return null;
            }
        }

        //--TESTADO E FUNCIONANDO--
        public async Task<List<Estagio>> listarEstagios(string idCursoParametro)
        {
            try
            {
                var todosEstagios = await listarEstagios();
                return todosEstagios.Where(a => a.idCursos.Contains(idCursoParametro)).ToList();
            }
            catch (FirebaseException)
            {
                return null;
            }
        }

        //--
        public async Task<Estagio> buscarEstagioPorID(string idEstagioParametro)
        {
            try
            {
                var todosEstagios = await listarEstagios();
                return todosEstagios.Where(a => a.idEstagio == idEstagioParametro).FirstOrDefault();
            }
            catch (FirebaseException)
            {
                return null;
            }
        }
    }
}
