using CustomerVehicleManagement.Domain.Entities;
using SharedKernel.Enums;
using SharedKernel.ValueObjects;
using System;
using System.Collections.Generic;

namespace Menominee.Client.Models
{
    public class PersonCreateDto
    {
        public PersonCreateDto(PersonName name, Gender gender)
            : this(name, gender, null) { }

        public PersonCreateDto(PersonName name, Gender gender, DateTime? birthday)
            : this(name, gender, birthday, null) { }

        public PersonCreateDto(PersonName name, Gender gender, DateTime? birthday, Address address)
            : this(name, gender, birthday, address, null) { }

        public PersonCreateDto(PersonName name, Gender gender, DateTime? birthday, Address address, DriversLicense driversLicense = null)
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
        public IList<Phone> Phones { get; set; } = new List<Phone>();

    }

}
