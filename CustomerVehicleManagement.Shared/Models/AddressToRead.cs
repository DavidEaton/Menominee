using Menominee.Common.Enums;
using Menominee.Common.ValueObjects;

namespace CustomerVehicleManagement.Shared.Models
{
    public class AddressToRead
    {
        public string AddressLine { get; set; }
        public string City { get; set; }
        public State State { get; set; }
        public string PostalCode { get; set; }
        public string AddressFull { get => string.IsNullOrWhiteSpace(AddressLine) ? $"{string.Empty}" : $"{AddressLine} {City}, {State} {PostalCode}"; }

        public static AddressToRead ConvertToDto(Address address)
        {
            if (address != null)
            {
                var addressReadDto = new AddressToRead
                {
                    AddressLine = address.AddressLine,
                    City = address.City,
                    State = address.State,
                    PostalCode = address.PostalCode
                };

                return addressReadDto;
            }
            return null;
        }
    }
}
