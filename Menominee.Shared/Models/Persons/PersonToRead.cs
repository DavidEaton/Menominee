using Menominee.Common.Enums;
using Menominee.Shared.Models.Addresses;
using Menominee.Shared.Models.Contactable;
using Menominee.Shared.Models.DriversLicenses;
using Menominee.Shared.Models.Persons.PersonNames;
using System;
using System.Collections.Generic;

namespace Menominee.Shared.Models.Persons
{
    public class PersonToRead
    {
        public long Id { get; set; } = default;
        public PersonNameToRead Name { get; set; } = new();
        public Gender Gender { get; set; } = Gender.Other;
        public DateTime? Birthday { get; set; }
        public DriversLicenseToRead DriversLicense { get; set; } = new();
        public AddressToRead Address { get; set; } = new();
        public List<PhoneToRead> Phones { get; set; } = new List<PhoneToRead>();
        public List<EmailToRead> Emails { get; set; } = new List<EmailToRead>();
    }
}