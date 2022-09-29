using Menominee.Common;
using Menominee.Common.Enums;

namespace CustomerVehicleManagement.Domain.Entities.Inventory
{
    public class InventoryItemInspection : Entity
    {
        public InventoryItem InventoryItem { get; private set; }
        public LaborAmount LaborAmount { get; set; }
        public TechAmount TechAmount { get; set; }
        public InventoryItemInspectionType Type { get; private set; }

        #region ORM

        // EF requires a parameterless constructor
        protected InventoryItemInspection() { }

        #endregion    
    }
}