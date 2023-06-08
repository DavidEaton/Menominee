using CSharpFunctionalExtensions;
using Entity = Menominee.Common.Entity;

namespace CustomerVehicleManagement.Domain.Entities.Inventory
{
    public class InventoryItemWarranty : Entity
    {
        public static readonly string RequiredMessage = $"Please include all required items.";
        public InventoryItemWarrantyPeriod WarrantyPeriod { get; private set; }

        private InventoryItemWarranty(InventoryItemWarrantyPeriod warrantyPeriod)
        {
            WarrantyPeriod = warrantyPeriod;
        }

        public static Result<InventoryItemWarranty> Create(InventoryItemWarrantyPeriod warrantyPeriod)
        {
            if (warrantyPeriod is null)
                return Result.Failure<InventoryItemWarranty>(RequiredMessage);

            return Result.Success(new InventoryItemWarranty(warrantyPeriod));
        }

        public Result<InventoryItemWarrantyPeriod> SetWarrantyPeriod(InventoryItemWarrantyPeriod warrantyPeriod)
        {
            if (warrantyPeriod is null)
                return Result.Failure<InventoryItemWarrantyPeriod>(RequiredMessage);

            return Result.Success(WarrantyPeriod = warrantyPeriod);
        }
        #region ORM

        // EF requires a parameterless constructor
        protected InventoryItemWarranty() { }

        #endregion  
    }
}
