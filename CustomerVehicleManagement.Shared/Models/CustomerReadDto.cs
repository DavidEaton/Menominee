using CustomerVehicleManagement.Api.Organizations;
using CustomerVehicleManagement.Shared.Models;
using SharedKernel.Enums;
using SharedKernel.ValueObjects;
using System.Collections.Generic;

namespace CustomerVehicleManagement.Shared.Models
{
    public class CustomerReadDto
    {
        public int Id { get; set; }
        public PersonReadDto Person { get; set; }
        public OrganizationReadDto Organization { get; set; }
        public EntityType EntityType { get; set; }
        public string Name { get; set; }
        public Address Address { get; set; }
        public string Note { get; set; }
        public PersonReadDto Contact { get; set; }
        public CustomerType CustomerType { get; set; }
        public IList<PhoneReadDto> Phones { get; set; } = new List<PhoneReadDto>();
        public IList<EmailReadDto> Emails { get; set; } = new List<EmailReadDto>();
    }
}