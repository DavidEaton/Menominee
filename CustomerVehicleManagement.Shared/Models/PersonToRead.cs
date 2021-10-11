using CustomerVehicleManagement.Domain.Entities;
using SharedKernel.Enums;
using System;
using System.Collections.Generic;

namespace CustomerVehicleManagement.Shared.Models
{
    public class PersonToRead
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public Gender Gender { get; set; }
        public DateTime? Birthday { get; set; }
        public DriversLicenseToRead DriversLicense { get; set; }
        public AddressToRead Address { get; set; }
        public IReadOnlyList<PhoneToRead> Phones { get; set; } = new List<PhoneToRead>();
        public IReadOnlyList<EmailToRead> Emails { get; set; } = new List<EmailToRead>();

        public static PersonToRead ConvertToDto(Person person)
        {
            return person != null
                ? new PersonToRead()
                {
                    Id = person.Id,
                    Name = person.Name.LastFirstMiddle,
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
