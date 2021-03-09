using EstagiosTCC.Model;
using EstagiosTCC.Util;
using Firebase.Database;
using Firebase.Database.Query;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace EstagiosTCC.Dao
{
    public static class EstagioDao
    {
        public static async Task<bool> Inserir(Estagio estagio, Stream anexo)
        {
            try
            {
                //CADASTRA AS INFORMAÇÕES INICIAIS DO ESTÁGIO
                var postEstagio = await ConnectionDB.Database.Child("Estagio").PostAsync(estagio);
                
                //PEGA O ID DO REGISTRO DO FIREBASE DO ESTÁGIO
                estagio.Codigo = postEstagio.Key;

                if (anexo != null)
                {
                    //CADASTRA A IMAGEM DO ANEXO NO STORAGE
                    var url = await ConnectionDB.Storage.Child("Anexo")
                        .Child(estagio.Codigo + ".jpg").PutAsync(anexo);
                    
                    //PEGA O ENDEREÇO DA IMAGEM SALVA 
                    estagio.AnexoUrl = url;
                }

                //DEFINE A ULTIMA VEZ EM QUE FOI ATUALIZADO
                estagio.UltimaAtualizacao = DateTime.Now;

                //PUT(UPDATE) NAS INFORMAÇÕES
                await ConnectionDB.Database.Child("Estagio").Child(estagio.Codigo).PutAsync(estagio);

                EmpresaDao.NovoEstagio(estagio.Codigo);

                return true;
            }
            catch (FirebaseException e)
            {
                throw new Exception(e.Message);
            }
        }

        public static async Task<bool> Alterar(Estagio estagio, Stream anexo)
        {
            try
            {
                if (anexo != null)
                {
                    //CADASTRA A IMAGEM DO ANEXO NO STORAGE
                    var url = await ConnectionDB.Storage.Child("Anexo")
                        .Child(estagio.Codigo + ".jpg").PutAsync(anexo);
                    
                    //PEGA O ENDEREÇO DA IMAGEM SALVA 
                    estagio.AnexoUrl = url;
                }

                //DEFINE A ULTIMA VEZ EM QUE FOI ATUALIZADO
                estagio.UltimaAtualizacao = DateTime.Now;
                
                //PUT(UPDATE) NAS INFORMAÇÕES
                await ConnectionDB.Database.Child("Estagio").Child(estagio.Codigo).PutAsync(estagio);

                return true;
            }
            catch (FirebaseException e)
            {
                throw new Exception(e.Message);
            }
        }

        public static async Task<List<Estagio>> Buscar()
        {
            try
            {
                return (await ConnectionDB.Database.Child("Estagio").OnceAsync<Estagio>())
                       .Select(item => item.Object).Where(x => x.Status == Status.Disponivel)
                       .OrderByDescending(x => x.UltimaAtualizacao).ToList();
            }
            catch (FirebaseException e)
            {
                throw new Exception(e.Message);
            }
        }

        public static async Task<Estagio> BuscarPeloCodigo(string codigo)
        {
            try
            {
                return await ConnectionDB.Database.Child("Estagio").Child(codigo)
                    .OnceSingleAsync<Estagio>();
            }
            catch (FirebaseException e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}