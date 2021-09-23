using SharedKernel.Enums;

namespace CustomerVehicleManagement.Shared.Models
{
    public class AddressUpdateDto
    {
        public string AddressLine { get; set; }
        public string City { get; set; }
        public State State { get; set; }
        public string PostalCode { get; set; }
    }
}
