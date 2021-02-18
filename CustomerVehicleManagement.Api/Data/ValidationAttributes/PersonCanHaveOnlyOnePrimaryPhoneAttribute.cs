using CustomerVehicleManagement.Api.Data.Models;
using System.ComponentModel.DataAnnotations;

namespace CustomerVehicleManagement.Api.Data.ValidationAttributes
{
    public class PersonCanHaveOnlyOnePrimaryPhoneAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var person = (PersonCreateDto)validationContext.ObjectInstance;
            int primaryPhoneCount = 0;

            foreach (var phone in person.Phones)
            {
                if (phone.Primary)
                    primaryPhoneCount += 1;
            }

            if (primaryPhoneCount > 1)
            {
                return new ValidationResult(
                    "Person can have only one Primary phone.",
                    new[] { nameof(PersonCreateDto) });
            }

            return ValidationResult.Success;
        }
    }
}
