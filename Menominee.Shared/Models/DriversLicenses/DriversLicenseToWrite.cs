using Menominee.Common.Enums;
using System;

namespace Menominee.Shared.Models.DriversLicenses
{
    public class DriversLicenseToWrite
    {
        public string Number { get; set; } = string.Empty;
        public DateTime Issued { get; set; }
        public DateTime Expiry { get; set; }
        public State State { get; set; }
        public bool IsEmpty =>
            string.IsNullOrWhiteSpace(Number) &&
            Issued.Equals(DateTime.MinValue) &&
            Expiry.Equals(DateTime.MinValue);
        public bool IsNotEmpty => !IsEmpty;
    }
}
