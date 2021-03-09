using System.ComponentModel.DataAnnotations;

namespace EstagiosTCC.Util.Validation
{
    public class CEPAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            string cep = value as string;

            if (MyValidation.ValidarCep(cep))
            {
                return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult(ErrorMessage);
            }
        }
    }
}