using Menominee.Common.Enums;

namespace CustomerVehicleManagement.Shared.Models
{
    public class AddressToWrite
    {
        public string AddressLine { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public State State { get; set; } = State.MI;
        public string PostalCode { get; set; } = string.Empty;
    }
}
