using CustomerVehicleManagement.Domain.Entities;
using SharedKernel.Enums;
using SharedKernel.Utilities;
using System;
using System.Text.Json.Serialization;

namespace CustomerVehicleManagement.Shared.Models
{
    public class PhoneCreateDto
    {
        public static readonly string PhoneEmptyMessage = "Phone number cannot be empty";

        [JsonConstructor]
        public PhoneCreateDto(string number, PhoneType phoneType, bool isPrimary)
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

        public string Number { get; private set; }
        public PhoneType PhoneType { get; private set; }
        public bool IsPrimary { get; private set; }

        public static Phone ConvertToEntity(PhoneCreateDto phone)
        {
            if (phone != null)
            {
                return new Phone(phone.Number, phone.PhoneType, phone.IsPrimary);
            }

            return null;
        }
    }
}
