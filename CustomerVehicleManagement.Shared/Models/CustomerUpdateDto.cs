using SharedKernel.Enums;
using System.Text.Json.Serialization;

namespace CustomerVehicleManagement.Shared.Models
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
