using CustomerVehicleManagement.Api.Organizations;
using CustomerVehicleManagement.Api.Persons;
using SharedKernel.Enums;
using SharedKernel.ValueObjects;

namespace CustomerVehicleManagement.Api.Customers
{
    public class CustomerUpdateDto
    {
        public int Id { get; set; }
        public CustomerType CustomerType { get; set; }
        public PersonUpdateDto PersonUpdateDto { get; set; }
        public OrganizationUpdateDto OrganizationUpdateDto { get; set; }
        public ContactPreferences ContactPreferences { get; set; }
    }
}
