using Menominee.Common.Enums;
using System.Collections.Generic;

namespace CustomerVehicleManagement.Domain.Entities.Inventory
{
    public class TechAmount : LaborAmount
    {
        public SkillLevel SkillLevel { get; private set; }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return PayType;
            yield return Amount;
            yield return SkillLevel;
        }
    }
}
