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
                Person = customer.Person is not null ? PersonHelper.ConvertToWriteDto(customer.Person) : null,
                Business = customer.Business is not null ? BusinessHelper.CovertToWriteDto(customer.Business) : null,
                Vehicles = ConvertVehiclesToWrite(customer)
            };
    }

    private static List<VehicleToWrite> ConvertVehiclesToWrite(Customer customer)
    {
        return customer.Vehicles.Select(vehicle => VehicleHelper.ConvertToWriteDto(vehicle)).ToList();
    }

    public static CustomerToWrite ConvertToWriteDto(CustomerToRead customer)
    {
        if (customer is null)
        {
            return new();
        }
        else
        {
            var convertedCustomer = new CustomerToWrite
            {
                Id = customer.Id,
                CustomerType = customer.CustomerType,
                EntityType = customer.EntityType,
                Code = customer.Code
            };

            if (customer.Person is not null)
                convertedCustomer.Person = PersonHelper.ConvertToWriteDto(customer.Person);

            if (customer.Business is not null)
                convertedCustomer.Business = BusinessHelper.CovertReadToWriteDto(customer.Business);

            if (customer.Vehicles.Count > 0)
                foreach (var vehicle in customer.Vehicles)
                    convertedCustomer.Vehicles.Add(VehicleHelper.ConvertReadToWriteDto(vehicle));

            return convertedCustomer;

        }
    }
}