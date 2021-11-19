using CustomerVehicleManagement.Domain.Entities;
using Menominee.Common.Enums;
using Helper = CustomerVehicleManagement.Shared.Helpers.PhoneHelper;

namespace CustomerVehicleManagement.Shared.Models
{
    public class PhoneToAdd
    {
        public string Number { get; set; } = string.Empty;
        public PhoneType PhoneType { get; set; } = PhoneType.Unknown;
        public bool IsPrimary { get; set; } = false;
        public override string ToString()
        {
            return Helper.FormatPhoneNumber(Number);
        }
    }
}
