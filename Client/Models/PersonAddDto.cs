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
        {
            Name = name;
            Gender = gender;
            Birthday = birthday;
            Address = address;
        }

        public PersonAddDto(PersonName name, Gender gender, DateTime? birthday, string addressLine, string city, string state, string postalCode)
        {
            Name = name;
            Gender = gender;
            Birthday = birthday;
            Address = new Address(addressLine, city, state, postalCode);
        }

        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }

        internal PersonName Name { get; set; }
        public Gender Gender { get; set; }
        public DateTime? Birthday { get; set; }
        public string AddressLine { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        internal Address Address { get; set; }
    }
}
