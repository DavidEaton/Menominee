using CSharpFunctionalExtensions;
using FluentValidation;
using System;
using AppValueObject = Menominee.Common.ValueObjects.ValueObject;

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
    }
}
