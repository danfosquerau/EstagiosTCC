using EstagiosTCC.Model;
using EstagiosTCC.Util;
using Firebase.Database;
using Firebase.Database.Query;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace EstagiosTCC.Dao
{
    public static class UsuarioDao
    {
        public static async Task<bool> Inserir(Usuario usuario, Stream fotoPerfil)
        {
            try
            {
                //CADASTRA AS INFORMAÇÕES INICIAIS DO USUARIO PARA AUTENTICAÇÃO
                App.UsuarioLogadoAuth = await ConnectionDB.Authentication
                    .CreateUserWithEmailAndPasswordAsync(usuario.Email, usuario.Senha);

                if (App.UsuarioLogadoAuth == null)
                    return false;

                //PEGA O ID DO REGISTRO DO FIREBASE DO USUARIO
                usuario.Codigo = App.UsuarioLogadoAuth.User.LocalId;
                ConnectionDB.InitializeData(App.UsuarioLogadoAuth.FirebaseToken);
                
                if (fotoPerfil != null)
                {
                    //CADASTRA A IMAGEM DO PERFIL NO STORAGE
                    var url = await ConnectionDB.Storage.Child("FotoPerfil")
                        .Child(usuario.Codigo + ".jpg").PutAsync(fotoPerfil);
                    //PEGA O ENDEREÇO DA IMAGEM SALVA 
                    usuario.FotoPerfilUrl = url;
                }

                usuario.Senha = string.Empty;
                //CADASTRA AS INFORMAÇÕES DO USUARIO NO BANCO
                await ConnectionDB.Database.Child("Usuario").Child(usuario.Codigo).PostAsync(usuario);
                await ConnectionDB.Database.Child("Usuario").Child(usuario.Codigo).PutAsync(usuario);
                App.UsuarioLogadoDados = usuario;

                return true;
            }
            catch (FirebaseException e)
            {
                throw new Exception(e.Message);
            }
        }

        public static async Task<bool> Alterar(Usuario usuario, Stream fotoPerfil)
        {
            try
            {
                if (fotoPerfil != null)
                {
                    //CADASTRA A IMAGEM DO PERFIL NO STORAGE
                    var url = await ConnectionDB.Storage.Child("FotoPerfil")
                        .Child(usuario.Codigo + ".jpg").PutAsync(fotoPerfil);
                    //PEGA O ENDEREÇO DA IMAGEM SALVA 
                    usuario.FotoPerfilUrl = url;
                }

                try
                {
                    if (fotoPerfil == null && string.IsNullOrEmpty(usuario.FotoPerfilUrl))
                        await ConnectionDB.Storage.Child("FotoPerfil").Child(usuario.Codigo + ".jpg").DeleteAsync();
                }
                catch { }

                //PUT(UPDATE) NAS INFORMAÇÕES
                usuario.Senha = string.Empty;
                await ConnectionDB.Database.Child("Usuario").Child(usuario.Codigo).PutAsync(usuario);
                App.UsuarioLogadoDados = usuario;
                
                return true;
            }
            catch (FirebaseException e)
            {
                throw new Exception(e.Message);
            }
        }

        public static async Task<bool> EntrarComEmailESenha(string email, string password)
        {
            try
            {
                App.UsuarioLogadoAuth = await ConnectionDB.Authentication
                    .SignInWithEmailAndPasswordAsync(email, password);

                if (App.UsuarioLogadoAuth == null)
                    return false;
                
                ConnectionDB.InitializeData(App.UsuarioLogadoAuth.FirebaseToken);
                App.UsuarioLogadoDados = await BuscarPeloCodigo(App.UsuarioLogadoAuth.User.LocalId);
                
                return true;
            }
            catch (FirebaseException e)
            {
                throw new Exception(e.Message);
            }
        }

        public static async Task<Usuario> BuscarPeloCodigo(string codigo)
        {
            try
            {
                return await ConnectionDB.Database.Child("Usuario").Child(codigo)
                    .OnceSingleAsync<Usuario>();
            }
            catch (FirebaseException e)
            {
                throw new Exception(e.Message);
            }
        }
    
        public static async void NovoEstagio(string codigo)
        {
            try
            {
                if (App.UsuarioLogadoDados.MeusEstagios == null)
                    App.UsuarioLogadoDados.MeusEstagios = new List<string>();

                App.UsuarioLogadoDados.MeusEstagios.Add(codigo);
                await ConnectionDB.Database.Child("Usuario")
                    .Child(App.UsuarioLogadoDados.Codigo).PutAsync(App.UsuarioLogadoDados);
            }
            catch (FirebaseException e)
            {
                throw new Exception(e.Message);
            }
        }
    
        public static async void Favorito()
        {
            await ConnectionDB.Database.Child("Usuario")
                .Child(App.UsuarioLogadoDados.Codigo).PutAsync(App.UsuarioLogadoDados);
        }
    }
}