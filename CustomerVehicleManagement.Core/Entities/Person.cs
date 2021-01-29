using SharedKernel;
using SharedKernel.Enums;
using SharedKernel.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CustomerVehicleManagement.Domain.Entities
{
    public class Person : Entity
    {
        // Blazor 5 requires public JsonConstructor-attributed contructor, 
        [JsonConstructor]
        public Person(PersonName name, Gender gender)
            : this(name, gender, null) { }

        public Person(PersonName name, Gender gender, DateTime? birthday)
            : this(name, gender, birthday, null) { }

        public Person(PersonName name, Gender gender, DateTime? birthday, Address address)
            : this(name, gender, birthday, address, null) { }

        public Person(PersonName name, Gender gender, DateTime? birthday, Address address, DriversLicense driversLicense = null)
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

        public void AddPhone(Phone phone)
        {
            Phones.Add(phone);
        }

        public void RemovePhone(Phone phone)
        {
            Phones.Remove(phone);
        }
        #region ORM

        // EF requires an empty constructor
        protected Person() { }

        #endregion

    }
}
