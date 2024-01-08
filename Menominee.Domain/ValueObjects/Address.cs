using CSharpFunctionalExtensions;
using Menominee.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Menominee.Domain.ValueObjects
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

        public static readonly int PostalCodeMinimumLength = 5;
        public static readonly int PostalCodeMaximumLength = 9;
        public static readonly string PostalCodeRequiredMessage = $"Postal Code is required";
        public static readonly string PostalCodeInvalidMessage = "Please enter a valid Postal Code";

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
            addressLine2 = addressLine2 is null || addressLine2 == string.Empty ? null : addressLine2.Trim();
            city = (city ?? string.Empty).Trim();
            postalCode = postalCode?.Trim();

            if (!IsPostalCodeValid(postalCode))
                return Result.Failure<Address>(PostalCodeInvalidMessage);

            if (addressLine1.Length < AddressMinimumLength)
                return Result.Failure<Address>(AddressLengthMessage);

            if (addressLine1.Length > AddressMaximumLength || addressLine2?.Length > AddressMaximumLength)
                return Result.Failure<Address>(AddressLengthMessage);

            if (city.Length < CityMinimumLength)
                return Result.Failure<Address>(CityLengthMessage);

            if (city.Length > CityMaximumLength)
                return Result.Failure<Address>(CityLengthMessage);

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
            return !Enum.IsDefined(typeof(State), newState)
                ? Result.Failure<Address>(StateInvalidMessage)
                : Result.Success(new Address(AddressLine1, City, newState, PostalCode, AddressLine2));
        }

        public Result<Address> NewPostalCode(string newPostalCode)
        {
            if (string.IsNullOrWhiteSpace(newPostalCode))
                return Result.Failure<Address>(PostalCodeRequiredMessage);

            newPostalCode = newPostalCode?.Trim();

            return !IsPostalCodeValid(newPostalCode)
                ? Result.Failure<Address>(PostalCodeInvalidMessage)
                : Result.Success(new Address(AddressLine1, City, State, newPostalCode, AddressLine2));
        }

        public Result<Address> NewAddressLine2(string newAddressLine2)
        {
            newAddressLine2 = (newAddressLine2 ?? string.Empty).Trim();

            return newAddressLine2.Length > AddressMaximumLength
                ? Result.Failure<Address>(AddressLengthMessage)
                : Result.Success(new Address(AddressLine1, City, State, PostalCode, newAddressLine2));
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

        private static bool IsPostalCodeValid(string postalCode)
        {
            return postalCode.Length >= PostalCodeMinimumLength &&
                   postalCode.Length <= PostalCodeMaximumLength &&
                   postalCode.All(char.IsDigit);
        }

        #region ORM

        // EF requires an empty constructor
        protected Address() { }

        #endregion
    }
}
