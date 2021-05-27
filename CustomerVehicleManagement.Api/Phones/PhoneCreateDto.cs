using SharedKernel.Enums;
using SharedKernel.Utilities;
using System;
using System.Text.Json.Serialization;

namespace CustomerVehicleManagement.Api.Phones
{
    public class PhoneCreateDto
    {
        public static readonly string PhoneEmptyMessage = "Phone number cannot be empty";

        [JsonConstructor]
        public PhoneCreateDto(string number, PhoneType phoneType, bool primary)
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
            Primary = primary;
        }

        [JsonInclude]
        public string Number { get; private set; }
        [JsonInclude]
        public PhoneType PhoneType { get; private set; }
        [JsonInclude]
        public bool Primary { get; private set; }

    }
}
