﻿using SharedKernel.Utilities;
using SharedKernel.ValueObjects;
using System;
using System.Text.Json.Serialization;

namespace CustomerVehicleManagement.Shared.Models
{
    public class DriversLicenseUpdateDto
    {
        public static readonly string DriversLicenseEmptyMessage = "Drivers License number cannot be empty";
        public static readonly string DriversLicenseDateRangeMessage = "Drivers License Expiry date cannot preceed Issued date";

        [JsonConstructor]
        public DriversLicenseUpdateDto(string number, DateTime issued, DateTime expiry, string state)
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
                throw new ArgumentException(DriversLicenseDateRangeMessage, nameof(issued));
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

        public static DriversLicense ConvertToEntity(DriversLicenseUpdateDto driversLicense)
        {
            return driversLicense != null
                ? new DriversLicense(driversLicense.Number, driversLicense.State, new DateTimeRange(driversLicense.Issued, driversLicense.Expiry))
                : null;
        }
    }
}
