using CustomerVehicleManagement.Domain.Entities;
using Menominee.Common.Enums;
using System.Collections.Generic;
using Helper = CustomerVehicleManagement.Shared.Helpers.PhoneHelper;

namespace CustomerVehicleManagement.Shared.Models
{
    public class PhoneToEdit
    {
        public string Number { get; set; } = string.Empty;
        public PhoneType PhoneType { get; set; } = PhoneType.Other;
        public bool IsPrimary { get; set; } = false;
        public override string ToString()
        {
            return Helper.FormatPhoneNumber(Number);
        }

        public static IList<Phone> ConvertToEntities(IList<PhoneToEdit> phones)
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
