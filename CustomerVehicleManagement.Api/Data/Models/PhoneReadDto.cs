using SharedKernel.Enums;

namespace CustomerVehicleManagement.Api.Data.Models
{
    public class PhoneReadDto
    {
        public int Id { get; set; }
        public string Number { get; set; }
        public PhoneType PhoneType { get; set; }
        public bool Primary { get; set; }

    }
}
