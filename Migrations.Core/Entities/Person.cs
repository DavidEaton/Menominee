using Migrations.Core.Enums;
using Migrations.Core.Interfaces;
using Migrations.Core.ValueObjects;
using SharedKernel;
using System;
using System.Collections.Generic;

namespace Migrations.Core.Entities
{
    public class Person : Entity, IPerson
    {
        // EF requires an empty constructor
        protected Person() { }

        public Person(PersonName name, DriversLicence driversLicence = null, Address address = null, List<Phone> phones = null)
        {
            Name = name;
            Phones = phones;
            DriversLicence = driversLicence;
            Address = address;
        }
        public PersonName Name { get; set; }
        public DateTime? Birthday { get; set; }
        public Gender Gender { get; set; }
        public DriversLicence DriversLicence { get; set; }
        public Address Address { get; set; }
        public IList<Phone> Phones { get; set; }
    }
}
