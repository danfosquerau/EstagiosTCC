using System.Collections.Generic;

namespace EstagiosTCC.Model
{
    public class Usuario
    {
        public string Codigo { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }
        public string FotoPerfilUrl { get; set; }
        public List<string> MeusEstagios { get; set; }

        public Usuario()
        {
            Codigo = string.Empty;
            Nome = string.Empty;
            Email = string.Empty;
            Senha = string.Empty;
            FotoPerfilUrl = string.Empty;
            MeusEstagios = new List<string>();
        }
    }
}