using SharedKernel.Utilities;
using SharedKernel.Enums;
using System.Collections.Generic;

namespace SharedKernel.ValueObjects
{
    public class Address : ValueObject
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

        public string AddressLine { get; private set; }
        public string City { get; private set; }
        public State State { get; private set; }
        public string PostalCode { get; private set; }

        private Address(string addressLine, string city, State state, string postalCode)
        {
            AddressLine = addressLine;
            City = city;
            State = state;
            PostalCode = postalCode;
        }

        public static Result<Address> Create(string addressLine, string city, State state, string postalCode)
        {
            addressLine = (addressLine ?? string.Empty).Trim();
            city = (city ?? string.Empty).Trim();
            postalCode = (postalCode ?? string.Empty).Trim();

            if (addressLine.Length < AddressMinimumLength)
                return Result.Fail<Address>(AddressUnderMinimumLengthMessage);

            if (addressLine.Length > AddressMaximumLength)
                return Result.Fail<Address>(AddressOverMaximumLengthMessage);

            if (city.Length < CityMinimumLength)
                return Result.Fail<Address>(CityUnderMinimumLengthMessage);

            if (city.Length > CityMaximumLength)
                return Result.Fail<Address>(CityOverMaximumLengthMessage);

            if (postalCode.Length < PostalCodeMinimumLength)
                return Result.Fail<Address>(PostalCodeUnderMinimumLengthMessage);

            if (postalCode.Length > PostalCodeMaximumLength)
                return Result.Fail<Address>(PostalCodeOverMaximumLengthMessage);

            return Result.Ok(new Address(addressLine, city, state, postalCode));
        }

        public Address NewAddressLine(string newAddressLine)
        {
            newAddressLine = (newAddressLine ?? string.Empty).Trim();
            Guard.ForNullOrEmpty(newAddressLine, "newAddressLine");
            return new Address(newAddressLine, City, State, PostalCode);
        }

        public Address NewCity(string newCity)
        {
            newCity = (newCity ?? string.Empty).Trim();
            Guard.ForNullOrEmpty(newCity, "newCity");
            return new Address(AddressLine, newCity, State, PostalCode);
        }

        public Address NewState(State newState)
        {
            return new Address(AddressLine, City, newState, PostalCode);
        }

        public Address NewPostalCode(string newPostalCode)
        {
            newPostalCode = (newPostalCode ?? string.Empty).Trim();
            Guard.ForNullOrEmpty(newPostalCode, "newPostalCode");
            return new Address(AddressLine, City, State, newPostalCode);
        }

        public override string ToString()
        {
            return AddressFull;
        }
        
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return AddressLine;
            yield return City;
            yield return State;
            yield return PostalCode;
        }

        public string AddressFull { get => $"{AddressLine} {City}, {State} {PostalCode}"; }

        #region ORM

        // EF requires an empty constructor
        protected Address() { }

        #endregion
    }
}
