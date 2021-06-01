using CustomerVehicleManagement.Domain.BaseClasses;
using CustomerVehicleManagement.Domain.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace CustomerVehicleManagement.Api.ValidationAttributes
{
    public class ContactableCanHaveOnlyOnePrimaryPhoneAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var contactable = (IContactLists)validationContext.ObjectInstance;
            int primaryPhoneCount = 0;

            foreach (var phone in contactable.Phones)
            {
                if (phone.IsPrimary)
                    primaryPhoneCount += 1;
            }

            if (primaryPhoneCount > 1)
            {
                return new ValidationResult(
                    "Can have only one Primary phone.",
                    new[] { nameof(Contactable) });
            }

            return ValidationResult.Success;
        }
    }
}
