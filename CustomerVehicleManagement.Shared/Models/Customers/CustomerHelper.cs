using CustomerVehicleManagement.Domain.Entities;
using CustomerVehicleManagement.Shared.Models.Addresses;
using CustomerVehicleManagement.Shared.Models.Organizations;
using CustomerVehicleManagement.Shared.Models.Persons;
using Menominee.Common.Enums;
using System;

namespace CustomerVehicleManagement.Shared.Models.Customers
{
    public class CustomerHelper
    {
        public static CustomerToWrite CovertReadToWriteDto(CustomerToRead customer)
        {
            var Customer = new CustomerToWrite
            {
                EntityType = customer.EntityType,
                CustomerType = customer.CustomerType
            };

            if (customer.EntityType == EntityType.Person)
            {
                Customer.Person = PersonHelper.ConvertReadToWriteDto(customer.Person);
            }

            if (customer.EntityType == EntityType.Organization)
            {
                Customer.Organization = OrganizationHelper.CovertReadToWriteDto(customer.Organization);
            }

            return Customer;
        }

        public static CustomerToRead ConvertEntityToReadDto(Customer customer)
        {
            if (customer is not null)
            {
                var customerReadDto = new CustomerToRead
                {
                    Id = customer.Id,
                    CustomerType = customer.CustomerType,
                    EntityType = customer.EntityType
                };

                if (customer.EntityType == EntityType.Organization)
                {
                    customerReadDto.Organization = OrganizationHelper.ConvertEntityToReadDto(customer.Organization);
                    customerReadDto.Address = AddressHelper.ConvertEntityToReadDto(customer.Organization?.Address);
                    customerReadDto.Name = customerReadDto.Organization.Name;
                    customerReadDto.Notes = customerReadDto.Organization?.Notes;
                    customerReadDto.Phones = customerReadDto.Organization?.Phones;
                    customerReadDto.Emails = customerReadDto.Organization?.Emails;
                    if (customer.Organization.Contact is not null)
                        customerReadDto.Contact = PersonHelper.ConvertToReadDto(customer.Organization.Contact);
                }

                if (customer.EntityType == EntityType.Person)
                {
                    customerReadDto.Person = PersonHelper.ConvertToReadDto(customer.Person);
                    customerReadDto.Address = AddressHelper.ConvertEntityToReadDto(customer.Person?.Address);
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

        public static CustomerToWrite ConvertReadToWriteDto(CustomerToRead customerReadDto)
        {
            throw new NotImplementedException();
        }
    }
}