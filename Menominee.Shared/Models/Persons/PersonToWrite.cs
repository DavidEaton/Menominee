using Menominee.Common.Enums;
using Menominee.Shared.Models.Addresses;
using Menominee.Shared.Models.Contactable;
using Menominee.Shared.Models.DriversLicenses;
using System;
using System.Collections.Generic;

namespace Menominee.Shared.Models.Persons
{
    public class PersonToWrite
    {
        public long Id { get; set; }
        public PersonNameToWrite Name { get; set; }
        public Gender Gender { get; set; }
        public DateTime? Birthday { get; set; }
        public string Notes { get; set; }
        public DriversLicenseToWrite DriversLicense { get; set; }
        public AddressToWrite Address { get; set; }
        public List<PhoneToWrite> Phones { get; set; } = new List<PhoneToWrite>();
        public List<EmailToWrite> Emails { get; set; } = new List<EmailToWrite>();
        public bool IsEmpty =>
            Id == 0 &&
            (Name is null ||
                (string.IsNullOrWhiteSpace(Name.FirstName) &&
                 string.IsNullOrWhiteSpace(Name.MiddleName) &&
                 string.IsNullOrWhiteSpace(Name.LastName))) &&
            Birthday is null &&
            string.IsNullOrWhiteSpace(Notes) &&
            ((DriversLicense is null) || DriversLicense.IsEmpty) &&
            ((Address is null) || Address.IsEmpty) &&
            Phones.Count == 0 &&
            Emails.Count == 0;
        public bool IsNotEmpty => !IsEmpty;
    }
}
