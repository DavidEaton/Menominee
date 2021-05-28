using CustomerVehicleManagement.Api.Emails;
using CustomerVehicleManagement.Api.Persons;
using CustomerVehicleManagement.Api.Phones;
using SharedKernel.ValueObjects;
using System.Collections.Generic;

namespace CustomerVehicleManagement.Api.Organizations
{
    public class OrganizationReadDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Address Address { get; set; }
        public PersonReadDto Contact { get; set; }
        public string Notes { get; set; }
        public IEnumerable<PhoneReadDto> Phones { get; set; } = new List<PhoneReadDto>();
        public IEnumerable<EmailReadDto> Emails { get; set; } = new List<EmailReadDto>();
    }
}
