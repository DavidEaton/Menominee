using CustomerVehicleManagement.Domain.ValueObjects;
using SharedKernel.Enums;
using System;

namespace Client.Models
{
    public class PersonAddDto
    {
        public PersonAddDto(PersonName name, Gender gender)
            : this(name, gender, null) { }

        public PersonAddDto(PersonName name, Gender gender, DateTime? birthday)
            : this(name, gender, birthday, null) { }

        public PersonAddDto(PersonName name, Gender gender, DateTime? birthday, Address address)
            : this(name, gender, birthday, address, null) { }

        public PersonAddDto(PersonName name, Gender gender, DateTime? birthday, Address address, DriversLicense driversLicense = null)
        {
            Name = name;
            Gender = gender;
            Birthday = birthday;
            Address = address;
            DriversLicense = driversLicense;
        }
        public PersonName Name { get; set; }
        public Gender Gender { get; set; }
        public DateTime? Birthday { get; set; }
        public DriversLicense DriversLicense { get; set; }
        public Address Address { get; set; }

    }
}
