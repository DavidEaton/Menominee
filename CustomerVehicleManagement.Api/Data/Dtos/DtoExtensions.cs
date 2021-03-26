using CustomerVehicleManagement.Domain.Entities;
using SharedKernel.Enums;
using System.Collections.Generic;

namespace CustomerVehicleManagement.Api.Data.Dtos
{
    /// <summary>
    /// The static DTO Extensions class.
    /// Contains static extension methods for converting domain classes to data transfer objects (DTOs).
    /// </summary>
    /// <remarks>
    /// It made more sense to map these manually, rather than trying to implement AutoMapper
    /// in a static class. Maybe AutoMapper should have been used, but these extension
    /// methods work well, and are easy enough to read and comprehend.
    /// </remarks>
    public static class DtoExtensions
    {
        /// <summary>
        /// Converts a domain Customer to a CustomerReadDto and returns it.
        /// </summary>
        /// <returns>
        /// A CustomerReadDto object.
        /// </returns>
        /// <param name="customer">A domain Customer object</param>
        public static CustomerReadDto ToReadDto(this Customer customer)
        {
            if (customer != null)
            {
                if (customer.EntityType == EntityType.Organization)
                {
                    Organization organization = customer.Entity as Organization;

                    return new CustomerReadDto
                    {
                        Id = customer.Id,
                        EntityType = EntityType.Organization,
                        Name = organization.Name.Value,
                        Address = organization.Address,
                        CustomerType = customer.CustomerType,
                        Contact = (organization.Contact != null)
                        ? new PersonReadDto()
                        {
                            Id = organization.Contact.Id,
                            Address = organization.Contact.Address,
                            Gender = organization.Contact.Gender,
                            Name = organization.Contact.Name.LastFirstMiddle,
                            Phones = MapPhones(organization.Contact.Phones)
                        }
                        : null
                    };
                }

                if (customer.EntityType == EntityType.Person)
                {
                    Person person = customer.Entity as Person;

                    return new CustomerReadDto
                    {
                        Id = customer.Id,
                        Name = person.Name.LastFirstMiddle,
                        Address = person.Address,
                        Phones = MapPhones(person.Phones)
                    };
                }
            }

            return null;
        }

        private static IList<PhoneReadDto> MapPhones(IList<Phone> phones)
        {
            var phonesDto = new List<PhoneReadDto>();

            foreach (var phone in phones)
            {
                PhoneReadDto phoneReadDto = new PhoneReadDto()
                {
                    Number = phone.Number,
                    PhoneType = phone.PhoneType.ToString(),
                    IsPrimary = phone.IsPrimary
                };

                phonesDto.Add(phoneReadDto);
            }

            return phonesDto;
        }
    }
}
