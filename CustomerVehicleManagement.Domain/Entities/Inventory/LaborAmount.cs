using Menominee.Common.Enums;
using Menominee.Common.ValueObjects;
using System.Collections.Generic;

namespace CustomerVehicleManagement.Domain.Entities.Inventory
{
    public class LaborAmount : AppValueObject
    {
        public ItemLaborType PayType { get; private set; }
        public double Amount { get; private set; }

        #region ORM

        // EF requires a parameterless constructor
        protected LaborAmount() { }

        #endregion    

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return PayType;
            yield return Amount;
        }
    }
}
