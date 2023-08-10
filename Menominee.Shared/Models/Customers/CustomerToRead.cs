using Menominee.Shared.Models.Addresses;
using Menominee.Shared.Models.Contactable;
using Menominee.Shared.Models.Businesses;
using Menominee.Shared.Models.Persons;
using Menominee.Shared.Models.Vehicles;
using Menominee.Common.Enums;
using System.Collections.Generic;

namespace Menominee.Shared.Models.Customers
{
    public class CustomerToRead
    {
        public long Id { get; set; }
        public PersonToRead Person { get; set; }
        public BusinessToRead Business { get; set; }
        public EntityType EntityType { get; set; }
        public string Name =>
            Person is not null
                ? Person.Name.FirstMiddleLast
                : Business is not null
                    ? Business.Name
                    : string.Empty;
        public CustomerType CustomerType { get; set; }
        public AddressToRead Address { get; set; }
        public string Notes { get; set; }
        public PersonToRead Contact { get; set; }
        public IList<PhoneToRead> Phones { get; set; } = new List<PhoneToRead>();
        public IList<EmailToRead> Emails { get; set; } = new List<EmailToRead>();
        public IList<VehicleToRead> Vehicles { get; set; } = new List<VehicleToRead>();
    }
}