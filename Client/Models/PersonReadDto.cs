using CustomerVehicleManagement.Domain.ValueObjects;
using SharedKernel.Enums;
using System;
using System.Text.Json.Serialization;

namespace Client.Models
{
    public class PersonReadDto
    {
        // Blazor 5 requires public JsonConstructor-attributed contructor, 
        [JsonConstructor]
        public PersonReadDto(PersonName name, Gender gender)
            : this(name, gender, null) { }

        public PersonReadDto(PersonName name, Gender gender, DateTime? birthday)
            : this(name, gender, birthday, null) { }

        public PersonReadDto(PersonName name, Gender gender, DateTime? birthday, Address address)
            : this(name, gender, birthday, address, null) { }

        public PersonReadDto(PersonName name, Gender gender, DateTime? birthday, Address address, DriversLicense driversLicense = null)
        {
            Name = name;
            Gender = gender;
            Birthday = birthday;
            Address = address;
            DriversLicense = driversLicense;
        }

        public int Id { get; set; }
        public PersonName Name { get; set; }
        public Gender Gender { get; set; }
        public DateTime? Birthday { get; set; }
        public DriversLicense DriversLicense { get; set; }
        public Address Address { get; set; }
    }
}
