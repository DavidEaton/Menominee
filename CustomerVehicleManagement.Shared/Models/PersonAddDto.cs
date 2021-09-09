using CustomerVehicleManagement.Domain.Entities;
using SharedKernel.Enums;
using SharedKernel.ValueObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CustomerVehicleManagement.Shared.Models
{
    public class PersonAddDto
    {
        [JsonConstructor]
        public PersonAddDto(PersonName name, Gender gender)
        {
            Name = name;
            Gender = gender;
        }

        [Required(ErrorMessage = "Person Name is required.")]
        public PersonName Name { get; set; }
        public Gender Gender { get; set; }
        public DateTime? Birthday { get; set; }
        public DriversLicenseCreateDto DriversLicense { get; set; }
        public Address Address { get; set; }
        public IList<PhoneCreateDto> Phones { get; set; } = new List<PhoneCreateDto>();
        public IList<EmailCreateDto> Emails { get; set; } = new List<EmailCreateDto>();

        public static Person ConvertToEntity(PersonAddDto personAddDto)
        {
            if (personAddDto != null)
            {
                var person = new Person(personAddDto.Name, personAddDto.Gender);

                person.SetAddress(personAddDto?.Address);
                person.SetBirthday(personAddDto?.Birthday);

                if (personAddDto.DriversLicense != null)
                {
                    DateTimeRange dateTimeRange = DateTimeRange.Create(
                                                                    personAddDto.DriversLicense.Issued,
                                                                    personAddDto.DriversLicense.Expiry)
                                                               .Value;

                    DriversLicense driversLicense = SharedKernel.ValueObjects.DriversLicense.Create(
                                                                    personAddDto.DriversLicense.Number,
                                                                    Enum.Parse<State>(personAddDto.DriversLicense.State),
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
