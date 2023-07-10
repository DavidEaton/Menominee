using Menominee.Shared.Models.Addresses;
using Menominee.Shared.Models.Contactable;
using Menominee.Shared.Models.DriversLicenses;
using Menominee.Shared.Models.Persons.PersonNames;
using Menominee.Common.Enums;
using System;
using System.Collections.Generic;

namespace Menominee.Shared.Models.Persons
{
    public class PersonToRead
    {
        public long Id { get; set; }
        public PersonNameToRead Name { get; set; }
        public Gender Gender { get; set; }
        public DateTime? Birthday { get; set; }
        public DriversLicenseToRead DriversLicense { get; set; }
        public AddressToRead Address { get; set; }
        public IList<PhoneToRead> Phones { get; set; } = new List<PhoneToRead>();
        public IList<EmailToRead> Emails { get; set; } = new List<EmailToRead>();
        public string LastFirstMiddle
        {
            get => string.IsNullOrWhiteSpace(Name.MiddleName) ? $"{Name.LastName}, {Name.FirstName}" : $"{Name.LastName}, {Name.FirstName} {Name.MiddleName}";
        }
        public string LastFirstMiddleInitial
        {
            get => string.IsNullOrWhiteSpace(Name.MiddleName) ? $"{Name.LastName}, {Name.FirstName}" : $"{Name.LastName}, {Name.FirstName} {Name.MiddleName[0]}.";
        }
        public string FirstMiddleLast
        {
            get => string.IsNullOrWhiteSpace(Name.MiddleName) ? $"{Name.FirstName} {Name.LastName}" : $"{Name.FirstName} {Name.MiddleName} {Name.LastName}";
        }

        public override string ToString()
        {
            return LastFirstMiddleInitial;
        }
    }
}