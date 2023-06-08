using CustomerVehicleManagement.Shared.Models.Addresses;
using CustomerVehicleManagement.Shared.Models.Contactable;
using CustomerVehicleManagement.Shared.Models.Payables.Invoices.Payments;
using Menominee.Common.Enums;
using System.Collections.Generic;

namespace CustomerVehicleManagement.Shared.Models.Payables.Vendors
{
    public class VendorToRead
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string VendorCode { get; set; }
        public VendorRole VendorRole { get; set; }
        public bool? IsActive { get; set; }
        public DefaultPaymentMethodToRead DefaultPaymentMethod { get; set; }
        public AddressToRead Address { get; set; }
        public string Notes { get; set; }
        public List<PhoneToRead> Phones { get; set; }
        public List<EmailToRead> Emails { get; set; }
    }
}
