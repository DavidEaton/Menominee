using CustomerVehicleManagement.Domain.Entities;
using Menominee.Common.Enums;
using Menominee.Common.ValueObjects;
using System;
using System.Collections.Generic;

namespace CustomerVehicleManagement.Shared.Models
{
    public class PersonToWrite
    {
        public PersonNameToWrite Name { get; set; }
        public Gender Gender { get; set; }
        public DateTime? Birthday { get; set; }
        public DriversLicenseToWrite DriversLicense { get; set; }
        public AddressToWrite Address { get; set; }
        public IList<PhoneToWrite> Phones { get; set; } = new List<PhoneToWrite>();
        public IList<EmailToWrite> Emails { get; set; } = new List<EmailToWrite>();

        public static Person ConvertToEntity(PersonToWrite personToWrite)
        {
            if (personToWrite != null)
            {
                var person = new Person(PersonName.Create(
                                            personToWrite.Name.LastName,
                                            personToWrite.Name.FirstName,
                                            personToWrite.Name.MiddleName).Value,
                                        personToWrite.Gender);

                person.SetAddress(AddressToWrite.ConvertToEntity(personToWrite?.Address));
                person.SetBirthday(personToWrite?.Birthday);
                person.SetDriversLicense(DriversLicenseToWrite.ConvertToEntity(personToWrite?.DriversLicense));
                person.SetPhones(PhoneToWrite.ConvertToEntities(personToWrite?.Phones));
                person.SetEmails(EmailToWrite.ConvertToEntities(personToWrite?.Emails));

                return person;
            }

            return null;
        }
    }
}
