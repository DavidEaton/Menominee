using CustomerVehicleManagement.Domain.Entities;
using Menominee.Common.Enums;
using MenomineePlayWASM.Shared.Entities.Inventory.Enums;

namespace CustomerVehicleManagement.Shared.Models.Inventory
{
    public class InventoryItemToWrite
    {
        public long Id { get; set; }
        public virtual Manufacturer Manufacturer { get; set; }
        public long ManufacturerId { get; set; }
        public string PartNumber { get; set; }
        public string Description { get; set; }
        public virtual ProductCode ProductCode { get; set; }
        public long ProductCodeId { get; set; }
        public PartType PartType { get; set; }      // FIX ME - PartType vs InventoryItemType
        public int QuantityOnHand { get; set; }
        public double Cost { get; set; }
        public double SuggestedPrice { get; set; }
        public double Labor { get; set; }
    }
}
