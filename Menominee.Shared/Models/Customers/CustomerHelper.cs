using Menominee.Domain.Entities;
using Menominee.Shared.Models.Addresses;
using Menominee.Shared.Models.Businesses;
using Menominee.Shared.Models.Contactable;
using Menominee.Shared.Models.Persons;
using Menominee.Shared.Models.Vehicles;
using System.Collections.Generic;
using System.Linq;

namespace Menominee.Shared.Models.Customers;

public class CustomerHelper
{
    public static CustomerToRead ConvertWriteToReadDto(CustomerToWrite customer)
    {
        return customer is null
            ? null
            : new CustomerToRead
            {
                Id = customer.Id,
                EntityType = customer.EntityType,
                CustomerType = customer.CustomerType,
                Person =
                    customer.Person is not null
                    ? PersonHelper.ConvertWriteToReadDto(customer.Person)
                    : new(),
                Business =
                    customer.Business is not null
                    ? BusinessHelper.CovertWriteToReadDto(customer.Business)
                    : new(),
                Code = customer.Code,
            };
    }

    public static CustomerToWrite ConvertReadToWriteDto(CustomerToRead customer)
    {
        return customer is null
            ? null
            : new CustomerToWrite
            {
                Id = customer.Id,
                EntityType = customer.EntityType,
                CustomerType = customer.CustomerType,
                Person =
                    customer.Person is not null
                    ? PersonHelper.ConvertReadToWriteDto(customer.Person)
                    : new(),
                Business =
                    customer.Business is not null
                    ? BusinessHelper.CovertReadToWriteDto(customer.Business)
                    : new(),
                Code = customer.Code,
            };
    }

    public static CustomerToRead ConvertToReadDto(Customer customer)
    {
        return customer is null
            ? new()
            : new()
            {
                Id = customer.Id,
                CustomerType = customer.CustomerType,
                EntityType = customer.EntityType,
                Business = BusinessHelper.ConvertToReadDto(customer.Business),
                Person = PersonHelper.ConvertToReadDto(customer.Person),
                Vehicles = VehicleHelper.ConvertToReadDtos(customer.Vehicles),
                Contact = PersonHelper.ConvertToReadDto(customer?.Contact),
                Address = AddressHelper.ConvertToReadDto(customer?.Address),
                Notes = customer?.Notes,
                Phones = PhoneHelper.ConvertToReadDtos(customer?.Phones),
                Emails = EmailHelper.ConvertToReadDtos(customer?.Emails)
            };
    }

    public static CustomerToReadInList ConvertToReadInListDto(Customer customer)
    {
        return customer is not null
        ? new()
        {
            Id = customer.Id,
            EntityType = customer.EntityType,
            CustomerType = customer.CustomerType,
            Name = customer.Name,
            AddressFull = customer?.Address?.AddressFull,
            PrimaryPhone = PhoneHelper.GetPrimaryPhone(customer?.Contact),
            PrimaryEmail = EmailHelper.GetPrimaryEmail(customer?.Contact)
        }
        : new();
    }

    public static CustomerToWrite ConvertToWriteDto(Customer customer)
    {
        return customer is null
            ? new()
            : new()
            {
                Id = customer.Id,
                CustomerType = customer.CustomerType,
                EntityType = customer.EntityType,
                Person = customer.Person is not null ? PersonHelper.ConvertToWriteDto(customer.Person) : new(),
                Business = customer.Business is not null ? BusinessHelper.CovertToWriteDto(customer.Business) : new(),
                Vehicles = ConvertVehiclesToWrite(customer)
            };
    }

    private static List<VehicleToWrite> ConvertVehiclesToWrite(Customer customer)
    {
        return customer.Vehicles.Select(vehicle => VehicleHelper.ConvertToWriteDto(vehicle)).ToList();
    }

    public static CustomerToWrite ConvertToWriteDto(CustomerToRead customer)
    {
        return customer is null
        ? new()
        : new()
        {
            Id = customer.Id,
            CustomerType = customer.CustomerType,
            EntityType = customer.EntityType,
            Person = customer.Person is not null ? PersonHelper.ConvertToWriteDto(customer.Person) : new(),
            Business = customer.Business is not null ? BusinessHelper.ConvertReadToWriteDto(customer.Business) : new(),
            Vehicles = ConvertVehiclesToWrite(customer)
        };
    }

    private static IList<VehicleToWrite> ConvertVehiclesToWrite(CustomerToRead customer)
    {
        return customer.Vehicles.Select(vehicle => VehicleHelper.ConvertReadToWriteDto(vehicle)).ToList();
    }
}
