using CustomerVehicleManagement.Domain.Entities;
using Menominee.Common.Enums;
using Menominee.Common.ValueObjects;
using System;
using System.Collections.Generic;

namespace CustomerVehicleManagement.Shared.Models
{
    public class PersonToEdit
    {
        public PersonNameToEdit Name { get; set; }
        public Gender Gender { get; set; }
        public DateTime? Birthday { get; set; }
        public DriversLicenseToEdit DriversLicense { get; set; }
        public Address Address { get; set; }
        public IList<PhoneToEdit> Phones { get; set; } = new List<PhoneToEdit>();
        public IList<EmailToEdit> Emails { get; set; } = new List<EmailToEdit>();

        public static Person ConvertToEntity(PersonToEdit personUpdateDto)
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
                person.SetDriversLicense(DriversLicenseToEdit.ConvertToEntity(personUpdateDto?.DriversLicense));
                person.SetPhones(PhoneToEdit.ConvertToEntities(personUpdateDto?.Phones));
                person.SetEmails(EmailToEdit.ConvertToEntities(personUpdateDto?.Emails));

                return person;
            }

            return null;
        }
    }
}
