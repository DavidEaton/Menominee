using CustomerVehicleManagement.Api.ValidationAttributes;
using CustomerVehicleManagement.Domain.BaseClasses;
using SharedKernel.Enums;
using SharedKernel.ValueObjects;
using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CustomerVehicleManagement.Api.Persons
{
    [ContactableCanHaveOnlyOnePrimaryPhone(ErrorMessage = "Can have only one Primary phone.")]
    [ContactableCanHaveOnlyOnePrimaryEmail(ErrorMessage = "Can have only one Primary email.")]
    public class PersonCreateDto : Contactable
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
        //public ICollection<PhoneCreateDto> Phones { get; set; } = new List<PhoneCreateDto>();
        //public ICollection<EmailCreateDto> Emails { get; set; } = new List<EmailCreateDto>();
    }
}
