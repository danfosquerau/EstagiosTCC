using Firebase.Database;
using Firebase.Database.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstagiosTCC.Models
{
    public class Curso
    {
        private readonly string ENDERECO_FIREBASE = "https://estagios-tcc-app.firebaseio.com/";
        private readonly FirebaseClient database;

        public Curso()
        {
            database = new FirebaseClient(ENDERECO_FIREBASE);
        }

        //Identificação do Curso
        public string idCurso { get; set; } = "";
        public string idPublicador { get; set; } = "";
        //Informações do Curso
        public string nomeCurso { get; set; } = "";
        public string coordenador { get; set; } = "";
        public string ultimaAtualizacao { get; set; } = "";


        //Firebase

        //--TESTADO E FUNCIONANDO--
        public async Task<bool> cadastrarCurso()
        {
            try
            {
                //CADASTRA AS INFORMAÇÕES INICIAIS DO CURSO
                var postCurso = await database.Child("Cursos").PostAsync(new Curso()
                {
                    idCurso = this.idCurso,
                    idPublicador = this.idPublicador,
                    nomeCurso = this.nomeCurso,
                    coordenador = this.coordenador,
                    ultimaAtualizacao = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")
                });
                //PEGA A KEY DO REGISTRO DO FIREBASE E SETA COMO ID DO CURSO
                this.idCurso = postCurso.Key;
                //PUT(UPDATE) NAS INFORMAÇÕES
                await database.Child("Cursos").Child(postCurso.Key).PutAsync(new Curso()
                {
                    idCurso = this.idCurso,
                    idPublicador = this.idPublicador,
                    nomeCurso = this.nomeCurso,
                    coordenador = this.coordenador,
                    ultimaAtualizacao = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")
                });

                return true;
            }
            catch (FirebaseException)
            {
                return false;
            }
        }

        //--TESTADO E FUNCIONANDO--
        public async Task<bool> alterarCurso()
        {
            try
            {
                //PEGA O CURSO PASSADO PELO ID
                var putCurso = (await database.Child("Cursos").OnceAsync<Curso>())
                               .Where(a => a.Object.idCurso == this.idCurso).FirstOrDefault();

                //PEGA O ID DO REGISTRO DO FIREBASE DO CURSO
                this.idCurso = putCurso.Key;

                //PUT(UPDATE) NAS INFORMAÇÕES
                await database.Child("Cursos").Child(putCurso.Key).PutAsync(new Curso()
                {
                    idCurso = this.idCurso,
                    idPublicador = this.idPublicador,
                    nomeCurso = this.nomeCurso,
                    coordenador = this.coordenador,
                    ultimaAtualizacao = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")
                });

                return true;
            }
            catch (FirebaseException)
            {
                return false;
            }
        }

        //--TESTADO E FUNCIONANDO--
        public async Task<bool> deletarCurso()
        {
            try
            {
                //PEGA O CURSO PASSADO PELO ID
                var deleteCurso = (await database.Child("Cursos").OnceAsync<Curso>())
                                  .Where(a => a.Object.idCurso == this.idCurso).FirstOrDefault();

                //DELETA O REGISTRO DO CURSO
                await database.Child("Cursos").Child(deleteCurso.Key).DeleteAsync();

                return true;
            }
            catch (FirebaseException)
            {
                return false;
            }
        }

        //--TESTADO E FUNCIONANDO--
        public async Task<List<Curso>> listarCursos()
        {
            try
            {
                return (await database.Child("Cursos").OnceAsync<Curso>())
                       .Select(item => new Curso
                       {
                           idCurso = item.Object.idCurso,
                           idPublicador = item.Object.idPublicador,
                           nomeCurso = item.Object.nomeCurso,
                           coordenador = item.Object.coordenador,
                           ultimaAtualizacao = item.Object.ultimaAtualizacao
                       }).ToList();
            }
            catch (FirebaseException)
            {
                return null;
            }
        }

        //--
        public async Task<Curso> buscarCursoPorID(string idCursoParametro)
        {
            try
            {
                var todosCursos = await listarCursos();
                return todosCursos.Where(a => a.idCurso == idCursoParametro).FirstOrDefault();
            }
            catch (FirebaseException)
            {
                return null;
            }
        }

    }
}
