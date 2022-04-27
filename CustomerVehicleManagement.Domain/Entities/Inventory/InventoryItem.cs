﻿using Menominee.Common;
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

        public InventoryItemPart Part { get; private set; }
        public InventoryItemLabor Labor { get; private set; }
        public InventoryItemTire Tire { get; private set; }

        public InventoryItem(InventoryItemPart part)
        {
            Guard.ForNull(part, "part == null");

            Part = part;
            ItemType = InventoryItemType.Part;
        }

        public InventoryItem(InventoryItemLabor labor)
        {
            Guard.ForNull(labor, "labor == null");

            Labor = labor;
            ItemType = InventoryItemType.Labor;
        }

        public InventoryItem(InventoryItemTire tire)
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