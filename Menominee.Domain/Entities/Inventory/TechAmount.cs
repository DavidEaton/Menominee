using CSharpFunctionalExtensions;
using Menominee.Domain.Enums;
using System;
using System.Collections.Generic;

namespace Menominee.Domain.Entities.Inventory
{
    public class TechAmount : LaborAmount
    {
        public SkillLevel SkillLevel { get; private set; }

        private TechAmount(ItemLaborType type, double amount, SkillLevel skillLevel)
            : base(type, amount)
        {
            SkillLevel = skillLevel;
        }

        public static Result<TechAmount> Create(ItemLaborType type, double amount, SkillLevel skillLevel)
        {
            if (!Enum.IsDefined(typeof(SkillLevel), skillLevel))
                return Result.Failure<TechAmount>(RequiredMessage);

            if (amount < MinimumAmount)
                return Result.Failure<TechAmount>(InvalidAmountMessage);

            return Result.Success(new TechAmount(type, amount, skillLevel));
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Type;
            yield return Amount;
            yield return SkillLevel;
        }

        #region ORM

        // EF requires a parameterless constructor
        protected TechAmount() { }

        #endregion
    }
}
