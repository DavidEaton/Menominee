using Menominee.Common.Enums;

namespace CustomerVehicleManagement.Shared.Models.Addresses
{
    public class AddressToRead
    {
        public string AddressLine { get; set; }
        public string City { get; set; }
        public State State { get; set; }
        public string PostalCode { get; set; }
        public string AddressFull { get => string.IsNullOrWhiteSpace(AddressLine) ? $"{string.Empty}" : $"{AddressLine} {City}, {State} {PostalCode}"; }
    }
}
