using Menominee.Common;
using Menominee.Common.Enums;
using Menominee.Common.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerVehicleManagement.Domain.Entities.Inventory
{
    public class InventoryItem : Entity
    {
        public virtual Manufacturer Manufacturer { get; set; }
        public long ManufacturerId { get; set; }
        public string ItemNumber { get; set; }
        public string Description { get; set; }
        public virtual ProductCode ProductCode { get; set; }
        public long ProductCodeId { get; set; }
        public InventoryItemType ItemType { get; set; }
        public long DetailId { get; set; }

        public InventoryPart Part { get; private set; }
        public InventoryLabor Labor { get; private set; }
        public InventoryTire Tire { get; private set; }

        public InventoryItem(InventoryPart part)
        {
            Guard.ForNull(part, "part == null");

            Part = part;
            ItemType = InventoryItemType.Part;
        }

        public InventoryItem(InventoryLabor labor)
        {
            Guard.ForNull(labor, "labor == null");

            Labor = labor;
            ItemType = InventoryItemType.Labor;
        }

        public InventoryItem(InventoryTire tire)
        {
            Guard.ForNull(tire, "tire == null");

            Tire = tire;
            ItemType = InventoryItemType.Tire;
        }


        #region ORM

        // EF requires an empty constructor
        public InventoryItem() { }

        #endregion
    }
}
