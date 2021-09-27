using SharedKernel.Enums;

namespace CustomerVehicleManagement.Shared.Models
{
    public class AddressAddDto
    {
        public string AddressLine { get; set; }
        public string City { get; set; }
        public State State { get; set; }
        public string PostalCode { get; set; }
    }
}
