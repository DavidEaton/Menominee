using Menominee.Common.ValueObjects;

namespace CustomerVehicleManagement.Shared.Models.Addresses
{
    public class AddressHelper
    {
        public static AddressToRead ConvertEntityToReadDto(Address address)
        {
            return (address is not null)
                ? new AddressToRead
                {
                    AddressLine = address.AddressLine,
                    City = address.City,
                    State = address.State,
                }
                : null;
        }

        public static AddressToWrite ConvertEntityToWriteDto(Address address)
        {
            return (address is not null)
                ? new AddressToWrite()
                {
                    AddressLine = address.AddressLine,
                    City = address.City,
                    State = address.State,
                    PostalCode = address.PostalCode
                }
                : null;
        }

        public static AddressToWrite ConvertReadToWriteDto(AddressToRead address)
        {
            return (address is not null)
                ? new AddressToWrite()
                {
                    AddressLine = address.AddressLine,
                    City = address.City,
                    State = address.State,
                    PostalCode = address.PostalCode
                }
                : null;
        }
    }
}