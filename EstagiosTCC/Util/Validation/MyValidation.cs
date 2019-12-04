namespace EstagiosTCC.Util.Validation
{
    public static class MyValidation
    {
        public static bool ValidarCep(string cep)
        {
            if (cep.Length != 8)
                return false;

            int NovoCEP = 0;
            if (!int.TryParse(cep, out NovoCEP))
                return false;

            return true;
        }
    }
}