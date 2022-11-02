using CSharpFunctionalExtensions;
using CustomerVehicleManagement.Domain.Interfaces;
using FluentValidation;
using Menominee.Common.ValueObjects;
using System;
using System.Collections.Generic;

namespace CustomerVehicleManagement.Shared
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

                Result<TValueObject> result = factoryMethod(value);

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
                Result<TEntity> result = factoryMethod(value);

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
                    context.AddFailure(
                        context.PropertyName,
                        $"Only one Primary item allowed in list");

            });
        }

        private static bool HasOnlyOnePrimary(IList<IHasPrimary> items)
        {
            int primaryCount = 0;

            foreach (var item in items)
            {
                if (item.IsPrimary)
                    primaryCount += 1;
            }

            if (primaryCount > 1)
            {
                return false;
            }

            return true;
        }
    }

}
