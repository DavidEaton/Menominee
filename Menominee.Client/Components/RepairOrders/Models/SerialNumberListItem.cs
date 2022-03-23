using CustomerVehicleManagement.Shared.Models.RepairOrders.SerialNumbers;

namespace Menominee.Client.Components.RepairOrders.Models
{
    public class SerialNumberListItem
    {
        public long RepairOrderItemId { get; set; }
        public long ItemId { get; set; }
        public string PartNumber { get; set; }
        public string Description { get; set; }
        public RepairOrderSerialNumberToWrite SerialNumberType { get; set; }

        public string SerialNumber
        {
            get =>
                !string.IsNullOrWhiteSpace(SerialNumberType.SerialNumber)
                ? SerialNumberType.SerialNumber
                : string.Empty;

            set => SerialNumberType.SerialNumber = value;
        }

        public bool IsComplete()
        {
            return !string.IsNullOrWhiteSpace(SerialNumberType.SerialNumber);
        }
    }
}
