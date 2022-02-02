namespace CustomerVehicleManagement.Shared.Models.RepairOrders.Techs
{
    public class RepairOrderTechToWrite
    {
        public long Id { get; set; } = 0;
        public long RepairOrderServiceId { get; set; } = 0;
        public long TechnicianId { get; set; } = 0;
    }
}
