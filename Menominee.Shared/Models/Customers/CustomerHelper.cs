using Menominee.Domain.Entities;
using Menominee.Shared.Models.Addresses;
using Menominee.Shared.Models.Businesses;
using Menominee.Shared.Models.Contactable;
using Menominee.Shared.Models.Persons;
using Menominee.Shared.Models.Vehicles;

namespace Menominee.Shared.Models.Customers;

public class CustomerHelper
{
    public static CustomerToWrite ConvertReadToWriteDto(CustomerToRead customer)
    {
        return customer is null
            ? null
            : new CustomerToWrite
            {
                Id = customer.Id,
                EntityType = customer.EntityType,
                CustomerType = customer.CustomerType,
                Person
                    = customer.Person is not null
                    ? PersonHelper.ConvertReadToWriteDto(customer.Person)
                    : null,
                Business
                    = customer.Business is not null
                    ? BusinessHelper.CovertReadToWriteDto(customer.Business)
                    : null,
                Code = customer.Code,
            };
    }

    public static CustomerToRead ConvertToReadDto(Customer customer)
    {
        return customer is null
            ? new CustomerToRead()
            : new CustomerToRead
            {
                Id = customer.Id,
                CustomerType = customer.CustomerType,
                EntityType = customer.EntityType,
                Business = BusinessHelper.ConvertToReadDto(customer.Business),
                Person = PersonHelper.ConvertToReadDto(customer.Person),
                Contact = customer.Contact is not null
                    ? PersonHelper.ConvertToReadDto(customer.Contact)
                    : null,
                Code = customer.Code,
                Vehicles = VehicleHelper.ConvertToReadDtos(customer.Vehicles),
                Address = AddressHelper.ConvertToReadDto(customer.Address),
                Notes = customer.Notes,
                Phones = PhoneHelper.ConvertToReadDtos(customer.Phones),
                Emails = EmailHelper.ConvertToReadDtos(customer.Emails),
            };
    }
}