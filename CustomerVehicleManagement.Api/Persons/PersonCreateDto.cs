using CustomerVehicleManagement.Api.Emails;
using CustomerVehicleManagement.Api.Phones;
using CustomerVehicleManagement.Api.ValidationAttributes;
using SharedKernel.Enums;
using SharedKernel.ValueObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CustomerVehicleManagement.Api.Persons
{
    [PersonCanHaveOnlyOnePrimaryPhone(ErrorMessage = "Person can have only one Primary phone.")]
    public class PersonCreateDto
    {
        [JsonConstructor]
        public PersonCreateDto(PersonName name, Gender gender)
        {
            Name = name;
            Gender = gender;
        }
        public int Id { get; set; }
        
        [Required(ErrorMessage = "Person Name is required.")]
        public PersonName Name { get; set; }
        public Gender Gender { get; set; }
        public DateTime? Birthday { get; set; }
        public DriversLicense DriversLicense { get; set; }
        public Address Address { get; set; }
        public ICollection<PhoneCreateDto> Phones { get; set; } = new List<PhoneCreateDto>();
        public ICollection<EmailCreateDto> Emails { get; set; } = new List<EmailCreateDto>();
    }
}
