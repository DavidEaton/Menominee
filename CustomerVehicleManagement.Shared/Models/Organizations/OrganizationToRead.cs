using CustomerVehicleManagement.Shared.Models.Addresses;
using CustomerVehicleManagement.Shared.Models.Contactable;
using CustomerVehicleManagement.Shared.Models.Persons;
using System.Collections.Generic;

namespace CustomerVehicleManagement.Shared.Models.Organizations
{
    public class OrganizationToRead
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public AddressToRead Address { get; set; }
        public PersonToRead Contact { get; set; }
        public string Note { get; set; }
        public IReadOnlyList<PhoneToRead> Phones { get; set; } = new List<PhoneToRead>();
        public IReadOnlyList<EmailToRead> Emails { get; set; } = new List<EmailToRead>();
    }
}
