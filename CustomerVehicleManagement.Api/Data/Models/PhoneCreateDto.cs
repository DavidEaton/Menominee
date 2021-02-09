using SharedKernel.Enums;
using SharedKernel.Utilities;
using System;

namespace CustomerVehicleManagement.Api.Data.Models
{
    public class PhoneCreateDto
    {
        public static readonly string PhoneEmptyMessage = "Phone number cannot be empty";
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

        public string Number { get; private set; }
        public PhoneType PhoneType { get; private set; }
        public bool Primary { get; private set; }

    }
}
