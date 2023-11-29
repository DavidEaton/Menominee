using FluentValidation;
using Menominee.Shared.Models.Contactable;

namespace Menominee.Client.Features.Contactables.Phones
{
    public class PhonesRequestValidator : ContactableRequestValidator<PhoneToWrite>
    {
        public PhonesRequestValidator()
        {
            ClassLevelCascadeMode = CascadeMode.Continue;

            AddHasPrimaryCollectionRules();

            RuleForEach(phones => phones)
                .SetValidator(new PhoneRequestValidator());
        }
    }
}
