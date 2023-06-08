using CustomerVehicleManagement.Domain.Entities;
using CustomerVehicleManagement.Shared.Models.Addresses;
using CustomerVehicleManagement.Shared.Models.Contactable;
using CustomerVehicleManagement.Shared.Models.Organizations;
using CustomerVehicleManagement.Shared.Models.Persons;
using CustomerVehicleManagement.Shared.Models.Vehicles;

namespace CustomerVehicleManagement.Shared.Models.Customers
{
    public class CustomerHelper
    {
        public static CustomerToWrite CovertReadToWriteDto(CustomerToRead customer)
        {
            var customerToWrite = new CustomerToWrite
            {
                EntityType = customer.EntityType,
                CustomerType = customer.CustomerType,
                Person
                    = customer.Person is not null
                    ? PersonHelper.ConvertReadToWriteDto(customer.Person)
                    : null,
                Organization
                    = customer.Organization is not null
                    ? OrganizationHelper.CovertReadToWriteDto(customer.Organization)
                    : null
            };

            return customerToWrite;
        }

        public static CustomerToRead ConvertToReadDto(Customer customer)
        {
            if (customer is not null)
            {
                var customerReadDto = new CustomerToRead
                {
                    Id = customer.Id,
                    CustomerType = customer.CustomerType,
                    EntityType = customer.EntityType,
                    Organization = OrganizationHelper.ConvertToReadDto(customer.Organization),
                    Person = PersonHelper.ConvertToReadDto(customer.Person),
                    Vehicles = VehicleHelper.ConvertToReadDtos(customer.Vehicles),
                    Contact = PersonHelper.ConvertToReadDto(customer.Contact),
                    Address = AddressHelper.ConvertToReadDto(customer?.Address),
                    Notes = customer?.Notes,
                    Phones = PhoneHelper.ConvertToReadDtos(customer?.Phones),
                    Emails = EmailHelper.ConvertToReadDtos(customer?.Emails)
                };

                if (customer.Contact is not null)
                    customerReadDto.Contact = PersonHelper.ConvertToReadDto(customer?.Contact);

                return customerReadDto;
            }

            return null;
        }

        public static CustomerToWrite ConvertReadToWriteDto(CustomerToRead customer)
        {
            if (customer is not null)
            {
                var customerWriteDto = new CustomerToWrite
                {
                    CustomerType = customer.CustomerType,
                    EntityType = customer.EntityType,
                    Person = PersonHelper.ConvertReadToWriteDto(customer.Person),
                    Organization = OrganizationHelper.CovertReadToWriteDto(customer.Organization)
                };

                return customerWriteDto;
            }

            return null;
        }
    }
}