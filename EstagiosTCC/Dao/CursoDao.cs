using EstagiosTCC.Model;
using EstagiosTCC.Util;
using Firebase.Database;
using Firebase.Database.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EstagiosTCC.Dao
{
    public static class CursoDao
    {
        public static async Task<bool> Inserir(Curso curso)
        {
            try
            {
                //CADASTRA AS INFORMAÇÕES INICIAIS DO CURSO
                var postCurso = await ConnectionDB.Database.Child("Curso").PostAsync(curso);
                //PEGA A KEY DO REGISTRO DO FIREBASE E SETA COMO ID DO CURSO
                curso.Codigo = postCurso.Key;
                //PUT(UPDATE) NAS INFORMAÇÕES
                await ConnectionDB.Database.Child("Curso").Child(postCurso.Key).PutAsync(curso);
                
                return true;
            }
            catch (FirebaseException e)
            {
                throw new Exception(e.Message);
            }
        }

        public static async Task<bool> Alterar(Curso curso)
        {
            try
            {
                //PUT(UPDATE) NAS INFORMAÇÕES
                await ConnectionDB.Database.Child("Curso").Child(curso.Codigo).PutAsync(curso);
                
                return true;
            }
            catch (FirebaseException e)
            {
                throw new Exception(e.Message);
            }
        }

        public static async Task<List<Curso>> Buscar()
        {
            try
            {
                return (await ConnectionDB.Database.Child("Curso").OnceAsync<Curso>())
                       .Select(item => item.Object).ToList();
            }
            catch (FirebaseException e)
            {
                throw new Exception(e.Message);
            }
        }

        public static async Task<Curso> BuscarPeloCodigo(string codigo)
        {
            try
            {
                return (await ConnectionDB.Database.Child("Curso").Child(codigo)
                    .OnceAsync<Curso>()).FirstOrDefault().Object;
            }
            catch (FirebaseException e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}