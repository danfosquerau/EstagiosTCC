namespace EstagiosTCC.Model
{
    public class Curso
    {
        public string Codigo { get; set; }
        public string Nome { get; set; }

        public Curso()
        {
            Codigo = string.Empty;
            Nome = string.Empty;
        }
    }
}