using CustomerVehicleManagement.Domain.Entities;
using CustomerVehicleManagement.Shared.Models;
using CustomerVehicleManagement.Shared.Models.Customers;
using Menominee.Common.Enums;

namespace CustomerVehicleManagement.Shared.Helpers
{
    public class CustomerHelper
    {
        public static CustomerToWrite CreateWriteFromReadDto(CustomerToRead customer)
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

        public static CustomerToRead ConvertToDto(Customer customer)
        {
            if (customer != null)
            {
                var customerReadDto = new CustomerToRead
                {
                    Id = customer.Id,
                    CustomerType = customer.CustomerType,
                    EntityType = customer.EntityType
                };

                if (customer.EntityType == EntityType.Organization)
                {
                    customerReadDto.Organization = OrganizationToRead.ConvertToDto(customer.Organization);
                    customerReadDto.Address = AddressToRead.ConvertToDto(customer.Organization?.Address);
                    customerReadDto.Name = customerReadDto.Organization.Name;
                    customerReadDto.Note = customerReadDto.Organization?.Note;
                    customerReadDto.Phones = customerReadDto.Organization?.Phones;
                    customerReadDto.Emails = customerReadDto.Organization?.Emails;
                    if (customer.Organization.Contact != null)
                        customerReadDto.Contact = PersonToRead.ConvertToDto(customer.Organization.Contact);
                }

                if (customer.EntityType == EntityType.Person)
                {
                    customerReadDto.Person = PersonToRead.ConvertToDto(customer.Person);
                    customerReadDto.Address = AddressToRead.ConvertToDto(customer.Person?.Address);
                    customerReadDto.Name = customerReadDto.Person.LastFirstMiddleInitial;
                    customerReadDto.Phones = customerReadDto.Person?.Phones;
                    customerReadDto.Emails = customerReadDto.Person?.Emails;
                }

                if (customer.EntityType != EntityType.Person && customer.EntityType != EntityType.Organization)
                    return null;

                return customerReadDto;
            }

            return null;
        }
    }
}
