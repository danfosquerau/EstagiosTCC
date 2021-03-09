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
    public static class EmpresaDao
    {
        public static async Task<bool> Inserir(Empresa empresa, Stream logoEmpresa)
        {
            try
            {
                if (logoEmpresa != null)
                {
                    //CADASTRA A IMAGEM DO PERFIL NO STORAGE
                    var url = await ConnectionDB.Storage.Child("LogoEmpresa")
                        .Child(empresa.Codigo + ".jpg").PutAsync(logoEmpresa);
                    //PEGA O ENDEREÇO DA IMAGEM SALVA 
                    empresa.LogoEmpresa = url;
                }

                //CADASTRA AS INFORMAÇÕES DO USUARIO NO BANCO
                await ConnectionDB.Database.Child("Empresa").Child(empresa.Codigo).PostAsync(empresa);
                await ConnectionDB.Database.Child("Empresa").Child(empresa.Codigo).PutAsync(empresa);
                App.EmpresaDados = empresa;

                return true;
            }
            catch (FirebaseException e)
            {
                throw new Exception(e.Message);
            }
        }

        public static async Task<bool> Alterar(Empresa empresa, Stream logoEmpresa)
        {
            try
            {
                if (logoEmpresa != null)
                {
                    //CADASTRA A IMAGEM DO PERFIL NO STORAGE
                    var url = await ConnectionDB.Storage.Child("LogoEmpresa")
                        .Child(empresa.Codigo + ".jpg").PutAsync(logoEmpresa);
                    //PEGA O ENDEREÇO DA IMAGEM SALVA 
                    empresa.LogoEmpresa = url;
                }

                try
                {
                    if (logoEmpresa == null && string.IsNullOrEmpty(empresa.LogoEmpresa))
                        await ConnectionDB.Storage.Child("LogoEmpresa").Child(empresa.Codigo + ".jpg").DeleteAsync();
                }
                catch { }

                //PUT(UPDATE) NAS INFORMAÇÕES
                await ConnectionDB.Database.Child("Empresa").Child(empresa.Codigo).PutAsync(empresa);
                App.EmpresaDados = empresa;

                return true;
            }
            catch (FirebaseException e)
            {
                throw new Exception(e.Message);
            }
        }

        public static async Task<Empresa> BuscarPeloCodigo(string codigo)
        {
            try
            {
                return await ConnectionDB.Database.Child("Empresa").Child(codigo)
                    .OnceSingleAsync<Empresa>();
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
                if (App.EmpresaDados.MeusEstagios == null)
                    App.EmpresaDados.MeusEstagios = new List<string>();

                App.EmpresaDados.MeusEstagios.Add(codigo);
                
                await ConnectionDB.Database.Child("Empresa")
                    .Child(App.EmpresaDados.Codigo).PutAsync(App.EmpresaDados);
            }
            catch (FirebaseException e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}