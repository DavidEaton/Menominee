using Menominee.Common.Enums;
using Menominee.Common.ValueObjects;
using System.Collections.Generic;

namespace CustomerVehicleManagement.Domain.Entities.Inventory
{
    public class InventoryItemWarrantyPeriod : AppValueObject
    {
        public InventoryItemWarrantyPeriodType PeriodType { get; private set; }
        public int Duration { get; private set; }

         protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return PeriodType;
            yield return Duration;
        }

       #region ORM

        // EF requires a parameterless constructor
        protected InventoryItemWarrantyPeriod() { }

        #endregion    
    }
}
