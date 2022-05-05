using Menominee.Common;
using Menominee.Common.Enums;

namespace CustomerVehicleManagement.Domain.Entities.Inventory
{
    public class InventoryItemPart : InstallablePart
    {

        #region ORM

        // EF requires an empty constructor
        public InventoryItemPart() { }

        #endregion
    }
}
