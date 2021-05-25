using CustomerVehicleManagement.Api.Organizations;
using CustomerVehicleManagement.Domain.Entities;
using SharedKernel;
using SharedKernel.Enums;
using SharedKernel.ValueObjects;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CustomerVehicleManagement.Api.Customers
{
    public class CustomerCreateDto : Entity
    {
        [JsonConstructor]
        public CustomerCreateDto(PersonIdDto personIdDto, OrganizationReadDto organization, CustomerType customerType)
        {
            if (personIdDto != null)
            {
                EntityId = personIdDto.Id;
                EntityType = EntityType.Person;
            }

            if (organization != null)
            {

                //if (organization.Id == 0)
                OrganizationReadDto = organization;

                //var organizationNameOrError = OrganizationName.Create(organization.Name);
                //if (organizationNameOrError.IsFailure)
                //    return;

                //Entity = new Organization(organizationNameOrError.Value, organization.Id);
                EntityId = organization.Id;
                EntityType = EntityType.Organization;
            }

            CustomerType = customerType;
        }

        public int EntityId { get; set; }
        public Entity Entity { get; set; }
        public EntityType EntityType { get; set; }
        public PersonIdDto PersonIdDto { get; set; }
        public OrganizationIdDto OrganizationIdDto { get; set; }
        public OrganizationReadDto OrganizationReadDto { get; set; }

        [Required(ErrorMessage = "Customer Type is required.")]
        public CustomerType CustomerType { get; set; }

        //public ContactPreferences ContactPreferences { get; set; }

    }
}
