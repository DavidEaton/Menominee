using Menominee.Common.Enums;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CustomerVehicleManagement.Shared.Models
{
    public class CustomerToAdd
    {
        [JsonConstructor]
        public CustomerToAdd(PersonToAdd personCreateDto, OrganizationToAdd organizationCreateDto, CustomerType customerType)
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
        public OrganizationToAdd OrganizationCreateDto { get; set; }
        public PersonToAdd PersonCreateDto { get; set; }

        [Required(ErrorMessage = "Customer Type is required.")]
        public CustomerType CustomerType { get; set; }

        //public ContactPreferences ContactPreferences { get; set; }

    }
}
