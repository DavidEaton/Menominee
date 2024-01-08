using Menominee.Domain.Enums;

namespace Menominee.Shared.Models.Addresses
{
    public class AddressToWrite
    {
        public string AddressLine1 { get; set; } = string.Empty;
        public string AddressLine2 { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public State State { get; set; }
        public string PostalCode { get; set; } = string.Empty;
        public bool IsEmpty =>
            string.IsNullOrWhiteSpace(AddressLine1) &&
            string.IsNullOrWhiteSpace(AddressLine2) &&
            string.IsNullOrWhiteSpace(City) &&
            string.IsNullOrWhiteSpace(PostalCode);
        public bool IsNotEmpty => !IsEmpty;
    }
}
