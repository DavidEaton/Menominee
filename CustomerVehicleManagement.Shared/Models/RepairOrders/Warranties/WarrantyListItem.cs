using Menominee.Common.Enums;

namespace CustomerVehicleManagement.Shared.Models.RepairOrders.Warranties
{
    public class WarrantyListItem
    {
        public long RepairOrderItemId { get; set; }
        public WarrantyType Type { get; set; }
        public string PartNumber { get; set; }
        public string Description { get; set; }

        public string WarrantyNumber
        {
            get =>
                !string.IsNullOrWhiteSpace(WarrantyType.OriginalWarranty)
                ? WarrantyType.OriginalWarranty
                : string.Empty;

            set => WarrantyType.OriginalWarranty = value;
        }

        public double Quantity
        {
            get => WarrantyType.Quantity;
            set => WarrantyType.Quantity = value;
        }

        public RepairOrderWarrantyToWrite WarrantyType { get; set; }
        public bool IsComplete()
        {
            return Quantity > 0;
        }
    }
}
