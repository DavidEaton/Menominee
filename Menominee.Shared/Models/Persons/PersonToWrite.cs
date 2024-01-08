using Menominee.Shared.Models.Addresses;
using Menominee.Shared.Models.Contactable;
using Menominee.Shared.Models.Persons.DriversLicenses;
using System;
using System.Collections.Generic;

namespace Menominee.Shared.Models.Persons
{
    public class PersonToWrite
    {
        public long Id { get; set; }
        public PersonNameToWrite Name { get; set; } = new();
        public DateTime? Birthday { get; set; }
        public string Notes { get; set; } = string.Empty;
        public DriversLicenseToWrite DriversLicense { get; set; } = new();
        public AddressToWrite Address { get; set; } = new();
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
