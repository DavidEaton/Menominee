using CustomerVehicleManagement.Domain.Entities;
using Menominee.Common.Enums;
using Menominee.Common.ValueObjects;
using System;
using System.Collections.Generic;

namespace CustomerVehicleManagement.Shared.Models
{
    public class PersonToRead
    {
        public long Id { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public Gender Gender { get; set; }
        public DateTime? Birthday { get; set; }
        public DriversLicenseToRead DriversLicense { get; set; }
        public AddressToRead Address { get; set; }
        public IReadOnlyList<PhoneToRead> Phones { get; set; } = new List<PhoneToRead>();
        public IReadOnlyList<EmailToRead> Emails { get; set; } = new List<EmailToRead>();
        public string LastFirstMiddle
        {
            get => string.IsNullOrWhiteSpace(MiddleName) ? $"{LastName}, {FirstName}" : $"{LastName}, {FirstName} {MiddleName}";
        }
        public string LastFirstMiddleInitial
        {
            get => string.IsNullOrWhiteSpace(MiddleName) ? $"{LastName}, {FirstName}" : $"{LastName}, {FirstName} {MiddleName[0]}.";
        }
        public string FirstMiddleLast
        {
            get => string.IsNullOrWhiteSpace(MiddleName) ? $"{FirstName} {LastName}" : $"{FirstName} {MiddleName} {LastName}";
        }

        public override string ToString()
        {
            return LastFirstMiddleInitial;
        }

        public static PersonToRead ConvertToDto(Person person)
        {
            return person != null
                ? new PersonToRead()
                {
                    Id = person.Id,
                    FirstName = person.Name.FirstName,
                    MiddleName = person.Name.MiddleName,
                    LastName = person.Name.LastName,
                    Gender = person.Gender,
                    DriversLicense = DriversLicenseToRead.ConvertToDto(person.DriversLicense),
                    Address = person?.Address != null
                        ? AddressToRead.ConvertToDto(person.Address)
                        : null,
                    Birthday = person?.Birthday,
                    Phones = PhoneToRead.ConvertToDto(person.Phones),
                    Emails = EmailToRead.ConvertToDto(person.Emails)
                }
                : null;
        }
    }
}
