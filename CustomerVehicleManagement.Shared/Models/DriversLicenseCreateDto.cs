using SharedKernel.Utilities;
using System;
using System.Text.Json.Serialization;

namespace CustomerVehicleManagement.Shared.Models
{
    public class DriversLicenseCreateDto
    {
        public static readonly string DriversLicenseEmptyMessage = "Drivers License number cannot be empty";
        public static readonly string DriversLicenseDateRangeMessage = "Drivers License Expiry date cannot preceed Issued date";
        public const string DriversLicenseErrorMessage = "Email address and/or its format is invalid";

        [JsonConstructor]
        public DriversLicenseCreateDto(string number, DateTime issued, DateTime expiry, string state)
        {
            try
            {
                Guard.ForNullOrEmpty(number, "number");
            }
            catch (Exception)
            {
                throw new ArgumentException(DriversLicenseEmptyMessage, nameof(number));
            }

            try
            {
                Guard.ForPrecedesDate(issued, expiry, "validRange");
            }
            catch (Exception)
            {
                throw new ArgumentException(DriversLicenseEmptyMessage, nameof(issued));
            }

            Number = number;
            Issued = issued;
            Expiry = expiry;
            State = state;
        }
        public string Number { get; set; }
        public DateTime Issued { get; set; }
        public DateTime Expiry { get; set; }
        public string State { get; set; }
    }
}
