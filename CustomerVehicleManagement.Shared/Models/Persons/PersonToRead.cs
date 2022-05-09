using CustomerVehicleManagement.Shared.Models.Addresses;
using CustomerVehicleManagement.Shared.Models.Contactable;
using CustomerVehicleManagement.Shared.Models.DriversLicenses;
using Menominee.Common.Enums;
using System;
using System.Collections.Generic;

namespace CustomerVehicleManagement.Shared.Models.Persons
{
    public class PersonToRead
    {
        public long Id { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public Gender Gender { get; set; }
        public DateTime? Birthday { get; set; }
        public DriversLicenseToRead DriversLicense { get; set; }
        public AddressToRead Address { get; set; }
        public IReadOnlyList<PhoneToRead> Phones { get; set; } = new List<PhoneToRead>();
        public IReadOnlyList<EmailToRead> Emails { get; set; } = new List<EmailToRead>();
        public string LastFirstMiddle
        {
            get => string.IsNullOrWhiteSpace(MiddleName) ? $"{LastName}, {FirstName}" : $"{LastName}, {FirstName} {MiddleName}";
        }
        public string LastFirstMiddleInitial
        {
            get => string.IsNullOrWhiteSpace(MiddleName) ? $"{LastName}, {FirstName}" : $"{LastName}, {FirstName} {MiddleName[0]}.";
        }
        public string FirstMiddleLast
        {
            get => string.IsNullOrWhiteSpace(MiddleName) ? $"{FirstName} {LastName}" : $"{FirstName} {MiddleName} {LastName}";
        }

        public override string ToString()
        {
            return LastFirstMiddleInitial;
        }
    }
}
