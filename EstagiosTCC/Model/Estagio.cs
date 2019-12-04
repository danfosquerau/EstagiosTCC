using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EstagiosTCC.Model
{
    public class Estagio
    {
        public string Codigo { get; set; }
        public Status Status { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Campo Obrigatório.")]
        public List<string> CodigosCursos { get; set; }
        public string AnexoUrl { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Campo Obrigatório.")]
        public string Empresa { get; set; }
        public string LogoEmpresa { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Campo Obrigatório.")]
        public string Setor { get; set; }
        public Endereco Endereco { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Campo Obrigatório.")]
        public string DescricaoDasAtividades { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Campo Obrigatório.")]
        public string Horario { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Campo Obrigatório.")]
        public string Pagamento { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Campo Obrigatório.")]
        public string OutrosAuxilios { get; set; }
        public string Contato { get; set; }
        public string LinkParaInformacoes { get; set; }
        public DateTime UltimaAtualizacao { get; set; }

        public Estagio()
        {
            Codigo = string.Empty;
            Status = Status.Disponivel;
            CodigosCursos = new List<string>();
            AnexoUrl = string.Empty;
            Empresa = string.Empty;
            LogoEmpresa = string.Empty;
            Setor = string.Empty;
            Endereco = new Endereco();
            DescricaoDasAtividades = string.Empty;
            Horario = string.Empty;
            Pagamento = string.Empty;
            OutrosAuxilios = string.Empty;
            Contato = string.Empty;
            LinkParaInformacoes = string.Empty;
            UltimaAtualizacao = DateTime.Now;
        }
    }

    public enum Status
    {
        Disponivel,
        Ocupado,
        Desativado
    }

    public class StatusEstagio
    {
        public Status Id { get; set; }
        public string Nome { get; set; }
    }
}