using CustomerVehicleManagement.Api.Emails;
using CustomerVehicleManagement.Api.Persons;
using CustomerVehicleManagement.Api.Phones;
using System.Collections.Generic;

namespace CustomerVehicleManagement.Api.Organizations
{
    public class OrganizationReadDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public PersonReadDto Contact { get; set; }
        public string AddressLine { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public string AddressFull { get => (AddressLine == null) ? null : $"{AddressLine} {City}, {State}  {PostalCode}"; }
        public string Notes { get; set; }
        public IEnumerable<PhoneReadDto> Phones { get; set; } = new List<PhoneReadDto>();
        public IEnumerable<EmailReadDto> Emails { get; set; } = new List<EmailReadDto>();
    }
}
