using SharedKernel.Enums;
using SharedKernel.Utilities;
using System;
using System.Text.Json.Serialization;

namespace CustomerVehicleManagement.Api.Phones
{
    public class PhoneUpdateDto
    {
        public static readonly string PhoneEmptyMessage = "Phone number cannot be empty";

        public string Number { get; set; }
        public PhoneType PhoneType { get; set; }
        public bool IsPrimary { get; set; }

        [JsonConstructor]
        public PhoneUpdateDto(string number, PhoneType phoneType, bool isPrimary)
        {
            try
            {
                Guard.ForNullOrEmpty(number, "number");

            }
            catch (Exception)
            {
                throw new ArgumentException(PhoneEmptyMessage);
            }

            Number = number;
            PhoneType = phoneType;
            IsPrimary = isPrimary;
        }
    }
}
