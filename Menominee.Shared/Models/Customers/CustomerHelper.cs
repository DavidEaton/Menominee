using Menominee.Domain.Entities;
using Menominee.Domain.Enums;
using Menominee.Domain.Interfaces;
using Menominee.Shared.Models.Businesses;
using Menominee.Shared.Models.Contactable;
using Menominee.Shared.Models.Persons;
using Menominee.Shared.Models.Vehicles;
using System;
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
                Code = customer.Code,
                Person = customer.Person is not null
                        ? PersonHelper.ConvertReadToWriteDto(customer.Person)
                        : new(),
                Business = customer.Business is not null
                        ? BusinessHelper.ConvertReadToWriteDto(customer.Business)
                        : new(),
                Vehicles = customer.Vehicles.Select(VehicleHelper.ConvertReadToWriteDto).ToList()
            };
    }

    public static CustomerToRead ConvertToReadDto(Customer customer)
    {
        return
            customer is null || customer.CustomerEntity is null
            ? new()
            : new()
            {
                Id = customer.Id,
                Code = customer.Code,
                CustomerType = customer.CustomerType,
                EntityType = customer.CustomerEntity switch
                {
                    Person => EntityType.Person,
                    Business => EntityType.Business,
                    _ => throw new InvalidOperationException("Unknown customer entity type.")
                },
                Notes = customer.Notes,
                Person = customer.CustomerEntity is Person
                ? PersonHelper.ConvertToReadDto(customer.CustomerEntity as Person)
                : null,
                Business = customer.CustomerEntity is Business
                ? BusinessHelper.ConvertToReadDto(customer.CustomerEntity as Business)
                : null,
                Vehicles = VehicleHelper.ConvertToReadDtos(customer.Vehicles)
            };
    }

    public static CustomerToReadInList ConvertToReadInListDto(Customer customer)
    {
        return customer is null
            ? new()
            : new()
            {
                Id = customer.Id,
                Code = customer.Code,
                EntityType = GetEntityType(customer.CustomerEntity),
                Name = customer.DisplayName,
                AddressFull = customer.CustomerEntity?.Address?.AddressFull,
                CustomerType = customer.CustomerType,
                PrimaryPhone = GetPrimaryPhoneString(customer.CustomerEntity),
                PrimaryEmail = GetPrimaryEmailString(customer.CustomerEntity)
            };
    }

    private static EntityType GetEntityType(ICustomerEntity customerEntity)
    {
        return customerEntity switch
        {
            Person => EntityType.Person,
            Business => EntityType.Business,
            _ => throw new InvalidOperationException("Unknown customer entity type.")
        };
    }

    private static string GetPrimaryPhoneString(ICustomerEntity entity)
    {
        var primaryPhoneResult = PhoneHelper.GetPrimaryPhone(entity);
        if (primaryPhoneResult.IsSuccess)
        {
            return primaryPhoneResult.Value.ToString();
        }

        var ordinalPhone = PhoneHelper.GetOrdinalPhone(entity, 0);
        return ordinalPhone?.ToString() ?? default;
    }

    private static string GetPrimaryEmailString(ICustomerEntity entity)
    {
        var primaryEmail = EmailHelper.GetPrimaryEmail(entity);
        if (!string.IsNullOrEmpty(primaryEmail))
        {
            return primaryEmail;
        }

        var ordinalEmail = EmailHelper.GetOrdinalEmail(entity, 0);
        return ordinalEmail?.ToString() ?? default;
    }


    public static CustomerToWrite ConvertToWriteDto(Customer customer)
    {
        return customer is null
            ? new()
            : new()
            {
                Id = customer.Id,
                EntityType = customer.EntityType,
                Code = customer.Code,
                CustomerType = customer.CustomerType,
                Person = customer.CustomerEntity is Person ? PersonHelper.ConvertToWriteDto(customer.CustomerEntity as Person) : null,
                Business = customer.CustomerEntity is Business ? BusinessHelper.ConvertToWriteDto(customer.CustomerEntity as Business) : null,
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
                convertedCustomer.Business = BusinessHelper.ConvertReadToWriteDto(customer.Business);

            if (customer.Vehicles.Count > 0)
                foreach (var vehicle in customer.Vehicles)
                    convertedCustomer.Vehicles.Add(VehicleHelper.ConvertReadToWriteDto(vehicle));

            return convertedCustomer;
        }
    }

    private static IList<VehicleToWrite> ConvertVehiclesToWrite(CustomerToRead customer)
    {
        return customer.Vehicles.Select(vehicle => VehicleHelper.ConvertReadToWriteDto(vehicle)).ToList();
    }
}