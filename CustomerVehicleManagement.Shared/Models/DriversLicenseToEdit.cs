using Menominee.Common.Enums;
using Menominee.Common.ValueObjects;
using System;

namespace CustomerVehicleManagement.Shared.Models
{
    public class DriversLicenseToEdit
    {
        public string Number { get; set; } = string.Empty;
        public DateTime Issued { get; set; }
        public DateTime Expiry { get; set; }
        public State State { get; set; }

        public static DriversLicense ConvertToEntity(DriversLicenseToEdit driversLicense)
        {
            return driversLicense != null
                ? DriversLicense.Create(
                    driversLicense.Number,
                    driversLicense.State,
                    DateTimeRange.Create(driversLicense.Issued,
                    driversLicense.Expiry)
                    .Value)
                .Value
                : null;
        }
    }
}
