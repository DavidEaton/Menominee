using Menominee.Common.Enums;
using System;

namespace CustomerVehicleManagement.Shared.Models.DriversLicenses
{
    public class DriversLicenseToWrite
    {
        public string Number { get; set; } = string.Empty;
        public DateTime Issued { get; set; }
        public DateTime Expiry { get; set; }
        public State State { get; set; }
    }
}
