using CustomerVehicleManagement.Domain.Entities;
using SharedKernel.Enums;
using SharedKernel.ValueObjects;
using System;
using System.Collections.Generic;

namespace CustomerVehicleManagement.Shared.Models
{
    public class PersonReadDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public Gender Gender { get; set; }
        public DateTime? Birthday { get; set; }
        public DriversLicense DriversLicense { get; set; }
        public Address Address { get; set; }
        public IReadOnlyList<PhoneReadDto> Phones { get; set; } = new List<PhoneReadDto>();
        public IReadOnlyList<EmailReadDto> Emails { get; set; } = new List<EmailReadDto>();

        public static PersonReadDto ConvertToDto(Person person)
        {
            return person != null
                ? new PersonReadDto()
                {
                    Id = person.Id,
                    Name = person.Name.LastFirstMiddle,
                    DriversLicense = person.DriversLicense,
                    Address = person?.Address,
                    Birthday = person?.Birthday,
                    Phones = PhoneReadDto.ConvertToDto(person.Phones),
                    Emails = EmailReadDto.ConvertToDto(person.Emails)
                }
                : null;
        }
    }
}
