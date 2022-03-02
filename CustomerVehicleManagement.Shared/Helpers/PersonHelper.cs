using CustomerVehicleManagement.Domain.Entities;
using CustomerVehicleManagement.Shared.Models;
using Menominee.Common.Enums;
using Menominee.Common.ValueObjects;
using System;
using System.Collections.Generic;

namespace CustomerVehicleManagement.Shared.Helpers
{
    public class PersonHelper
    {
        public static Person CreateEntityFromWriteDto(PersonToWrite person)
        {
            Address address = null;
            List<Phone> phones = new();
            List<Email> emails = new();
            DriversLicense driversLicense = null;

            var personName = PersonName.Create(person.Name.LastName, person.Name.FirstName, person.Name.MiddleName).Value;

            if (person?.Address != null)
                address = Address.Create(person.Address.AddressLine, person.Address.City, person.Address.State, person.Address.PostalCode).Value;

            if (person?.Phones.Count > 0)
                foreach (var phone in person.Phones)
                    phones.Add(Phone.Create(phone.Number, phone.PhoneType, phone.IsPrimary).Value);

            if (person?.Emails.Count > 0)
                foreach (var email in person.Emails)
                    emails.Add(Email.Create(email.Address, email.IsPrimary).Value);

            if (person?.DriversLicense != null)
            {
                DateTimeRange dateTimeRange = DateTimeRange.Create(
                    person.DriversLicense.Issued,
                    person.DriversLicense.Expiry).Value;

                driversLicense = DriversLicense.Create(person.DriversLicense.Number,
                    person.DriversLicense.State,
                    dateTimeRange).Value;
            }

            return new(personName, person.Gender, address, emails, phones, person.Birthday, driversLicense);

        }
        public static PersonToWrite CreateWriteDtoFromReadDto(PersonToRead person)
        {
            PersonToWrite Person = new()
            {
                Name = new()
                {
                    LastName = person.LastName,
                    MiddleName = person.MiddleName,
                    FirstName = person.FirstName
                },

                Gender = person.Gender,
                Birthday = person?.Birthday
            };

            if (person?.Address is not null)
                Person.Address = new()
                {
                    AddressLine = person.Address.AddressLine,
                    City = person.Address.City,
                    State = person.Address.State,
                    PostalCode = person.Address.PostalCode
                };

            if (person?.DriversLicense is not null)
                Person.DriversLicense = new()
                {
                    Number = person.DriversLicense.Number,
                    State = person.DriversLicense.State,
                    Issued = person.DriversLicense.Issued,
                    Expiry = person.DriversLicense.Expiry
                };

            if (person?.Phones.Count > 0)
            {
                foreach (var phone in person.Phones)
                {
                    Person.Phones.Add(new()
                    {
                        Number = phone.Number,
                        PhoneType = Enum.Parse<PhoneType>(phone.PhoneType),
                        IsPrimary = phone.IsPrimary
                    });
                }
            }

            if (person?.Emails.Count > 0)
            {
                foreach (var email in person.Emails)
                {
                    Person.Emails.Add(new()
                    {
                        Address = email.Address,
                        IsPrimary = email.IsPrimary
                    });
                }
            }

            return Person;
        }
    }
}
