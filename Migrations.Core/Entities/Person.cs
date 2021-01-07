using Migrations.Core.Enums;
using Migrations.Core.Interfaces;
using Migrations.Core.ValueObjects;
using SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Migrations.Core.Entities
{
    public class Person : Entity
    {
        public Person(PersonName name, Gender gender)
            : this(name, gender, null) { }
        public Person(PersonName name, Gender gender, IList<Phone> phones)
            : this(name, gender, phones, null) { }
        public Person(PersonName name, Gender gender, IList<Phone> phones, DateTime? birthday)
            : this(name, gender, phones, birthday, null) { }
        public Person(PersonName name, Gender gender, IList<Phone> phones, DateTime? birthday, Address address)
            : this(name, gender, phones, birthday, address, null) { }

        public Person(PersonName name, Gender gender, IList<Phone> phones, DateTime? birthday, Address address, DriversLicence driversLicence = null)
        {
            Name = name;
            Gender = gender;
            if (phones != null)
                phonesOfPerson = ConvertPhonesOfPerson(phones);
            Birthday = birthday;
            Address = address;
            DriversLicence = driversLicence;
        }

        public PersonName Name { get; set; }
        public Gender Gender { get; set; }
        public DateTime? Birthday { get; set; }
        public DriversLicence DriversLicence { get; set; }
        public Address Address { get; set; }

        private IList<PhoneOfPerson> phonesOfPerson;

        public IReadOnlyList<Phone> Phones =>
            phonesOfPerson
                .Select(x => x.Phone)
                .ToList();

        public void AddPhone(Phone phone)
        {
            phonesOfPerson.Add(new PhoneOfPerson(phone, this));
        }

        private IList<PhoneOfPerson> ConvertPhonesOfPerson(IList<Phone> phones)
        {
            var result = new List<PhoneOfPerson>();
            foreach (var phone in phones)
            {
                var phoneOfPerson = new PhoneOfPerson(phone, this);
                result.Add(phoneOfPerson);
            }

            return result;
        }

        #region ORM

        // EF requires an empty constructor
        protected Person() { }

        #endregion

    }
}
