using Menominee.Domain.Enums;

namespace Menominee.Shared.Models.Addresses
{
    public class AddressToRead
    {
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; } = string.Empty;
        public string City { get; set; }
        public State State { get; set; }
        public string PostalCode { get; set; }
        public string AddressFull
        {
            get => string.IsNullOrWhiteSpace(AddressLine1) ? $"{string.Empty}" :
                string.IsNullOrWhiteSpace(AddressLine2) ?
                    $"{AddressLine1} {City}, {State} {PostalCode}" : $"{AddressLine1}, {AddressLine2}, {City}, {State} {PostalCode}";
        }

    }
}
