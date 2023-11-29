using FluentValidation;
using Menominee.Shared.Models.Contactable;

namespace Menominee.Client.Features.Contactables.Emails
{
    public class EmailsRequestValidator : ContactableRequestValidator<EmailToWrite>
    {
        public EmailsRequestValidator()
        {
            ClassLevelCascadeMode = CascadeMode.Continue;

            AddHasPrimaryCollectionRules();

            RuleForEach(emails => emails)
                .SetValidator(new EmailRequestValidator());
        }
    }
}