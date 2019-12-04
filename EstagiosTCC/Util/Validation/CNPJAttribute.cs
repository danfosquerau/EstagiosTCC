using System.ComponentModel.DataAnnotations;

namespace EstagiosTCC.Util.Validation
{
    class CNPJAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            string cnpj = value as string;

            if (MyValidation.IsCnpj(cnpj))
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
