using CustomerVehicleManagement.Api.Emails;
using CustomerVehicleManagement.Api.Persons;
using CustomerVehicleManagement.Api.Phones;
using SharedKernel.Enums;
using SharedKernel.ValueObjects;
using System.Collections.Generic;

namespace CustomerVehicleManagement.Api.Customers
{
    public class CustomerReadDto
    {
        public int Id { get; set; }
        public EntityType EntityType { get; set; }
        public string Name { get; set; }
        public Address Address { get; set; }
        public PersonReadDto Contact { get; set; }
        public CustomerType CustomerType { get; set; }
        public IList<PhoneReadDto> Phones { get; set; } = new List<PhoneReadDto>();
        public IList<EmailReadDto> Emails { get; set; } = new List<EmailReadDto>();

    }
}
