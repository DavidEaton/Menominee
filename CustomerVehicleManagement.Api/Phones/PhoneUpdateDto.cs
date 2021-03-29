using SharedKernel.Enums;

namespace CustomerVehicleManagement.Api.Phones
{
    public class PhoneUpdateDto
    {
        public int Id { get; set; }
        public string Number { get; set; }
        public PhoneType PhoneType { get; set; }
        public bool IsPrimary { get; set; }
    }
}
