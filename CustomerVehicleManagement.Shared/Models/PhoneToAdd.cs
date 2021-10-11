using CustomerVehicleManagement.Domain.Entities;
using Menominee.Common.Enums;

namespace CustomerVehicleManagement.Shared.Models
{
    public class PhoneToAdd
    {
        public string Number { get; set; } = string.Empty;
        public PhoneType PhoneType { get; set; } = PhoneType.Unknown;
        public bool IsPrimary { get; set; } = false;

        public static Phone ConvertToEntity(PhoneToAdd phone)
        {
            if (phone != null)
            {
                return new Phone(phone.Number, phone.PhoneType, phone.IsPrimary);
            }

            return null;
        }
    }
}
