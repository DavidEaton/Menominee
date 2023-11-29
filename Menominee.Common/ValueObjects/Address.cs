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
        public static readonly string AddressLengthMessage = $"Address must be between {AddressMinimumLength} and {AddressMaximumLength} character(s) in length";
        public static readonly string AddressRequiredMessage = $"Address is required";

        public static readonly int CityMinimumLength = 3;
        public static readonly int CityMaximumLength = 255;
        public static readonly string CityLengthMessage = $"City must be between {CityMinimumLength} and {CityMaximumLength} character(s) in length";
        public static readonly string CityRequiredMessage = $"City is required";

        public static readonly string StateInvalidMessage = $"Please enter a valid State";

        public static readonly int PostalCodeMaximumLength = 9;
        public static readonly string PostalCodeRequiredMessage = $"Postal Code is required";
        public static readonly string PostalCodeInvalidMessage = "Please enter a valid Postal Code";

        // from https://www.oreilly.com/library/view/regular-expressions-cookbook/9781449327453/ch04s14.html
        private static readonly string usPostalCodeRegEx = @"^[0-9]{5}(?:-[0-9]{4})?$";

        public string AddressLine1 { get; private set; }
        public string AddressLine2 { get; private set; }
        public string City { get; private set; }
        public State State { get; private set; }
        public string PostalCode { get; private set; }

        private Address(string addressLine1, string city, State state, string postalCode, string addressLine2 = null)
        {
            AddressLine1 = addressLine1;
            AddressLine2 = addressLine2;
            City = city;
            State = state;
            PostalCode = postalCode;
        }

        public static Result<Address> Create(string addressLine1, string city, State state, string postalCode, string addressLine2 = null)
        {
            if (string.IsNullOrWhiteSpace(addressLine1))
                return Result.Failure<Address>(AddressRequiredMessage);

            if (string.IsNullOrWhiteSpace(city))
                return Result.Failure<Address>(CityRequiredMessage);

            if (!Enum.IsDefined(typeof(State), state))
                return Result.Failure<Address>(StateInvalidMessage);

            if (string.IsNullOrWhiteSpace(postalCode))
                return Result.Failure<Address>(PostalCodeRequiredMessage);

            addressLine1 = (addressLine1 ?? string.Empty).Trim();
            addressLine2 = (addressLine2 is null || addressLine2 == string.Empty) ? null : addressLine2.Trim();
            city = (city ?? string.Empty).Trim();
            postalCode = (postalCode ?? string.Empty).Trim();

            if (addressLine1.Length < AddressMinimumLength)
                return Result.Failure<Address>(AddressLengthMessage);

            if (addressLine1.Length > AddressMaximumLength || addressLine2?.Length > AddressMaximumLength)
                return Result.Failure<Address>(AddressLengthMessage);

            if (city.Length < CityMinimumLength)
                return Result.Failure<Address>(CityLengthMessage);

            if (city.Length > CityMaximumLength)
                return Result.Failure<Address>(CityLengthMessage);

            if (!Regex.Match(postalCode, usPostalCodeRegEx).Success)
                return Result.Failure<Address>(PostalCodeInvalidMessage);

            return Result.Success(new Address(addressLine1, city, state, postalCode, addressLine2));
        }
        public Result<Address> NewAddressLine1(string newAddressLine)
        {
            newAddressLine = (newAddressLine ?? string.Empty).Trim();

            if (newAddressLine.Length < AddressMinimumLength)
                return Result.Failure<Address>(AddressLengthMessage);

            if (newAddressLine.Length > AddressMaximumLength)
                return Result.Failure<Address>(AddressLengthMessage);

            return Result.Success(new Address(newAddressLine, City, State, PostalCode, AddressLine2));
        }

        public Result<Address> NewCity(string newCity)
        {
            if (string.IsNullOrWhiteSpace(newCity))
                return Result.Failure<Address>(CityRequiredMessage);

            newCity = (newCity ?? string.Empty).Trim();

            if (newCity.Length < CityMinimumLength)
                return Result.Failure<Address>(CityLengthMessage);

            if (newCity.Length > CityMaximumLength)
                return Result.Failure<Address>(CityLengthMessage);

            return Result.Success(new Address(AddressLine1, newCity, State, PostalCode, AddressLine2));
        }

        public Result<Address> NewState(State newState)
        {
            if (!Enum.IsDefined(typeof(State), newState))
                return Result.Failure<Address>(StateInvalidMessage);

            return Result.Success(new Address(AddressLine1, City, newState, PostalCode, AddressLine2));
        }

        public Result<Address> NewPostalCode(string newPostalCode)
        {
            newPostalCode = (newPostalCode ?? string.Empty).Trim();

            if (!Regex.Match(newPostalCode, usPostalCodeRegEx).Success)
                return Result.Failure<Address>(PostalCodeInvalidMessage);

            return Result.Success(new Address(AddressLine1, City, State, newPostalCode, AddressLine2));
        }

        public Result<Address> NewAddressLine2(string newAddressLine2)
        {
            newAddressLine2 = (newAddressLine2 ?? string.Empty).Trim();

            if (newAddressLine2.Length > AddressMaximumLength)
                return Result.Failure<Address>(AddressLengthMessage);

            return Result.Success(new Address(AddressLine1, City, State, PostalCode, newAddressLine2));

        }

        public override string ToString()
        {
            return AddressFull;
        }

        protected override IEnumerable<IComparable> GetEqualityComponents()
        {
            yield return AddressLine1;
            yield return City;
            yield return State;
            yield return PostalCode;
            yield return AddressLine2 ?? string.Empty;
        }
        public string AddressFull
        {
            get => string.IsNullOrWhiteSpace(AddressLine1) ? $"{string.Empty}" :
                string.IsNullOrWhiteSpace(AddressLine2) ?
                    $"{AddressLine1} {City}, {State} {PostalCode}" : $"{AddressLine1}, {AddressLine2}, {City}, {State} {PostalCode}";
        }

        #region ORM

        // EF requires an empty constructor
        protected Address() { }

        #endregion
    }
}
