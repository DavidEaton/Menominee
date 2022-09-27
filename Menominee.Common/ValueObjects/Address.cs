using CSharpFunctionalExtensions;
using Menominee.Common.Enums;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

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

        public static readonly string StateInvalidMessage = $"Please enter a valid State";

        public static readonly int PostalCodeMinimumLength = 5;
        public static readonly int PostalCodeMaximumLength = 10;
        public static readonly string PostalCodeMinimumLengthMessage = $"Postal Code cannot be less than {PostalCodeMinimumLength} character(s) in length";
        public static readonly string PostalCodeMaximumLengthMessage = $"Postal Code cannot be over {PostalCodeMaximumLength} characters in length";
        public static readonly string PostalCodeRequiredMessage = $"Postal Code is required";
        public static readonly string PostalCodeInvalidMessage = "Please enter a valid Postal Code";

        // from https://www.oreilly.com/library/view/regular-expressions-cookbook/9781449327453/ch04s14.html
        private static readonly string usPostalCodeRegEx = @"^[0-9]{5}(?:-[0-9]{4})?$";

        public string AddressLine { get; private set; }
        public string City { get; private set; }
        public State State { get; private set; }
        public string PostalCode { get; private set; }

        private Address(string addressLine, string city, State state, string postalCode)
        {
            if (string.IsNullOrWhiteSpace(addressLine))
                throw new ArgumentOutOfRangeException(AddressRequiredMessage);

            addressLine = (addressLine ?? "").Trim();

            if (addressLine.Length < AddressMinimumLength)
                throw new ArgumentOutOfRangeException(AddressMinimumLengthMessage);

            if (addressLine.Length > AddressMaximumLength)
                throw new ArgumentOutOfRangeException(AddressMaximumLengthMessage);

            if (string.IsNullOrWhiteSpace(city))
                throw new ArgumentOutOfRangeException(CityRequiredMessage);

            city = (city ?? "").Trim();

            if (city.Length < CityMinimumLength)
                throw new ArgumentOutOfRangeException(CityMinimumLengthMessage);

            if (city.Length > CityMaximumLength)
                throw new ArgumentOutOfRangeException(CityMaximumLengthMessage);

            if (!Enum.IsDefined(typeof(State), state))
                throw new ArgumentOutOfRangeException(StateInvalidMessage);

            if (string.IsNullOrWhiteSpace(postalCode))
                throw new ArgumentOutOfRangeException(PostalCodeRequiredMessage);

            if (postalCode.Length < PostalCodeMinimumLength)
                throw new ArgumentOutOfRangeException(PostalCodeMinimumLengthMessage);

            if (postalCode.Length > PostalCodeMaximumLength)
                throw new ArgumentOutOfRangeException(PostalCodeMaximumLengthMessage);

            if (!Regex.Match(postalCode, usPostalCodeRegEx).Success)
                throw new ArgumentOutOfRangeException(PostalCodeInvalidMessage);

            AddressLine = addressLine;
            City = city;
            State = state;
            PostalCode = postalCode;
        }

        public static Result<Address> Create(string addressLine, string city, State state, string postalCode)
        {
            if (string.IsNullOrWhiteSpace(addressLine))
                return Result.Failure<Address>(AddressRequiredMessage);

            if (string.IsNullOrWhiteSpace(city))
                return Result.Failure<Address>(CityRequiredMessage);

            if (!Enum.IsDefined(typeof(State), state))
                return Result.Failure<Address>(StateInvalidMessage);

            if (string.IsNullOrWhiteSpace(postalCode))
                return Result.Failure<Address>(PostalCodeRequiredMessage);

            addressLine = (addressLine ?? "").Trim();
            city = (city ?? "").Trim();
            postalCode = (postalCode ?? "").Trim();

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

        public Result<Address> NewAddressLine(string newAddressLine)
        {
            if (string.IsNullOrWhiteSpace(newAddressLine))
                return Result.Failure<Address>(AddressRequiredMessage);

            newAddressLine = (newAddressLine ?? string.Empty).Trim();

            if (newAddressLine.Length < AddressMinimumLength)
                return Result.Failure<Address>(AddressMinimumLengthMessage);

            if (newAddressLine.Length > AddressMaximumLength)
                return Result.Failure<Address>(AddressMaximumLengthMessage);
            
            return Result.Success(new Address(newAddressLine, City, State, PostalCode));
        }

        public Result<Address> NewCity(string newCity)
        {
            if (string.IsNullOrWhiteSpace(newCity))
                return Result.Failure<Address>(CityRequiredMessage);

            newCity = (newCity ?? string.Empty).Trim();

            if (newCity.Length < CityMinimumLength)
                return Result.Failure<Address>(CityMinimumLengthMessage);

            if (newCity.Length > CityMaximumLength)
                return Result.Failure<Address>(CityMaximumLengthMessage);

            return Result.Success(new Address(AddressLine, newCity, State, PostalCode));
        }

        public Result<Address> NewState(State newState)
        {
            if (!Enum.IsDefined(typeof(State), newState))
                return Result.Failure<Address>(StateInvalidMessage);

            return Result.Success(new Address(AddressLine, City, newState, PostalCode));
        }

        public Result<Address> NewPostalCode(string newPostalCode)
        {
            if (string.IsNullOrWhiteSpace(newPostalCode))
                return Result.Failure<Address>(PostalCodeRequiredMessage);
            
            newPostalCode = (newPostalCode ?? string.Empty).Trim();

            if (newPostalCode.Length < PostalCodeMinimumLength)
                return Result.Failure<Address>(PostalCodeMinimumLengthMessage);

            if (newPostalCode.Length > PostalCodeMaximumLength)
                return Result.Failure<Address>(PostalCodeMaximumLengthMessage);

            return Result.Success(new Address(AddressLine, City, State, newPostalCode));
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
