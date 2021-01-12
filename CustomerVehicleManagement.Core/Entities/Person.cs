using CustomerVehicleManagement.Domain.Enums;
using CustomerVehicleManagement.Domain.ValueObjects;
using SharedKernel;
using System;

namespace CustomerVehicleManagement.Domain.Entities
{
    public class Person : Entity
    {
        public Person(PersonName name, Gender gender)
            : this(name, gender, null) { }

        public Person(PersonName name, Gender gender, DateTime? birthday)
            : this(name, gender, birthday, null) { }

        public Person(PersonName name, Gender gender, DateTime? birthday, Address address)
            : this(name, gender, birthday, address, null) { }

        public Person(PersonName name, Gender gender, DateTime? birthday, Address address, DriversLicence driversLicence = null)
        {
            Name = name;
            Gender = gender;
            Birthday = birthday;
            Address = address;
            DriversLicence = driversLicence;
        }

        public PersonName Name { get; set; }
        public Gender Gender { get; set; }
        public DateTime? Birthday { get; set; }
        public DriversLicence DriversLicence { get; set; }
        public Address Address { get; set; }

        #region ORM

        // EF requires an empty constructor
        protected Person() { }

        #endregion

    }
}
