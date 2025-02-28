﻿using EstagiosTCC.Util.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EstagiosTCC.Model
{
    public class Empresa
    {
        public string Codigo { get; set; }
        public string Email { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Campo Obrigatório.")]
        [CNPJ(ErrorMessage ="CNPJ inválido.")]
        public string Cnpj { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Campo Obrigatório.")]
        public string AtividadeEconomica { get; set; }
        public DateTime DataAbertura { get; set; }
        public Endereco Endereco { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Campo Obrigatório.")]
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
            Endereco = new Endereco();
            NomeEmpresa = string.Empty;
            LogoEmpresa = string.Empty;
            MeusEstagios = new List<string>() { };
        }
    }
}