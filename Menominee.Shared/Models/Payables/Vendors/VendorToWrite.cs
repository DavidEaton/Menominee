using Menominee.Domain.Enums;
using Menominee.Shared.Models.Addresses;
using Menominee.Shared.Models.Contactable;
using Menominee.Shared.Models.Payables.Invoices.Payments;
using System.Collections.Generic;

namespace Menominee.Shared.Models.Payables.Vendors
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
