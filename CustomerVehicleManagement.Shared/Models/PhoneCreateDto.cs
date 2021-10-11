﻿using CustomerVehicleManagement.Domain.Entities;
using SharedKernel.Enums;

namespace CustomerVehicleManagement.Shared.Models
{
    public class PhoneCreateDto
    {
        public string Number { get; set; } = string.Empty;
        public PhoneType PhoneType { get; set; } = PhoneType.Unknown;
        public bool IsPrimary { get; set; } = false;

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
