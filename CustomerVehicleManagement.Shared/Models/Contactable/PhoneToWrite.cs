using Menominee.Common.Enums;

namespace CustomerVehicleManagement.Shared.Models.Contactable
{
    public class PhoneToWrite
    {
        public long Id { get; set; }
        public string Number { get; set; } = string.Empty;
        public PhoneType PhoneType { get; set; } = PhoneType.Other;
        public bool IsPrimary { get; set; } = false;
        public override string ToString()
        {
            return PhoneHelper.FormatPhoneNumber(Number);
        }
    }
}
