using CSharpFunctionalExtensions;
using CustomerVehicleManagement.Shared.Models;
using FluentValidation;
using Menominee.Common.ValueObjects;
using System;
using System.Collections.Generic;
using AppValueObject = Menominee.Common.ValueObjects.AppValueObject;

namespace CustomerVehicleManagement.Api.Validators
{
    public static class ApplicationValidators
    {
        //public static IRuleBuilderOptionsConditions<T, IList<TElement>> ListMustContainOnlyOnePrimary<T, TElement>(
        //    this IRuleBuilder<T, IList<TElement>> ruleBuilder) where TElement : class
        //{
        //    return ruleBuilder.Custom((list, context) =>
        //    {
        //        int primaryCount = 0;
        //        object i;

        //        if (list[0] is PhoneToAdd)
        //            i = new PhoneToAdd();

        //        foreach (var item in list)
        //        {
        //            if (item is PhoneToAdd)
        //                i = item as PhoneToAdd;

        //            if ((PhoneToAdd)i.IsPrimary)
        //                primaryCount += 1;
        //        }

        //        if (primaryCount > 1)
        //        {
        //            context.AddFailure($"Can have only one Primary phone.");
        //        }
        //    });
        //}

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

        public static IRuleBuilderOptions<T, TValueObject> MustBeValueObject<T, TValueObject>(
            this IRuleBuilder<T, TValueObject> ruleBuilder,
            Func<TValueObject, Result<TValueObject>> factoryMethod)
            where TValueObject : AppValueObject
        {
            return (IRuleBuilderOptions<T, TValueObject>)(IRuleBuilderOptions<T, string>)ruleBuilder.Custom((value, context) =>
            {
                //if (string.IsNullOrWhiteSpace(value))
                //    return;

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
    }
}
