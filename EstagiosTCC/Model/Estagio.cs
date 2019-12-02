using System;
using System.Collections.Generic;

namespace EstagiosTCC.Model
{
    public class Estagio
    {
        public string Codigo { get; set; }
        public string Status { get; set; }
        public List<string> CodigosCursos { get; set; }
        public string AnexoUrl { get; set; }
        public string Empresa { get; set; }
        public string LogoEmpresa { get; set; }
        public string Setor { get; set; }
        public string Endereco { get; set; }
        public string DescricaoDasAtividades { get; set; }
        public string Horario { get; set; }
        public string Pagamento { get; set; }
        public string OutrosAuxilios { get; set; }
        public string Contato { get; set; }
        public string LinkParaInformacoes { get; set; }
        public DateTime UltimaAtualizacao { get; set; }

        public Estagio()
        {
            Codigo = string.Empty;
            Status = string.Empty;
            CodigosCursos = new List<string>();
            AnexoUrl = string.Empty;
            Empresa = string.Empty;
            LogoEmpresa = string.Empty;
            Setor = string.Empty;
            Endereco = string.Empty;
            DescricaoDasAtividades = string.Empty;
            Horario = string.Empty;
            Pagamento = string.Empty;
            OutrosAuxilios = string.Empty;
            Contato = string.Empty;
            LinkParaInformacoes = string.Empty;
            UltimaAtualizacao = DateTime.Now;
        }
    }
}