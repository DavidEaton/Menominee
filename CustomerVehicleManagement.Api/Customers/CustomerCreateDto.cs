using CustomerVehicleManagement.Api.Organizations;
using CustomerVehicleManagement.Api.Persons;
using CustomerVehicleManagement.Domain.BaseClasses;
using SharedKernel.Enums;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CustomerVehicleManagement.Api.Customers
{
    public class CustomerCreateDto : Contactable
    {
        [JsonConstructor]
        public CustomerCreateDto(PersonCreateDto personCreateDto, OrganizationCreateDto organizationCreateDto, CustomerType customerType)
        {
            if (personCreateDto != null)
            {
                PersonCreateDto = personCreateDto;
                EntityType = EntityType.Person;
            }

            if (organizationCreateDto != null)
            {
                OrganizationCreateDto = organizationCreateDto;
                EntityType = EntityType.Organization;
            }

            CustomerType = customerType;
        }

        public EntityType EntityType { get; set; }
        public OrganizationCreateDto OrganizationCreateDto { get; set; }
        public PersonCreateDto PersonCreateDto { get; set; }

        [Required(ErrorMessage = "Customer Type is required.")]
        public CustomerType CustomerType { get; set; }

        //public ContactPreferences ContactPreferences { get; set; }

    }
}
