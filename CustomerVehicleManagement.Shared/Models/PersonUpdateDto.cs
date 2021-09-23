using CustomerVehicleManagement.Domain.Entities;
using SharedKernel.Enums;
using SharedKernel.ValueObjects;
using System;
using System.Collections.Generic;

namespace CustomerVehicleManagement.Shared.Models
{
    public class PersonUpdateDto
    {
        public PersonNameUpdateDto Name { get; set; }
        public Gender Gender { get; set; }
        public DateTime? Birthday { get; set; }
        public DriversLicenseUpdateDto DriversLicense { get; set; }
        public Address Address { get; set; }
        public IList<PhoneUpdateDto> Phones { get; set; } = new List<PhoneUpdateDto>();
        public IList<EmailUpdateDto> Emails { get; set; } = new List<EmailUpdateDto>();

        public static Person ConvertToEntity(PersonUpdateDto personUpdateDto)
        {
            if (personUpdateDto != null)
            {
                var person = new Person(PersonName.Create(
                                            personUpdateDto.Name.LastName,
                                            personUpdateDto.Name.FirstName,
                                            personUpdateDto.Name.MiddleName).Value,
                                        personUpdateDto.Gender);

                person.SetAddress(personUpdateDto?.Address);
                person.SetBirthday(personUpdateDto?.Birthday);
                person.SetDriversLicense(DriversLicenseUpdateDto.ConvertToEntity(personUpdateDto?.DriversLicense));
                person.SetPhones(PhoneUpdateDto.ConvertToEntities(personUpdateDto?.Phones));
                person.SetEmails(EmailUpdateDto.ConvertToEntities(personUpdateDto?.Emails));

                return person;
            }

            return null;
        }
    }
}
