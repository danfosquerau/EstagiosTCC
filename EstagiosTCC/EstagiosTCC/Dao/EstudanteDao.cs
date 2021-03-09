using EstagiosTCC.Model;
using EstagiosTCC.Util;
using Firebase.Database;
using Firebase.Database.Query;
using System;
using System.IO;
using System.Threading.Tasks;

namespace EstagiosTCC.Dao
{
    public static class EstudanteDao
    {
        public static async Task<bool> Inserir(Estudante estudante, Stream fotoPerfil)
        {
            try
            {
                if (fotoPerfil != null)
                {
                    //CADASTRA A IMAGEM DO PERFIL NO STORAGE
                    var url = await ConnectionDB.Storage.Child("FotoPerfil")
                        .Child(estudante.Codigo + ".jpg").PutAsync(fotoPerfil);
                    //PEGA O ENDEREÇO DA IMAGEM SALVA 
                    estudante.FotoPerfilUrl = url;
                }

                //CADASTRA AS INFORMAÇÕES DO USUARIO NO BANCO
                await ConnectionDB.Database.Child("Estudante").Child(estudante.Codigo).PostAsync(estudante);
                await ConnectionDB.Database.Child("Estudante").Child(estudante.Codigo).PutAsync(estudante);
                App.EstudanteDados = estudante;

                return true;
            }
            catch (FirebaseException e)
            {
                throw new Exception(e.Message);
            }
        }

        public static async Task<bool> Alterar(Estudante estudante, Stream fotoPerfil)
        {
            try
            {
                if (fotoPerfil != null)
                {
                    //CADASTRA A IMAGEM DO PERFIL NO STORAGE
                    var url = await ConnectionDB.Storage.Child("FotoPerfil")
                        .Child(estudante.Codigo + ".jpg").PutAsync(fotoPerfil);
                    //PEGA O ENDEREÇO DA IMAGEM SALVA 
                    estudante.FotoPerfilUrl = url;
                }

                try
                {
                    if (fotoPerfil == null && string.IsNullOrEmpty(estudante.FotoPerfilUrl))
                        await ConnectionDB.Storage.Child("FotoPerfil").Child(estudante.Codigo + ".jpg").DeleteAsync();
                }
                catch { }

                //PUT(UPDATE) NAS INFORMAÇÕES
                await ConnectionDB.Database.Child("Estudante").Child(estudante.Codigo).PutAsync(estudante);
                App.EstudanteDados = estudante;

                return true;
            }
            catch (FirebaseException e)
            {
                throw new Exception(e.Message);
            }
        }

        public static async Task<Estudante> BuscarPeloCodigo(string codigo)
        {
            try
            {
                return await ConnectionDB.Database.Child("Estudante").Child(codigo)
                    .OnceSingleAsync<Estudante>();
            }
            catch (FirebaseException e)
            {
                throw new Exception(e.Message);
            }
        }

        public static async void Favorito()
        {
            await ConnectionDB.Database.Child("Estudante")
                .Child(App.EstudanteDados.Codigo).PutAsync(App.EstudanteDados);
        }
    }
}