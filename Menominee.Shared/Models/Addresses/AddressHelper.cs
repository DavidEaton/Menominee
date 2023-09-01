using Menominee.Common.ValueObjects;

namespace Menominee.Shared.Models.Addresses
{
    public class AddressHelper
    {
        public static AddressToRead ConvertToReadDto(Address address)
        {
            return (address is not null)
                ? new AddressToRead
                {
                    AddressLine1 = address.AddressLine1,
                    AddressLine2 = address.AddressLine2,
                    City = address.City,
                    State = address.State,
                }
                : null;
        }

        public static AddressToWrite ConvertToWriteDto(Address address)
        {
            return (address is not null)
                ? new AddressToWrite()
                {
                    AddressLine1 = address.AddressLine1,
                    AddressLine2 = address.AddressLine2,
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
                    AddressLine1 = address.AddressLine1,
                    AddressLine2 = address.AddressLine2,
                    City = address.City,
                    State = address.State,
                    PostalCode = address.PostalCode
                }
                : null;
        }
    }
}