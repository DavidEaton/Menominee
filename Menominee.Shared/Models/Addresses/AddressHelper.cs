using Menominee.Common.ValueObjects;

namespace Menominee.Shared.Models.Addresses
{
    public class AddressHelper
    {
        public static AddressToRead ConvertToReadDto(Address address)
        {
            return (address is not null)
                ? new()
                {
                    AddressLine1 = address.AddressLine1,
                    AddressLine2 = address.AddressLine2,
                    City = address.City,
                    State = address.State,
                    PostalCode = address.PostalCode
                }
                : new();
        }

        public static AddressToWrite ConvertToWriteDto(Address address)
        {
            return (address is not null)
                ? new()
                {
                    AddressLine1 = address.AddressLine1,
                    AddressLine2 = address.AddressLine2,
                    City = address.City,
                    State = address.State,
                    PostalCode = address.PostalCode
                }
                : new();
        }

        public static AddressToWrite ConvertReadToWriteDto(AddressToRead address)
        {
            return (address is not null)
                ? new()
                {
                    AddressLine1 = address.AddressLine1,
                    AddressLine2 = address.AddressLine2,
                    City = address.City,
                    State = address.State,
                    PostalCode = address.PostalCode
                }
                : new();
        }

        internal static AddressToRead ConvertWriteToReadDto(AddressToWrite address)
        {
            return (address is not null)
                ? new()
                {
                    AddressLine1 = address.AddressLine1,
                    AddressLine2 = address.AddressLine2,
                    City = address.City,
                    State = address.State,
                    PostalCode = address.PostalCode
                }
                : new();
        }
    }
}