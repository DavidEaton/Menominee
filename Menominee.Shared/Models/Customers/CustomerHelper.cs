using Menominee.Domain.Entities;
using Menominee.Shared.Models.Addresses;
using Menominee.Shared.Models.Contactable;
using Menominee.Shared.Models.Businesses;
using Menominee.Shared.Models.Persons;
using Menominee.Shared.Models.Vehicles;

namespace Menominee.Shared.Models.Customers
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
                Business
                    = customer.Business is not null
                    ? BusinessHelper.CovertReadToWriteDto(customer.Business)
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
                    Business = BusinessHelper.ConvertToReadDto(customer.Business),
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
                    Business = BusinessHelper.CovertReadToWriteDto(customer.Business)
                };

                return customerWriteDto;
            }

            return null;
        }
    }
}