using CustomerVehicleManagement.Domain.Entities;
using SharedKernel.Enums;
using System.Collections.Generic;

namespace CustomerVehicleManagement.Shared.Models
{
    public class PhoneUpdateDto
    {
        public string Number { get; set; } = string.Empty;
        public PhoneType PhoneType { get; set; } = PhoneType.Other;
        public bool IsPrimary { get; set; } = false;

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
