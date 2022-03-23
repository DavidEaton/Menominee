namespace CustomerVehicleManagement.Shared.Models.RepairOrders.SerialNumbers
{
    public class RepairOrderSerialNumberToRead
    {
        public long Id { get; set; }
        public long RepairOrderItemId { get; set; }
        public string SerialNumber { get; set; }
    }
}
