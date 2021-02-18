using CustomerVehicleManagement.Api.Data.ValidationAttributes;
using SharedKernel.Enums;
using SharedKernel.ValueObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CustomerVehicleManagement.Api.Data.Models
{
    [PersonCanHaveOnlyOnePrimaryPhoneAttribute]
    public class PersonCreateDto
    {
        public PersonCreateDto(PersonName name, Gender gender)
            : this(name, gender, null) { }

        public PersonCreateDto(PersonName name, Gender gender, DateTime? birthday)
            : this(name, gender, birthday, null) { }

        public PersonCreateDto(PersonName name, Gender gender, DateTime? birthday, Address address)
            : this(name, gender, birthday, address, null) { }

        [JsonConstructor]
        public PersonCreateDto(PersonName name, Gender gender, DateTime? birthday, Address address, DriversLicense driversLicense = null)
        {
            Name = name;
            Gender = gender;
            Birthday = birthday;
            Address = address;
            DriversLicense = driversLicense;
        }
        public int Id { get; set; }
        
        [Required]
        public PersonName Name { get; set; }
        public Gender Gender { get; set; }
        public DateTime? Birthday { get; set; }
        public DriversLicense DriversLicense { get; set; }
        public Address Address { get; set; }
        public ICollection<PhoneCreateDto> Phones { get; set; } = new List<PhoneCreateDto>();
    }
}
