namespace EstagiosTCC.Model
{
    public class Usuario
    {
        public string Codigo { get; set; }
        public Tipo Tipo { get; set; }

        public Usuario()
        {
            Codigo = string.Empty;
            Tipo = Tipo.NaoDefinido;
        }
    }

    public enum Tipo
    {
        NaoDefinido,
        Estudante,
        Empresa
    }
}