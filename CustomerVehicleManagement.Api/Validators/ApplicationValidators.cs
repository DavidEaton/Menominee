using CSharpFunctionalExtensions;
using FluentValidation;
using Menominee.Common.ValueObjects;
using System;
using AppValueObject = Menominee.Common.ValueObjects.AppValueObject;

namespace CustomerVehicleManagement.Api.Validators
{
    public static class ApplicationValidators
    {
        public static IRuleBuilderOptions<T, string> MustBeValueObject<T, TValueObject>(
            this IRuleBuilder<T, string> ruleBuilder,
            Func<string, Result<TValueObject>> factoryMethod)
            where TValueObject : AppValueObject
        {
            return (IRuleBuilderOptions<T, string>)ruleBuilder.Custom((value, context) =>
            {
                if (string.IsNullOrWhiteSpace(value))
                    return;

                Result<TValueObject> result = factoryMethod(value);

                if (result.IsFailure)
                {
                    context.AddFailure(result.Error);
                }
            });
        }

        public static IRuleBuilderOptions<T, Address> MustBeValueObject<T, TValueObject>(
            this IRuleBuilder<T, Address> ruleBuilder,
            Func<Address, Result<TValueObject>> factoryMethod)
            where TValueObject : AppValueObject
        {
            return (IRuleBuilderOptions<T, Address>)ruleBuilder.Custom((value, context) =>
            {
                if (value == null)
                    return;

                Result<TValueObject> result = factoryMethod(value);

                if (result.IsFailure)
                {
                    context.AddFailure(result.Error);
                }
            });
        }

        public static IRuleBuilderOptions<T, TElement> MustBeEntity<T, TElement, TValueObject>(
            this IRuleBuilder<T, TElement> ruleBuilder,
            Func<TElement, Result<TValueObject>> factoryMethod)
            where TValueObject : Menominee.Common.Entity
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
    }
}
