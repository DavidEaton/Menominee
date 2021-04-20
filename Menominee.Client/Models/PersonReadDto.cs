using CustomerVehicleManagement.Domain.Entities;
using SharedKernel.Enums;
using SharedKernel.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Menominee.Client.Models
{
    public class PersonReadDto
    {
        // Blazor 5 requires public JsonConstructor-attributed contructor, 
        [JsonConstructor]
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
        public IList<Phone> Phones { get; set; } = new List<Phone>();
    }

}
