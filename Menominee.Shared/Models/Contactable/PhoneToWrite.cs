using Menominee.Common.Enums;
using Menominee.Domain.Interfaces;

namespace Menominee.Shared.Models.Contactable
{
    public class PhoneToWrite : IHasPrimary
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
