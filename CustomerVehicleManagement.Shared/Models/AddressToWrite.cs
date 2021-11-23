using Menominee.Common.Enums;
using Menominee.Common.ValueObjects;

namespace CustomerVehicleManagement.Shared.Models
{
    public class AddressToWrite
    {
        public string AddressLine { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public State State { get; set; } = State.MI;
        public string PostalCode { get; set; } = string.Empty;

        public static Address ConvertToEntity(AddressToWrite address)
        {
            if (address != null)
            {
                return Address.Create(address.AddressLine, address.City, address.State, address.PostalCode).Value;
            }

            return null;
        }
    }
}
