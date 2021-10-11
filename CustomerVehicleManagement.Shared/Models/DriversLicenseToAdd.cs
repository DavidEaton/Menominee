using Menominee.Common.Enums;
using System;

namespace CustomerVehicleManagement.Shared.Models
{
    public class DriversLicenseToAdd
    {
        public string Number { get; set; } = string.Empty;
        public DateTime Issued { get; set; }
        public DateTime Expiry { get; set; }
        public State State { get; set; }
    }
}
