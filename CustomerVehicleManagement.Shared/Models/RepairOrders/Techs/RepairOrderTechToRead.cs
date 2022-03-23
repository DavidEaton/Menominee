namespace CustomerVehicleManagement.Shared.Models.RepairOrders.Techs
{
    public class RepairOrderTechToRead
    {
        public long Id { get; set; }
        public long RepairOrderServiceId { get; set; }
        public long TechnicianId { get; set; }
    }
}
