using System;
using System.Collections.Generic;

namespace SharedKernel.ValueObjects
{
    public class Address : ValueObject
    {
        public static readonly string AddressEmptyMessage = "Address details cannot be empty";
        private string addressFull;

        public Address(string addressLine, string city, string state, string postalCode)
        {
            if (string.IsNullOrWhiteSpace(addressLine) | string.IsNullOrWhiteSpace(city) | string.IsNullOrWhiteSpace(state) | string.IsNullOrWhiteSpace(postalCode))
            {
                throw new ArgumentException(AddressEmptyMessage);
            }

            AddressLine = addressLine;
            City = city;
            State = state;
            PostalCode = postalCode;
        }

        public string AddressLine { get; }
        public string City { get; }
        public string State { get; }
        public string PostalCode { get; }

        public Address NewAddressLine(string newAddressLine)
        {
            return new Address(newAddressLine, City, State, PostalCode);
        }

        public Address NewCity(string newCity)
        {
            return new Address(AddressLine, newCity, State, PostalCode);
        }

        public Address NewState(string newState)
        {
            return new Address(AddressLine, City, newState, PostalCode);
        }

        public Address NewPostalCode(string newPostalCode)
        {
            return new Address(AddressLine, City, State, newPostalCode);
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return AddressLine;
            yield return City;
            yield return State;
            yield return PostalCode;
        }

        public string AddressFull { get => $"{AddressLine} {City}, {State}  {PostalCode}"; }

        #region ORM

        // EF requires an empty constructor
        protected Address() { }

        #endregion
    }
}
