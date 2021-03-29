using SharedKernel.Enums;
using SharedKernel.Utilities;
using System;

namespace CustomerVehicleManagement.Api.Phones
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

        public string Number { get; set; }
        public PhoneType PhoneType { get; set; }
        public bool Primary { get; set; }

    }
}
