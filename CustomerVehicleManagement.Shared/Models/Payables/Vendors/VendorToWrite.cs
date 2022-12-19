using CustomerVehicleManagement.Domain.Entities.Payables;
using CustomerVehicleManagement.Shared.Models.Addresses;
using CustomerVehicleManagement.Shared.Models.Contactable;
using System.Collections.Generic;

namespace CustomerVehicleManagement.Shared.Models.Payables.Vendors
{
    public class VendorToWrite
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string VendorCode { get; set; } = string.Empty;
        public bool? IsActive { get; set; }
        public AddressToWrite Address { get; set; }
        public string Notes { get; set; } = string.Empty;
        public IList<PhoneToWrite> Phones { get; set; } = new List<PhoneToWrite>();
        public IList<EmailToWrite> Emails { get; set; } = new List<EmailToWrite>();
        public DefaultPaymentMethod DefaultPaymentMethod { get; set; }
    }
}
