using CustomerVehicleManagement.Domain.BaseClasses;
using SharedKernel.Enums;
using SharedKernel.ValueObjects;
using System;

namespace CustomerVehicleManagement.Domain.Entities
{
    public class Person : Contactable
    {
        public Person(PersonName name, Gender gender)
        {
            Name = name;
            Gender = gender;
        }

        public PersonName Name { get; private set; }
        public Gender Gender { get; private set; }
        public DateTime? Birthday { get; private set; }
        public DriversLicense DriversLicense { get; private set; }
        public Address Address { get; private set; }

        public void SetName(PersonName name)
        {
            Name = name;
        }

        public void SetGender(Gender gender)
        {
            Gender = gender;
        }

        public void SetBirthday(DateTime? birthday)
        {
            if (birthday.HasValue)
                Birthday = birthday;
        }

        public void SetDriversLicense(DriversLicense driversLicense)
        {
            DriversLicense = driversLicense;
        }

        public void SetAddress(Address address)
        {
            Address = address;
        }

        #region ORM

        // Code that pollutes our domain class (very minor impact in this case), but
        // is necessary for EntityFramework, makes our model <100% persistence ignorant.

        // EF requires an empty constructor
        protected Person() { }

        #endregion

    }
}
