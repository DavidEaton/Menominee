using FluentValidation;
using Menominee.Domain.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace Menominee.Api.Features.Contactables
{
    public class ItemsWithPrimaryValidator<T, TValidator> : AbstractValidator<IList<T>>
        where T : IHasPrimary
        where TValidator : AbstractValidator<T>, new()
    {
        private const string onePrimaryMessage = "Can have only one Primary item.";

        public ItemsWithPrimaryValidator()
        {
            ClassLevelCascadeMode = CascadeMode.Continue;

            RuleFor(items => items)
                .Must(HaveOnlyOnePrimary)
                .WithMessage(onePrimaryMessage)
                .ForEach(itemRule =>
                {
                    itemRule.SetValidator(new TValidator());
                });
        }

        private static bool HaveOnlyOnePrimary(IList<T> items)
        {
            return items.Count(item => item is not null && item.IsPrimary) <= 1;
        }
    }
}
