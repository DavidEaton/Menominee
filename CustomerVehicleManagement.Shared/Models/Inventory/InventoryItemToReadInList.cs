﻿using CustomerVehicleManagement.Domain.Entities.Inventory;
using Menominee.Common.Enums;

namespace CustomerVehicleManagement.Shared.Models.Inventory
{
    public class InventoryItemToReadInList
    {
        public long Id { get; set; }
        public virtual Manufacturer Manufacturer { get; set; }
        public long ManufacturerId { get; set; }
        public string ManufacturerName { get; set; }
        public string ItemNumber { get; set; }
        public string Description { get; set; }
        public virtual ProductCode ProductCode { get; set; }
        public long ProductCodeId { get; set; }
        public string ProductCodeName { get; set; }
        public InventoryItemType ItemType { get; set; }
        public long DetailId { get; set; }

        //public int QuantityOnHand { get; set; }
        //public double Cost { get; set; }
        //public double SuggestedPrice { get; set; }
        //public double Labor { get; set; }
    }
}
