using CustomerVehicleManagement.Domain.BaseClasses;
using Menominee.Common.Enums;
using Menominee.Common.Utilities;
using Menominee.Common.ValueObjects;
using System;
using System.Collections.Generic;

namespace CustomerVehicleManagement.Domain.Entities
{
    public class Person : Contactable
    {
        public Person(PersonName name,
                      Gender gender,
                      Address address,
                      IList<Email> emails,
                      IList<Phone> phones,
                      DateTime? birthday = null,
                      DriversLicense driversLicense = null)
             : base(address, phones, emails)
        {
            Name = name;
            Gender = gender;

            if (birthday.HasValue)
                Birthday = birthday;

            if (driversLicense != null)
                DriversLicense = driversLicense;
        }

        public PersonName Name { get; private set; }
        public Gender Gender { get; private set; }
        public DateTime? Birthday { get; private set; }
        public DriversLicense DriversLicense { get; private set; }

        public void SetName(PersonName name)
        {
            Guard.ForNullOrEmpty(name, "PersonName");
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
            if (driversLicense != null)
                DriversLicense = driversLicense;
        }

        #region ORM

        // EF requires an empty constructor
        protected Person() { }

        #endregion

    }
}
