﻿using CustomerVehicleManagement.Domain.Entities;
using SharedKernel.Enums;
using SharedKernel.Utilities;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CustomerVehicleManagement.Shared.Models
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

        public static IList<Phone> ConvertToEntities(IList<PhoneUpdateDto> phones)
        {
            var phoneEntities = new List<Phone>();

            if (phones != null)
            {
                foreach (var phone in phones)
                    phoneEntities.Add(new Phone(phone.Number, phone.PhoneType, phone.IsPrimary));
            }

            return phoneEntities;
        }

    }
}
