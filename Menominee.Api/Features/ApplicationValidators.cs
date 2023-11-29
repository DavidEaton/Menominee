using CSharpFunctionalExtensions;
using FluentValidation;
using Menominee.Common.ValueObjects;
using Menominee.Domain.Interfaces;
using System;
using System.Collections.Generic;

namespace Menominee.Api.Features
{
    public static class ApplicationValidators
    {
        public static IRuleBuilderOptions<T, TElement> MustBeValueObject<T, TValueObject, TElement>(
            this IRuleBuilder<T, TElement> ruleBuilder,
            Func<TElement, Result<TValueObject>> factoryMethod)
            where TValueObject : AppValueObject
        {
            return (IRuleBuilderOptions<T, TElement>)ruleBuilder.Custom((value, context) =>
            {

                var result = factoryMethod(value);

                if (result.IsFailure)
                {
                    context.AddFailure(result.Error);
                }
            });
        }

        public static IRuleBuilderOptions<T, TElement> MustBeEntity<T, TElement, TEntity>(
            this IRuleBuilder<T, TElement> ruleBuilder,
            Func<TElement, Result<TEntity>> factoryMethod)
            where TEntity : Menominee.Common.Entity
        {
            return (IRuleBuilderOptions<T, TElement>)ruleBuilder.Custom((value, context) =>
            {
                var result = factoryMethod(value);

                if (result.IsFailure)
                {
                    context.AddFailure(result.Error);
                }
            });
        }

        public static IRuleBuilderOptionsConditions<T, IList<IHasPrimary>> ListHasNoMoreThanOnePrimary<T, TElement>(
            this IRuleBuilder<T, IList<IHasPrimary>> ruleBuilder)
        {
            return ruleBuilder.Custom((list, context) =>
            {
                if (HasOnlyOnePrimary(list))
                {
                    context.AddFailure(
                        context.PropertyName,
                        $"Only one Primary item allowed in list");
                }
            });
        }

        private static bool HasOnlyOnePrimary(IList<IHasPrimary> items)
        {
            var primaryCount = 0;

            foreach (var item in items)
            {
                if (item.IsPrimary)
                {
                    primaryCount += 1;
                }
            }

            return primaryCount <= 1;
        }
    }
}
