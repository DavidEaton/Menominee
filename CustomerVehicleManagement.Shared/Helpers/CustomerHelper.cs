using CustomerVehicleManagement.Shared.Models;
using Menominee.Common.Enums;

namespace CustomerVehicleManagement.Shared.Helpers
{
    public class CustomerHelper
    {
        public static CustomerToWrite CreateWriteDtoFromReadDto(CustomerToRead customer)
        {
            var Customer = new CustomerToWrite
            {
                EntityType = customer.EntityType,
                CustomerType = customer.CustomerType
            };

            if (customer.EntityType == EntityType.Person)
            {
                Customer.Person = PersonHelper.CreateWriteDtoFromReadDto(customer.Person);
            }

            if (customer.EntityType == EntityType.Organization)
            {
                Customer.Organization = OrganizationHelper.CreateWriteDtoFromReadDto(customer.Organization);
            }

            return Customer;
        }
    }
}
