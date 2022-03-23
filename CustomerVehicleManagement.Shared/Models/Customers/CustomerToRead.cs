using Menominee.Common.Enums;
using System.Collections.Generic;

namespace CustomerVehicleManagement.Shared.Models.Customers
{
    public class CustomerToRead
    {
        public long Id { get; set; }
        public PersonToRead Person { get; set; }
        public OrganizationToRead Organization { get; set; }
        public EntityType EntityType { get; set; }
        public string Name { get; set; }
        public AddressToRead Address { get; set; }
        public string Note { get; set; }
        public PersonToRead Contact { get; set; }
        public CustomerType CustomerType { get; set; }
        public IReadOnlyList<PhoneToRead> Phones { get; set; } = new List<PhoneToRead>();
        public IReadOnlyList<EmailToRead> Emails { get; set; } = new List<EmailToRead>();
    }
}