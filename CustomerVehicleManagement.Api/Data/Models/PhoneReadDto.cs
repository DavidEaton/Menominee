using SharedKernel.Enums;

namespace CustomerVehicleManagement.Api.Data.Models
{
    public class PhoneReadDto
    {
        public int Id { get; set; }
        public string Number { get; private set; }
        public PhoneType PhoneType { get; private set; }
        public bool Primary { get; private set; }

    }
}
