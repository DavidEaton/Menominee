using CustomerVehicleManagement.Api.Organizations;
using CustomerVehicleManagement.Shared.Models;
using SharedKernel.Enums;
using System.Text.Json.Serialization;

namespace CustomerVehicleManagement.Api.Customers
{
    public class CustomerUpdateDto
    {
        [JsonConstructor]
        public CustomerUpdateDto(CustomerType customerType, PersonUpdateDto personUpdateDto, OrganizationUpdateDto organizationUpdateDto)
        {
            CustomerType = customerType;
            PersonUpdateDto = personUpdateDto;
            OrganizationUpdateDto = organizationUpdateDto;
        }
        public CustomerType CustomerType { get; set; }
        public PersonUpdateDto PersonUpdateDto { get; set; }
        public OrganizationUpdateDto OrganizationUpdateDto { get; set; }
        //public ContactPreferences ContactPreferences { get; set; }
    }
}
