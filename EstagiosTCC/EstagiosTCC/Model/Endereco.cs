using EstagiosTCC.Util.Validation;
using System.ComponentModel.DataAnnotations;

namespace EstagiosTCC.Model
{
    public class Endereco
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Campo Obrigatório.")]
        [CEP(ErrorMessage ="CEP inválido.")]
        public string Cep { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Campo Obrigatório.")]
        public string Logradouro { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Campo Obrigatório.")]
        public string Bairro { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Campo Obrigatório.")]
        public string Localidade { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Campo Obrigatório.")]
        [StringLength(2,ErrorMessage ="Informe a sigla",MinimumLength =2)]
        public string Uf { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Campo Obrigatório.")]
        public string Unidade { get; set; }

        public Endereco()
        {
            Cep = string.Empty;
            Logradouro = string.Empty;
            Bairro = string.Empty;
            Localidade = string.Empty;
            Uf = string.Empty;
            Unidade = string.Empty;
        }
    }
}