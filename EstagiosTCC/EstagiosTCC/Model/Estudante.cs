using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EstagiosTCC.Model
{
    public class Estudante
    {
        public string Codigo { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Campo Obrigatório.")]
        public string Nome { get; set; }
        public string Email { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Campo Obrigatório.")]
        public string Curso { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Campo Obrigatório.")]
        public string InstituicaoEnsino { get; set; }
        public string FotoPerfilUrl { get; set; }
        public List<string> Favoritos { get; set; }

        public Estudante()
        {
            Codigo = string.Empty;
            Nome = string.Empty;
            Email = string.Empty;
            Curso = string.Empty;
            InstituicaoEnsino = string.Empty;
            FotoPerfilUrl = string.Empty;
            Favoritos = new List<string>();
        }
    }
}