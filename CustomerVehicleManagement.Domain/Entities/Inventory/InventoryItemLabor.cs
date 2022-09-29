using CSharpFunctionalExtensions;
using Entity = Menominee.Common.Entity;

namespace CustomerVehicleManagement.Domain.Entities.Inventory
{
    public class InventoryItemLabor : Entity
    {
        public InventoryItem InventoryItem { get; private set; }
        public LaborAmount LaborAmount { get; set; }
        public TechAmount TechAmount { get; set; }

        private InventoryItemLabor(InventoryItem inventoryItem, LaborAmount laborAmount, TechAmount techAmount)
        {
            InventoryItem = inventoryItem;
            LaborAmount = laborAmount;
            TechAmount = techAmount;
        }

        public Result<InventoryItemLabor> Create(InventoryItem inventoryItem, LaborAmount laborAmount, TechAmount techAmount)
        {


            return Result.Success(new InventoryItemLabor(inventoryItem, laborAmount, techAmount));
        }

        #region ORM

        // EF requires a parameterless constructor
        protected InventoryItemLabor() { }

        #endregion
    }
}
