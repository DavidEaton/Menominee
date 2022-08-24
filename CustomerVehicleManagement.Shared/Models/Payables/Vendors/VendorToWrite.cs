namespace CustomerVehicleManagement.Shared.Models.Payables.Vendors
{
    public class VendorToWrite
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string VendorCode { get; set; } = string.Empty;
        public bool? IsActive { get; set; }
    }
}
