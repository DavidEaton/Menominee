using CSharpFunctionalExtensions;
using Menominee.Common.Enums;
using System;
using System.Collections.Generic;

namespace CustomerVehicleManagement.Domain.Entities.Inventory
{
    public class TechAmount : LaborAmount
    {
        public SkillLevel SkillLevel { get; private set; }

        private TechAmount(ItemLaborType payType, double amount, SkillLevel skillLevel)
            : base(payType, amount)
        {
            if (!Enum.IsDefined(typeof(SkillLevel), skillLevel))
                throw new ArgumentOutOfRangeException(RequiredMessage); 

            SkillLevel = skillLevel;
        }

        public static Result<TechAmount> Create(ItemLaborType payType, double amount, SkillLevel skillLevel)
        {
            if (!Enum.IsDefined(typeof(ItemLaborType), payType))
                return Result.Failure<TechAmount>(RequiredMessage);

            if (!Enum.IsDefined(typeof(SkillLevel), skillLevel))
                return Result.Failure<TechAmount>(RequiredMessage);


            return Result.Success(new TechAmount(payType, amount, skillLevel));
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return PayType;
            yield return Amount;
            yield return SkillLevel;
        }

        #region ORM

        // EF requires a parameterless constructor
        protected TechAmount() { }

        #endregion
    }
}
