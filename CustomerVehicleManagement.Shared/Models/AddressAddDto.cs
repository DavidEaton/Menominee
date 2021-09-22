using SharedKernel.Enums;
using SharedKernel.Utilities;
using System;
using System.Text.Json.Serialization;

namespace CustomerVehicleManagement.Shared.Models
{
    public class AddressAddDto
    {
        public static readonly int AddressMinimumLength = 3;
        public static readonly int AddressMaximumLength = 255;
        public static readonly string AddressUnderMinimumLengthMessage = $"Address cannot be less than {AddressMinimumLength} character(s) in length";
        public static readonly string AddressOverMaximumLengthMessage = $"Address cannot be over {AddressMaximumLength} characters in length";

        public static readonly int CityMinimumLength = 3;
        public static readonly int CityMaximumLength = 255;
        public static readonly string CityUnderMinimumLengthMessage = $"City cannot be less than {CityMinimumLength} character(s) in length";
        public static readonly string CityOverMaximumLengthMessage = $"City cannot be over {CityMaximumLength} characters in length";

        public static readonly int PostalCodeMinimumLength = 5;
        public static readonly int PostalCodeMaximumLength = 32;
        public static readonly string PostalCodeUnderMinimumLengthMessage = $"Postal Code cannot be less than {PostalCodeMinimumLength} character(s) in length";
        public static readonly string PostalCodeOverMaximumLengthMessage = $"Postal Code cannot be over {PostalCodeMaximumLength} characters in length";
        public static readonly string StateEmptyMessage = "State cannot be empty";

        [JsonConstructor]
        public AddressAddDto(string addressLine, string city, State state, string postalCode)
        {
            try
            {
                Guard.ForNullOrEmpty(addressLine, "addressLine");
            }
            catch (Exception)
            {
                throw new ArgumentException(AddressUnderMinimumLengthMessage, nameof(addressLine));
            }

            try
            {
                Guard.ForNullOrEmpty(city, "city");
            }
            catch (Exception)
            {
                throw new ArgumentException(CityUnderMinimumLengthMessage, nameof(city));
            }

            try
            {
                Guard.ForNullOrEmpty(postalCode, "postalCode");
            }
            catch (Exception)
            {
                throw new ArgumentException(PostalCodeUnderMinimumLengthMessage, nameof(postalCode));
            }

            AddressLine = addressLine;
            City = city;
            State = state;
            PostalCode = postalCode;
        }

        public string AddressLine { get; set; }
        public string City { get; set; }
        public State State { get; set; }
        public string PostalCode { get; set; }
    }
}
