using Menominee.Domain.ValueObjects;

namespace Menominee.Shared.Models.Persons.DriversLicenses
{
    public class DriversLicenseHelper
    {
        public static DriversLicenseToRead ConvertToReadDto(DriversLicense driversLicense)
        {
            return driversLicense is null
                ? null
                : new()
                {
                    Number = driversLicense.Number,
                    Issued = driversLicense.ValidDateRange.Start,
                    Expiry = driversLicense.ValidDateRange.End,
                    State = driversLicense.State
                };
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
                    State = driversLicense.State
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
                    State = driversLicense.State
                };
        }

        internal static DriversLicenseToRead ConvertWriteToReadDto(DriversLicenseToWrite driversLicense)
        {
            return driversLicense is null
                ? null
                : new()
                {
                    Number = driversLicense.Number,
                    Issued = driversLicense.Issued,
                    Expiry = driversLicense.Expiry,
                    State = driversLicense.State
                };
        }
    }
}
