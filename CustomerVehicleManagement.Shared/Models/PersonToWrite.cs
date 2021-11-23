using CustomerVehicleManagement.Domain.Entities;
using Menominee.Common.Enums;
using Menominee.Common.ValueObjects;
using System;
using System.Collections.Generic;

namespace CustomerVehicleManagement.Shared.Models
{
    public class PersonToWrite
    {
        public PersonNameToWrite Name { get; set; }
        public Gender Gender { get; set; }
        public DateTime? Birthday { get; set; }
        public DriversLicenseToWrite DriversLicense { get; set; }
        public AddressToWrite Address { get; set; }
        public IList<PhoneToWrite> Phones { get; set; } = new List<PhoneToWrite>();
        public IList<EmailToWrite> Emails { get; set; } = new List<EmailToWrite>();
    }
}
