using CustomerVehicleManagement.Shared.Models.Addresses;
using CustomerVehicleManagement.Shared.Models.Contactable;
using CustomerVehicleManagement.Shared.Models.Payables.Invoices.Payments;
using Menominee.Common.Enums;
using System.Collections.Generic;

namespace CustomerVehicleManagement.Shared.Models.Payables.Vendors
{
    public class VendorToWrite
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string VendorCode { get; set; } = string.Empty;
        public VendorRole VendorRole { get; set; }
        public bool? IsActive { get; set; }
        public AddressToWrite Address { get; set; }
        public DefaultPaymentMethodToRead DefaultPaymentMethod { get; set; }
        public string Notes { get; set; } = string.Empty;
        public List<PhoneToWrite> Phones { get; set; } = new List<PhoneToWrite>();
        public List<EmailToWrite> Emails { get; set; } = new List<EmailToWrite>();
    }
}
