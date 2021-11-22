using CustomerVehicleManagement.Domain.Entities;
using Menominee.Common.Enums;
using Menominee.Common.ValueObjects;
using System;
using System.Collections.Generic;

namespace CustomerVehicleManagement.Shared.Models
{
    public class PersonToAdd
    {
        public PersonNameToAdd Name { get; set; }
        public Gender Gender { get; set; }
        public DateTime? Birthday { get; set; }
        public DriversLicenseToAdd DriversLicense { get; set; }
        public AddressToAdd Address { get; set; }
        public IList<PhoneToAdd> Phones { get; set; } = new List<PhoneToAdd>();
        public IList<EmailToAdd> Emails { get; set; } = new List<EmailToAdd>();
    }
}
