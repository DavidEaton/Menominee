using Menominee.Common.ValueObjects;
using Menominee.Domain.Entities;
using Menominee.Shared.Models.Addresses;
using Menominee.Shared.Models.Contactable;
using Menominee.Shared.Models.DriversLicenses;
using Menominee.Shared.Models.Persons.PersonNames;
using System.Collections.Generic;
using System.Linq;

namespace Menominee.Shared.Models.Persons
{
    public class PersonHelper
    {
        public static PersonToRead ConvertToReadDto(Person person)
        {
            return person is null
                ? null
                : new()
                {
                    Id = person.Id,
                    Name = new PersonNameToRead()
                    {
                        FirstName = person.Name.FirstName,
                        LastName = person.Name.LastName,
                        MiddleName = person.Name?.MiddleName
                    },
                    Gender = person.Gender,
                    DriversLicense = DriversLicenseHelper.ConvertToReadDto(person.DriversLicense),
                    Address =
                        person?.Address is not null
                        ? AddressHelper.ConvertToReadDto(person.Address)
                        : null,
                    Birthday = person?.Birthday,
                    Phones = PhoneHelper.ConvertToReadDtos(person.Phones),
                    Emails = EmailHelper.ConvertToReadDtos(person.Emails)
                };
        }

        public static PersonToWrite ConvertToWriteDto(Person person)
        {
            return person is null
                ? null
                : new()
                {
                    Id = person.Id,
                    Name = PersonNameHelper.ConvertToWriteDto(person.Name),
                    Gender = person.Gender,
                    DriversLicense = DriversLicenseHelper
                        .ConvertToWriteDto(person.DriversLicense),
                    Address = person.Address is not null
                        ? AddressHelper.ConvertToWriteDto(person.Address)
                        : null,
                    Birthday = person.Birthday,
                    Phones = PhoneHelper.ConvertToWriteDtos(person.Phones),
                    Emails = EmailHelper.ConvertToWriteDtos(person.Emails)
                };
        }

        public static Person ConvertWriteDtoToEntity(PersonToWrite person)
        {
            if (person is null)
                return null;

            Address address = null;
            DriversLicense driversLicense = null;
            List<Phone> phones = new();
            List<Email> emails = new();

            var personName = PersonName.Create(person.Name.LastName, person.Name.FirstName, person.Name.MiddleName).Value;

            if (person?.Address is not null)
                address = Address.Create(person.Address.AddressLine1, person.Address.City, person.Address.State, person.Address.PostalCode, person.Address.AddressLine2).Value;

            if (person?.Phones.Count > 0)
                foreach (var phone in person.Phones)
                    phones.Add(Phone.Create(phone.Number, phone.PhoneType, phone.IsPrimary).Value);

            if (person?.Emails.Count > 0)
                foreach (var email in person.Emails)
                    emails.Add(Email.Create(email.Address, email.IsPrimary).Value);

            if (person?.DriversLicense is not null)
            {
                DateTimeRange dateTimeRange = DateTimeRange.Create(
                    person.DriversLicense.Issued,
                    person.DriversLicense.Expiry).Value;

                driversLicense = DriversLicense.Create(person.DriversLicense.Number,
                    person.DriversLicense.State,
                    dateTimeRange).Value;
            }

            return Person.Create(
                personName,
                person.Gender,
                person.Notes,
                person.Birthday,
                emails,
                phones,
                address,
                driversLicense)
                .Value;
        }

        public static PersonToWrite ConvertReadToWriteDto(PersonToRead person)
        {
            if (person is null)
                return new();

            PersonToWrite Person = new()
            {
                Id = person.Id,
                Name = new()
                {
                    LastName = person.Name.LastName,
                    MiddleName = person.Name?.MiddleName,
                    FirstName = person.Name.FirstName
                },

                Gender = person.Gender,
                Birthday = person?.Birthday
            };
            if (person?.Address is not null)
            {
                Person.Address = new()
                {
                    AddressLine1 = person.Address.AddressLine1,
                    City = person.Address.City,
                    State = person.Address.State,
                    PostalCode = person.Address.PostalCode,
                    AddressLine2 = person.Address.AddressLine2,
                };
            }

            if (person?.DriversLicense is not null)
            {
                Person.DriversLicense = new()
                {
                    Number = person.DriversLicense.Number,
                    State = person.DriversLicense.State,
                    Issued = person.DriversLicense.Issued,
                    Expiry = person.DriversLicense.Expiry
                };
            }

            if (person?.Phones.Count > 0)
            {
                foreach (var phone in person.Phones)
                {
                    Person.Phones.Add(new()
                    {
                        Number = phone.Number,
                        PhoneType = phone.PhoneType,
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

        public static PersonToReadInList ConvertToReadInListDto(Person person)
        {
            return person is null
                ? null
                : new()
                {
                    AddressLine1 = person?.Address?.AddressLine1,
                    AddressLine2 = person?.Address?.AddressLine2,
                    Birthday = person?.Birthday,
                    City = person?.Address?.City,
                    Id = person.Id,
                    Name = person.Name.LastFirstMiddle,
                    PostalCode = person?.Address?.PostalCode,
                    State = person?.Address?.State.ToString(),
                    PrimaryPhone = PhoneHelper.GetPrimaryPhone(person) ?? PhoneHelper.GetOrdinalPhone(person, 0),
                    PrimaryPhoneType = PhoneHelper.GetPrimaryPhoneType(person) ?? PhoneHelper.GetOrdinalPhoneType(person, 0),
                    PrimaryEmail = EmailHelper.GetPrimaryEmail(person) ?? EmailHelper.GetOrdinalEmail(person, 0)
                };
        }

        internal static PersonToWrite ConvertToWriteDto(PersonToRead person)
        {
            return person is null
                ? null
                : new()
                {
                    Id = person.Id,
                    Address = AddressHelper.ConvertReadToWriteDto(person.Address),
                    DriversLicense = DriversLicenseHelper.ConvertReadToWriteDto(person.DriversLicense),
                    Birthday = person?.Birthday,
                    Name = PersonNameHelper.ConvertReadToWriteDto(person.Name),

                    Phones = (List<PhoneToWrite>)person.Phones.Select(phone => new PhoneToWrite()
                    {
                        Number = phone.Number,
                        PhoneType = phone.PhoneType,
                        IsPrimary = phone.IsPrimary
                    }),
                    Emails = (List<EmailToWrite>)person.Emails.Select(email => new EmailToWrite()
                    {
                        Address = email.Address,
                        IsPrimary = email.IsPrimary
                    }),
                };
        }
    }
}
