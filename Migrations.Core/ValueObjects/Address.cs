using SharedKernel.ValueObjects;
using System;
using System.Collections.Generic;

namespace Migrations.Core.ValueObjects
{
    public class Address : ValueObject
    {
        public static readonly string AddressEmptyMessage = "Address details cannot be empty";
        public Address(string addressLine, string city, string state, string postalCode, string countryCode)
        {
            if (string.IsNullOrWhiteSpace(addressLine) | string.IsNullOrWhiteSpace(city) | string.IsNullOrWhiteSpace(state) | string.IsNullOrWhiteSpace(postalCode) | string.IsNullOrWhiteSpace(countryCode))
            {
                throw new ArgumentException(AddressEmptyMessage);
            }

            AddressLine = addressLine;
            City = city;
            State = state;
            PostalCode = postalCode;
            CountryCode = countryCode;
        }

        public string AddressLine { get; }
        public string City { get; }
        public string State { get; }
        public string PostalCode { get; }
        public string CountryCode { get; }

        public Address NewAddressLine(string newAddressLine)
        {
            return new Address(newAddressLine, City, State, PostalCode, CountryCode);
        }

        public Address NewCity(string newCity)
        {
            return new Address(AddressLine, newCity, State, PostalCode, CountryCode);
        }

        public Address NewState(string newState)
        {
            return new Address(AddressLine, City, newState, PostalCode, CountryCode);
        }

        public Address NewPostalCode(string newPostalCode)
        {
            return new Address(AddressLine, City, State, newPostalCode, CountryCode);
        }

        public Address NewCountryCode(string newCountryCode)
        {
            return new Address(AddressLine, City, State, PostalCode, newCountryCode);
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return AddressLine;
            yield return City;
            yield return State;
            yield return PostalCode;
            yield return CountryCode;
        }
    }
}
