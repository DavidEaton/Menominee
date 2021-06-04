using CustomerVehicleManagement.Api.Emails;
using CustomerVehicleManagement.Api.Phones;
using SharedKernel.Enums;
using SharedKernel.ValueObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CustomerVehicleManagement.Api.Persons
{
    public class PersonCreateDto
    {
        [JsonConstructor]
        public PersonCreateDto(PersonName name, Gender gender)
        {
            Name = name;
            Gender = gender;
        }

        [Required(ErrorMessage = "Person Name is required.")]
        public PersonName Name { get; set; }
        public Gender Gender { get; set; }
        public DateTime? Birthday { get; set; }
        public DriversLicense DriversLicense { get; set; }
        public Address Address { get; set; }
        public IList<PhoneCreateDto> Phones { get; set; } = new List<PhoneCreateDto>();
        public IList<EmailCreateDto> Emails { get; set; } = new List<EmailCreateDto>();
    }
}
