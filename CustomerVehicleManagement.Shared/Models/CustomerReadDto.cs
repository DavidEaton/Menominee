using CustomerVehicleManagement.Domain.Entities;
using SharedKernel.Enums;
using System.Collections.Generic;

namespace CustomerVehicleManagement.Shared.Models
{
    public class CustomerReadDto
    {
        public long Id { get; set; }
        public PersonReadDto Person { get; set; }
        public OrganizationReadDto Organization { get; set; }
        public EntityType EntityType { get; set; }
        public string Name { get; set; }
        public AddressReadDto Address { get; set; }
        public string Note { get; set; }
        public PersonReadDto Contact { get; set; }
        public CustomerType CustomerType { get; set; }
        public IReadOnlyList<PhoneReadDto> Phones { get; set; } = new List<PhoneReadDto>();
        public IReadOnlyList<EmailReadDto> Emails { get; set; } = new List<EmailReadDto>();
        public static CustomerReadDto ConvertToDto(Customer customer)
        {
            if (customer != null)
            {
                var customerReadDto = new CustomerReadDto
                {
                    Id = customer.Id,
                    CustomerType = customer.CustomerType,
                    EntityType = customer.EntityType
                };

                if (customer.EntityType == EntityType.Organization)
                {
                    customerReadDto.Organization = OrganizationReadDto.ConvertToDto(customer.Organization);
                    customerReadDto.Address = AddressReadDto.ConvertToDto(customer.Organization?.Address);
                    customerReadDto.Name = customerReadDto.Organization.Name;
                    customerReadDto.Note = customerReadDto.Organization?.Note;
                    customerReadDto.Phones = customerReadDto.Organization?.Phones;
                    customerReadDto.Emails = customerReadDto.Organization?.Emails;
                    if (customer.Organization.Contact != null)
                        customerReadDto.Contact = PersonReadDto.ConvertToDto(customer.Organization.Contact);
                }

                if (customer.EntityType == EntityType.Person)
                {
                    customerReadDto.Person = PersonReadDto.ConvertToDto(customer.Person);
                    customerReadDto.Address = AddressReadDto.ConvertToDto(customer.Person?.Address);
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