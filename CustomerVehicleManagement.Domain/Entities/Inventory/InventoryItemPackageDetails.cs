using Menominee.Common.ValueObjects;
using System;
using System.Collections.Generic;

namespace CustomerVehicleManagement.Domain.Entities.Inventory
{
    public class InventoryItemPackageDetails : AppValueObject
    {
        public double Quantity { get; private set; }
        public bool PartAmountIsAdditional { get; private set; }
        public bool LaborAmountIsAdditional { get; private set; }
        public bool ExciseFeeIsAdditional { get; private set; }
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Quantity;
            yield return PartAmountIsAdditional;
            yield return LaborAmountIsAdditional;
            yield return ExciseFeeIsAdditional;
        }

        #region ORM

        // EF requires a parameterless constructor
        protected InventoryItemPackageDetails() { }

        #endregion    

    }
}
