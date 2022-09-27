using Menominee.Common.ValueObjects;

namespace CustomerVehicleManagement.Shared.Models.DriversLicenses
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
                    Expiry = driversLicense.ValidDateRange.End
                };

                return driversLicenseReadDto;
            }
            return null;
        }

    }
}
