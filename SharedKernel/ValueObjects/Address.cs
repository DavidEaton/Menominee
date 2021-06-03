using SharedKernel.Utilities;
using System;
using System.Collections.Generic;

namespace SharedKernel.ValueObjects
{
    public class Address : ValueObject
    {
        public static readonly string AddressEmptyMessage = "Address details cannot be empty";

        public Address(string addressLine, string city, string state, string postalCode)
        {
            try
            {
                Guard.ForNullOrEmpty(addressLine, "addressLine");
                Guard.ForNullOrEmpty(city, "city");
                Guard.ForNullOrEmpty(state, "state");
                Guard.ForNullOrEmpty(postalCode, "postalCode");
            }
            catch (Exception)
            {
                throw new ArgumentException(AddressEmptyMessage);
            }

            AddressLine = addressLine;
            City = city;
            State = state;
            PostalCode = postalCode;
        }

        public string AddressLine { get; private set; }
        public string City { get; private set; }
        public string State { get; private set; }
        public string PostalCode { get; private set; }

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
