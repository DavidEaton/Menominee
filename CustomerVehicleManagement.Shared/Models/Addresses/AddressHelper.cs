using Menominee.Common.ValueObjects;

namespace CustomerVehicleManagement.Shared.Models.Addresses
{
    public class AddressHelper
    {
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
