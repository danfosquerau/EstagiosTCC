using System;
using System.Collections.Generic;

namespace EstagiosTCC.Model
{
    public class Empresa
    {
        public string Codigo { get; set; }
        public string Email { get; set; }
        public string Cnpj { get; set; }
        public string AtividadeEconomica { get; set; }
        public DateTime DataAbertura { get; set; }
        public string Endereco { get; set; }
        public string NomeEmpresa { get; set; }
        public string LogoEmpresa { get; set; }
        public List<string> MeusEstagios { get; set; }

        public Empresa()
        {
            Codigo = string.Empty;
            Email = string.Empty;
            Cnpj = string.Empty;
            AtividadeEconomica = string.Empty;
            DataAbertura = DateTime.Now;
            Endereco = string.Empty;
            NomeEmpresa = string.Empty;
            LogoEmpresa = string.Empty;
            MeusEstagios = new List<string>() { };
        }
    }
}