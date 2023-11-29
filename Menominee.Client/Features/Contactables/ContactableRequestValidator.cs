using FluentValidation;
using Menominee.Domain.Interfaces;

namespace Menominee.Client.Features.Contactables
{
    public abstract class ContactableRequestValidator<T> : AbstractValidator<IList<T>> where T : IHasPrimary
    {
        protected void AddHasPrimaryCollectionRules()
        {
            ClassLevelCascadeMode = CascadeMode.Continue;

            RuleFor(items => items)
                .Must(items => items.All(item => item is not null))
                .WithMessage("list cannot contain null values")
                .Must(HaveOnlyOnePrimary)
                .WithMessage("Can have only one Primary.");
        }

        protected bool HaveOnlyOnePrimary(IList<T> items) =>
            items.Count(item =>
            item is not null && item.IsPrimary)
            <= 1;
    }
}
