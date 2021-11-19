using CustomerVehicleManagement.Domain.Entities;
using Menominee.Common.Enums;
using Menominee.Common.ValueObjects;
using System;
using System.Collections.Generic;

namespace CustomerVehicleManagement.Shared.Models
{
    public class PersonToEdit
    {
        public PersonNameToEdit Name { get; set; }
        public Gender Gender { get; set; }
        public DateTime? Birthday { get; set; }
        public DriversLicenseToEdit DriversLicense { get; set; }
        public Address Address { get; set; }
        public IList<PhoneToEdit> Phones { get; set; } = new List<PhoneToEdit>();
        public IList<EmailToEdit> Emails { get; set; } = new List<EmailToEdit>();
    }
}
