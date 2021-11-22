using Menominee.Common.Enums;
using Menominee.Common.ValueObjects;
using System;

namespace CustomerVehicleManagement.Shared.Models
{
    public class DriversLicenseToRead
    {
        public string Number { get; set; }
        public DateTime Issued { get; set; }
        public DateTime Expiry { get; set; }
        public State State { get; set; }

        public static DriversLicenseToRead ConvertToDto(DriversLicense driversLicense)
        {
            if (driversLicense != null)
            {
                var driversLicenseReadDto = new DriversLicenseToRead
                {
                    Number = driversLicense.Number,
                    Issued = driversLicense.ValidDateRange.Start,
                    Expiry = driversLicense.ValidDateRange.End
                };

                return driversLicenseReadDto;
            }
            return null;
        }
    }
}
