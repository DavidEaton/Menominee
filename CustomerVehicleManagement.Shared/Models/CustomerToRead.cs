using CustomerVehicleManagement.Domain.Entities;
using Menominee.Common.Enums;
using System.Collections.Generic;

namespace CustomerVehicleManagement.Shared.Models
{
    public class CustomerToRead
    {
        public long Id { get; set; }
        public PersonToRead Person { get; set; }
        public OrganizationToRead Organization { get; set; }
        public EntityType EntityType { get; set; }
        public string Name { get; set; }
        public AddressToRead Address { get; set; }
        public string Note { get; set; }
        public PersonToRead Contact { get; set; }
        public CustomerType CustomerType { get; set; }
        public IReadOnlyList<PhoneToRead> Phones { get; set; } = new List<PhoneToRead>();
        public IReadOnlyList<EmailToRead> Emails { get; set; } = new List<EmailToRead>();
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
                    customerReadDto.Name = customerReadDto.Person.Name;
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