﻿using Menominee.Common;

namespace CustomerVehicleManagement.Domain.Entities.Inventory
{
    // TODO: No detail for this entitiy.  Can we avoid using this class?
    public class InventoryItemDonation : Entity
    {
        public long InventoryItemId { get; set; }

        #region ORM

        // EF requires an empty constructor
        public InventoryItemDonation() { }

        #endregion  
    }
}
