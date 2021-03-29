using CustomerVehicleManagement.Domain.BaseClasses;
using System.ComponentModel.DataAnnotations;

namespace CustomerVehicleManagement.Api.ValidationAttributes
{
    public class ContactableCanHaveOnlyOnePrimaryEmailAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var contactable = (Contactable)validationContext.ObjectInstance;
            int primaryEmailCount = 0;

            foreach (var email in contactable.Emails)
            {
                if (email.IsPrimary)
                    primaryEmailCount += 1;
            }

            if (primaryEmailCount > 1)
            {
                return new ValidationResult(
                    "Can have only one Primary email.",
                    new[] { nameof(Contactable) });
            }

            return ValidationResult.Success;
        }
    }
}
