using CustomerVehicleManagement.Domain.Entities.Payables;

namespace CustomerVehicleManagement.Shared.Models.Payables.Vendors
{
    public class VendorToRead
    {
        public long Id { get; set; }
        public string VendorCode { get; set; }
        public string Name { get; set; }
        public bool? IsActive { get; set; }
        public DefaultPaymentMethod DefaultPaymentMethod { get; set; }
    }
}
