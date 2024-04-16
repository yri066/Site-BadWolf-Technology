using BadWolfTechnology.Data.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace BadWolfTechnology.Data.DataAnnotations
{
    public class MinimumAgeAttribute : ValidationAttribute
    {
        public int MinimumAge { get; }
        public MinimumAgeAttribute(int minimumAge)
        {
            MinimumAge = minimumAge;

            string defaultErrorMessage = $"Возраст должен быть не менее {minimumAge} лет.";
            ErrorMessage ??= defaultErrorMessage;
        }

        public string GetErrorMessage() => ErrorMessage;

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null || value is not DateTime)
            {
                return new ValidationResult("Это поле обязательно.");
            }

            var service = validationContext.GetService(typeof(IDateTime)) as IDateTime;

            var today = service?.Today ?? DateTime.Today;
            var birthDate = Convert.ToDateTime(value);


            int age = today.Year - birthDate.Year;

            if (birthDate.AddYears(age) > today)
            {
                age--;
            }

            if (age < MinimumAge)
            {
            
                return new ValidationResult(GetErrorMessage());
            }

            return ValidationResult.Success;
        }
    }
}
