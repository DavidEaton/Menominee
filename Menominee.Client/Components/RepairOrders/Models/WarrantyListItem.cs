using CustomerVehicleManagement.Shared.Models.RepairOrders.Warranties;
using Menominee.Common.Enums;

namespace Menominee.Client.Components.RepairOrders.Models
{
    public class WarrantyListItem
    {
        //public long Id { get; set; }
        public long RepairOrderItemId { get; set; }
        public long SequenceNumber { get; set; }
        public WarrantyType Type { get; set; }
        public string PartNumber { get; set; }
        public string Description { get; set; }
        public string WarrantyNumber { get; set; }
        public double Quantity { get; set; }
        public RepairOrderWarrantyToWrite WarrantyType { get; set; }
        public bool IsComplete()
        {
            return (WarrantyNumber is null) ? false : WarrantyNumber.Length > 0;
        }
    }
}
