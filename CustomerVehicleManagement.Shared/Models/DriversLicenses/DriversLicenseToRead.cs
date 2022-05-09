using Menominee.Common.Enums;
using System;

namespace CustomerVehicleManagement.Shared.Models.DriversLicenses
{
    public class DriversLicenseToRead
    {
        public string Number { get; set; }
        public DateTime Issued { get; set; }
        public DateTime Expiry { get; set; }
        public State State { get; set; }
    }
}
