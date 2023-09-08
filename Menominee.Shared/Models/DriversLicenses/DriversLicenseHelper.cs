using Menominee.Common.ValueObjects;

namespace Menominee.Shared.Models.DriversLicenses
{
    public class DriversLicenseHelper
    {
        public static DriversLicenseToRead ConvertToReadDto(DriversLicense driversLicense)
        {
            if (driversLicense is not null)
            {
                var driversLicenseReadDto = new DriversLicenseToRead
                {
                    Number = driversLicense.Number,
                    Issued = driversLicense.ValidDateRange.Start,
                    Expiry = driversLicense.ValidDateRange.End,
                    State = driversLicense.State,
                };

                return driversLicenseReadDto;
            }
            return null;
        }

        public static DriversLicenseToWrite ConvertToWriteDto(DriversLicense driversLicense)
        {
            return driversLicense is null
                ? null
                : new()
                {
                    Number = driversLicense.Number,
                    Issued = driversLicense.ValidDateRange.Start,
                    Expiry = driversLicense.ValidDateRange.End,
                    State = driversLicense.State,
                };
        }

        internal static DriversLicenseToWrite ConvertReadToWriteDto(DriversLicenseToRead driversLicense)
        {
            return driversLicense is null
                ? null
                : new()
                {
                    Number = driversLicense.Number,
                    Issued = driversLicense.Issued,
                    Expiry = driversLicense.Expiry,
                    State = driversLicense.State,
                };
        }
    }
}
