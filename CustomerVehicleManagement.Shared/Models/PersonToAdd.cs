using CustomerVehicleManagement.Domain.Entities;
using Menominee.Common.Enums;
using Menominee.Common.ValueObjects;
using System;
using System.Collections.Generic;

namespace CustomerVehicleManagement.Shared.Models
{
    public class PersonToAdd
    {
        public PersonNameToAdd Name { get; set; }
        public Gender Gender { get; set; }
        public DateTime? Birthday { get; set; }
        public DriversLicenseToAdd DriversLicense { get; set; }
        public AddressToAdd Address { get; set; }
        public IList<PhoneToAdd> Phones { get; set; } = new List<PhoneToAdd>();
        public IList<EmailToAdd> Emails { get; set; } = new List<EmailToAdd>();

        public static Person ConvertToEntity(PersonToAdd personAddDto)
        {
            if (personAddDto != null)
            {
                var person = new Person(
                    PersonName.Create(
                        personAddDto.Name.LastName,
                        personAddDto.Name.FirstName,
                        personAddDto.Name.MiddleName).Value,
                    personAddDto.Gender);

                if (personAddDto?.Address != null)
                    person.SetAddress(Menominee.Common.ValueObjects.Address.Create(
                        personAddDto.Address.AddressLine,
                        personAddDto.Address.City,
                        personAddDto.Address.State,
                        personAddDto.Address.PostalCode).Value);

                person.SetBirthday(personAddDto?.Birthday);

                if (personAddDto.DriversLicense != null)
                {
                    DateTimeRange dateTimeRange = DateTimeRange.Create(
                                                                    personAddDto.DriversLicense.Issued,
                                                                    personAddDto.DriversLicense.Expiry)
                                                               .Value;

                    DriversLicense driversLicense = Menominee.Common.ValueObjects.DriversLicense.Create(
                                                                    personAddDto.DriversLicense.Number,
                                                                    personAddDto.DriversLicense.State,
                                                                    dateTimeRange)
                                                               .Value;

                    person.SetDriversLicense(driversLicense);
                }

                if (personAddDto?.Phones?.Count > 0)
                    foreach (var phone in personAddDto.Phones)
                        person.AddPhone(new Phone(phone.Number, phone.PhoneType, phone.IsPrimary));

                if (personAddDto?.Emails?.Count > 0)
                    foreach (var email in personAddDto.Emails)
                        person.AddEmail(new Email(email.Address, email.IsPrimary));

                return person;
            }

            return null;
        }
    }
}
