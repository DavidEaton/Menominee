using Menominee.Common.Enums;

namespace CustomerVehicleManagement.Shared.Models.Contactable
{
    public class PhoneToRead
    {
        public long Id { get; set; }
        public string Number { get; set; }
        public PhoneType PhoneType { get; set; }
        public bool IsPrimary { get; set; }
    }
}
