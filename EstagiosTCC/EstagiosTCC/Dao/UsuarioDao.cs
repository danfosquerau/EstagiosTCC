using EstagiosTCC.Model;
using EstagiosTCC.Util;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Database.Query;
using System;
using System.Threading.Tasks;

namespace EstagiosTCC.Dao
{
    public static class UsuarioDao
    {
        public static async Task<bool> Inserir(string email, string senha)
        {
            try
            {
                //CADASTRA AS INFORMAÇÕES INICIAIS DO USUARIO PARA AUTENTICAÇÃO
                App.UsuarioLogadoAuth = await ConnectionDB.Authentication
                    .CreateUserWithEmailAndPasswordAsync(email, senha);

                if (App.UsuarioLogadoAuth == null)
                    return false;

                Usuario usuario = new Usuario
                {
                    //PEGA O ID DO REGISTRO DO FIREBASE DO USUARIO
                    Codigo = App.UsuarioLogadoAuth.User.LocalId
                };

                ConnectionDB.InitializeData(App.UsuarioLogadoAuth.FirebaseToken);

                //CADASTRA AS INFORMAÇÕES DO USUARIO NO BANCO
                await ConnectionDB.Database.Child("Usuario").Child(usuario.Codigo).PostAsync(usuario);
                await ConnectionDB.Database.Child("Usuario").Child(usuario.Codigo).PutAsync(usuario);
                App.UsuarioLogado = usuario;

                return true;
            }
            catch (FirebaseException e)
            {
                throw new Exception(e.Message);
            }
        }

        public static async Task<bool> Alterar(Usuario usuario)
        {
            try
            {
                //PUT(UPDATE) NAS INFORMAÇÕES
                await ConnectionDB.Database.Child("Usuario").Child(usuario.Codigo).PutAsync(usuario);
                App.UsuarioLogado = usuario;

                return true;
            }
            catch (FirebaseException e)
            {
                throw new Exception(e.Message);
            }
        }

        public static async Task<bool> EntrarComEmailESenha(string email, string senha)
        {
            try
            {
                App.UsuarioLogadoAuth = await ConnectionDB.Authentication
                    .SignInWithEmailAndPasswordAsync(email, senha);

                if (App.UsuarioLogadoAuth == null)
                    return false;

                ConnectionDB.InitializeData(App.UsuarioLogadoAuth.FirebaseToken);

                App.UsuarioLogado = await BuscarPeloCodigo(App.UsuarioLogadoAuth.User.LocalId);

                return true;
            }
            catch (FirebaseException e)
            {
                throw new Exception(e.Message);
            }
        }

        public static async Task<bool> EntrarComOAuth(FirebaseAuthType authType, string accessToken)
        {
            try
            {
                App.UsuarioLogadoAuth = await ConnectionDB.Authentication
                    .SignInWithOAuthAsync(authType, accessToken);

                if (App.UsuarioLogadoAuth == null)
                    return false;

                ConnectionDB.InitializeData(App.UsuarioLogadoAuth.FirebaseToken);

                App.UsuarioLogado = await BuscarPeloCodigo(App.UsuarioLogadoAuth.User.LocalId);

                if (App.UsuarioLogado == null)
                {
                    Usuario usuario = new Usuario()
                    {
                        Codigo = App.UsuarioLogadoAuth.User.LocalId,
                    };
                    //CADASTRA AS INFORMAÇÕES DO USUARIO NO BANCO
                    await ConnectionDB.Database.Child("Usuario").Child(usuario.Codigo).PostAsync(usuario);
                    await ConnectionDB.Database.Child("Usuario").Child(usuario.Codigo).PutAsync(usuario);

                    App.UsuarioLogado = await BuscarPeloCodigo(App.UsuarioLogadoAuth.User.LocalId);
                }

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
    }
}