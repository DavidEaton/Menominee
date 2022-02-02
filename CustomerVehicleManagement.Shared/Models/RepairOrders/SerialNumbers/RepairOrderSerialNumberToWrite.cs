namespace CustomerVehicleManagement.Shared.Models.RepairOrders.SerialNumbers
{
    public class RepairOrderSerialNumberToWrite
    {
        public long Id { get; set; } = 0;
        public long RepairOrderItemId { get; set; } = 0;
        public string SerialNumber { get; set; } = string.Empty;
    }
}
