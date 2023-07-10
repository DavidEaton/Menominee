using Menominee.Shared.Models.Addresses;
using Menominee.Shared.Models.Contactable;
using Menominee.Shared.Models.Persons;
using System.Collections.Generic;

namespace Menominee.Shared.Models.Organizations
{
    public class OrganizationToRead
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public AddressToRead Address { get; set; }
        public PersonToRead Contact { get; set; }
        public string Notes { get; set; }
        public List<PhoneToRead> Phones { get; set; } = new List<PhoneToRead>();
        public List<EmailToRead> Emails { get; set; } = new List<EmailToRead>();
    }
}