using Menominee.Common.Utilities;
using Menominee.Common.Enums;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using CSharpFunctionalExtensions;

namespace Menominee.Common.ValueObjects
{
    public class Address : AppValueObject
    {
        public static readonly int AddressMinimumLength = 3;
        public static readonly int AddressMaximumLength = 255;
        public static readonly string AddressMinimumLengthMessage = $"Address cannot be less than {AddressMinimumLength} character(s) in length";
        public static readonly string AddressMaximumLengthMessage = $"Address cannot be over {AddressMaximumLength} characters in length";
        public static readonly string AddressRequiredMessage = $"Address is required";

        public static readonly int CityMinimumLength = 3;
        public static readonly int CityMaximumLength = 255;
        public static readonly string CityMinimumLengthMessage = $"City cannot be less than {CityMinimumLength} character(s) in length";
        public static readonly string CityMaximumLengthMessage = $"City cannot be over {CityMaximumLength} characters in length";
        public static readonly string CityRequiredMessage = $"City is required";

        public static readonly int PostalCodeMinimumLength = 5;
        public static readonly int PostalCodeMaximumLength = 10;
        public static readonly string PostalCodeMinimumLengthMessage = $"Postal Code cannot be less than {PostalCodeMinimumLength} character(s) in length";
        public static readonly string PostalCodeMaximumLengthMessage = $"Postal Code cannot be over {PostalCodeMaximumLength} characters in length";
        public static readonly string PostalCodeRequiredMessage = $"Postal Code is required";
        public static readonly string PostalCodeInvalidMessage = "Please enter a valid Postal Code";

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
            // from http://unicode.org/Public/cldr/26.0.1/
            var usPostalCodeRegEx = @"\d{5}([ \-]\d{4})?";

            if (string.IsNullOrWhiteSpace(addressLine))
                return Result.Failure<Address>(AddressRequiredMessage);

            if (string.IsNullOrWhiteSpace(city))
                return Result.Failure<Address>(CityRequiredMessage);

            if (string.IsNullOrWhiteSpace(postalCode))
                return Result.Failure<Address>(PostalCodeRequiredMessage);

            addressLine = addressLine.Trim();
            city = city.Trim();
            postalCode = postalCode.Trim();

            if (addressLine.Length < AddressMinimumLength)
                return Result.Failure<Address>(AddressMinimumLengthMessage);

            if (addressLine.Length > AddressMaximumLength)
                return Result.Failure<Address>(AddressMaximumLengthMessage);

            if (city.Length < CityMinimumLength)
                return Result.Failure<Address>(CityMinimumLengthMessage);

            if (city.Length > CityMaximumLength)
                return Result.Failure<Address>(CityMaximumLengthMessage);

            if (postalCode.Length < PostalCodeMinimumLength)
                return Result.Failure<Address>(PostalCodeMinimumLengthMessage);

            if (postalCode.Length > PostalCodeMaximumLength)
                return Result.Failure<Address>(PostalCodeMaximumLengthMessage);

            if (!Regex.Match(postalCode, usPostalCodeRegEx).Success)
                return Result.Failure<Address>(PostalCodeInvalidMessage);

            return Result.Success(new Address(addressLine, city, state, postalCode));
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

        public string AddressFull { get => string.IsNullOrWhiteSpace(AddressLine) ? $"{string.Empty}" : $"{AddressLine} {City}, {State} {PostalCode}"; }

        #region ORM

        // EF requires an empty constructor
        protected Address() { }

        #endregion
    }
}
