using Menominee.Domain.Entities;
using Menominee.Domain.ValueObjects;
using Menominee.Shared.Models.Addresses;
using Menominee.Shared.Models.Contactable;
using Menominee.Shared.Models.Persons.DriversLicenses;
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
                    DriversLicense = DriversLicenseHelper.ConvertToReadDto(person?.DriversLicense),
                    Address =
                        person?.Address is not null
                        ? AddressHelper.ConvertToReadDto(person?.Address)
                        : null,
                    Birthday = person?.Birthday,
                    Notes = person?.Notes,
                    Phones = PhoneHelper.ConvertToReadDtos(person?.Phones),
                    Emails = EmailHelper.ConvertToReadDtos(person?.Emails)
                };
        }

        public static PersonToWrite ConvertToWriteDto(Person person)
        {
            return person is null
                ? new()
                : new()
                {
                    Id = person.Id,
                    Name = PersonNameHelper.ConvertToWriteDto(person?.Name),
                    DriversLicense = person.DriversLicense is not null
                        ? DriversLicenseHelper
                        .ConvertToWriteDto(person?.DriversLicense)
                        : null,
                    Address = person?.Address is not null
                        ? AddressHelper.ConvertToWriteDto(person?.Address)
                        : null,
                    Birthday = person.Birthday,
                    Notes = person.Notes,
                    Phones = PhoneHelper.ConvertToWriteDtos(person?.Phones),
                    Emails = EmailHelper.ConvertToWriteDtos(person?.Emails)
                };
        }

        public static Person ConvertWriteDtoToEntity(PersonToWrite personDto)
        {
            if (personDto is null)
                return null;

            Address address = null;
            DriversLicense driversLicense = null;
            List<Phone> phones = new();
            List<Email> emails = new();

            var personName = PersonName.Create(personDto.Name.LastName, personDto.Name.FirstName, personDto.Name.MiddleName).Value;

            if (personDto.Address is not null)
                address = Address.Create(personDto.Address.AddressLine1, personDto.Address.City, personDto.Address.State, personDto.Address.PostalCode, personDto.Address.AddressLine2).Value;

            if (personDto?.Phones.Count > 0)
                foreach (var phone in personDto.Phones)
                    phones.Add(Phone.Create(phone.Number, phone.PhoneType, phone.IsPrimary).Value);

            if (personDto?.Emails.Count > 0)
                foreach (var email in personDto.Emails)
                    emails.Add(Email.Create(email.Address, email.IsPrimary).Value);

            if (personDto?.DriversLicense is not null)
            {
                DateTimeRange dateTimeRange = DateTimeRange.Create(
                    personDto.DriversLicense.Issued,
                    personDto.DriversLicense.Expiry).Value;

                driversLicense = DriversLicense.Create(personDto.DriversLicense.Number,
                    personDto.DriversLicense.State,
                    dateTimeRange).Value;
            }

            return Person.Create(
                personName,
                personDto.Notes,
                personDto.Birthday,
                emails,
                phones,
                address,
                driversLicense)
                .Value;
        }

        public static PersonToWrite ConvertReadToWriteDto(PersonToRead person)
        {
            return person is null
                ? new()
                : new()
                {
                    Id = person.Id,
                    Name = new()
                    {
                        LastName = person.Name.LastName,
                        MiddleName = person.Name?.MiddleName,
                        FirstName = person.Name.FirstName
                    },

                    Birthday = person?.Birthday,
                    Notes = person?.Notes,
                    Address = person.Address is not null
                ? new()
                {
                    AddressLine1 = person.Address.AddressLine1,
                    City = person.Address.City,
                    State = person.Address.State,
                    PostalCode = person.Address.PostalCode,
                    AddressLine2 = person.Address.AddressLine2,
                }
                : new(),

                    DriversLicense = person?.DriversLicense is not null
                ? new DriversLicenseToWrite
                {
                    Number = person.DriversLicense.Number,
                    State = person.DriversLicense.State,
                    Issued = person.DriversLicense.Issued,
                    Expiry = person.DriversLicense.Expiry
                }
                : new(),

                    Phones = person?.Phones
                ?.Select(phone => new PhoneToWrite
                {
                    Number = phone.Number,
                    PhoneType = phone.PhoneType,
                    IsPrimary = phone.IsPrimary
                })
                .ToList() ?? new(),

                    Emails = person?.Emails
                ?.Select(email => new EmailToWrite
                {
                    Address = email.Address,
                    IsPrimary = email.IsPrimary
                })
                .ToList() ?? new()
                };
        }

        public static PersonToReadInList ConvertToReadInListDto(Person person)
        {
            if (person == null)
            {
                return null;
            }

            var dto = new PersonToReadInList
            {
                Id = person.Id,
                Name = person.Name.LastFirstMiddle,
                AddressLine1 = person.Address?.AddressLine1,
                AddressLine2 = person.Address?.AddressLine2,
                City = person.Address?.City,
                State = person.Address?.State.ToString(),
                PostalCode = person.Address?.PostalCode,
                Birthday = person.Birthday,
                PrimaryPhone = GetPrimaryPhoneString(person),
                PrimaryPhoneType = GetPrimaryPhoneTypeString(person),
                PrimaryEmail = GetPrimaryEmailString(person)
            };

            return dto;
        }

        private static string GetPrimaryPhoneString(Person person) =>
            PhoneHelper.GetPrimaryPhone(person) switch
            {
                // If primary phone retrieval is successful and the phone is not null
                { IsSuccess: true, Value: var phone } when phone is not null => phone.ToString(),

                // Fallback to ordinal phone if primary phone retrieval is unsuccessful
                _ => PhoneHelper.GetOrdinalPhone(person, 0)?.ToString() ?? string.Empty
            };

        private static string GetPrimaryPhoneTypeString(Person person) =>
            PhoneHelper.GetPrimaryPhone(person) is { IsSuccess: true, Value: var phone } && phone is not null
                ? phone.PhoneType.ToString()
                : PhoneHelper.GetOrdinalPhone(person, 0) is { } ordinalPhone
                    ? ordinalPhone.PhoneType.ToString()
                    : string.Empty;

        private static string GetPrimaryEmailString(Person person)
        {
            return EmailHelper.GetPrimaryEmail(person) ?? EmailHelper.GetOrdinalEmail(person, 0);
        }


        internal static PersonToWrite ConvertToWriteDto(PersonToRead person)
        {
            return person is null
                ? null
                : new()
                {
                    Id = person.Id,
                    Address = AddressHelper.ConvertReadToWriteDto(person?.Address),
                    DriversLicense = DriversLicenseHelper.ConvertReadToWriteDto(person?.DriversLicense),
                    Birthday = person?.Birthday,
                    Name = PersonNameHelper.ConvertReadToWriteDto(person?.Name),
                    Notes = person?.Notes,
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

        internal static PersonToRead ConvertWriteToReadDto(PersonToWrite person)
        {
            return person is null
                ? null
                : new()
                {
                    Id = person.Id,
                    Address = AddressHelper.ConvertWriteToReadDto(person?.Address),
                    DriversLicense = DriversLicenseHelper.ConvertWriteToReadDto(person?.DriversLicense),
                    Birthday = person?.Birthday,
                    Name = PersonNameHelper.ConvertWriteToReadDto(person?.Name),
                    Notes = person?.Notes,
                    Phones = (List<PhoneToRead>)person.Phones.Select(phone => new PhoneToRead()
                    {
                        Number = phone.Number,
                        PhoneType = phone.PhoneType,
                        IsPrimary = phone.IsPrimary
                    }),
                    Emails = (List<EmailToRead>)person.Emails.Select(email => new EmailToRead()
                    {
                        Address = email.Address,
                        IsPrimary = email.IsPrimary
                    }),
                };
        }
    }
}
