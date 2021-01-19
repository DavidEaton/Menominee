using CustomerVehicleManagement.Domain.Enums;
using CustomerVehicleManagement.Domain.ValueObjects;
using System;

namespace Client.Models
{
    public class PersonDto
    {
        public int Id { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public Gender Gender { get; set; }
        public DateTime? Birthday { get; set; }
        public string DriversLicenceNumber { get; set; }
        public string DriversLicenceState { get; set; }
        public DateTime DriversLicenceIssued { get; set; }
        public DateTime DriversLicenceExpiry { get; set; }
        public string AddressLine { get; set; }
        public string AddressCity { get; set; }
        public string AddressState { get; set; }
        public string AddressPostalCode { get; set; }
        public string AddressCountryCode { get; set; }

        public string LastFirstMiddle
        {
            get => string.IsNullOrWhiteSpace(MiddleName) ? $"{LastName}, {FirstName}" : $"{LastName}, {FirstName} {MiddleName}";
        }
        public string LastFirstMiddleInitial
        {
            get => string.IsNullOrWhiteSpace(MiddleName) ? $"{LastName}, {FirstName}" : $"{LastName}, {FirstName} {MiddleName[0]}.";
        }

        internal void AddDriversLicense(DriversLicence driversLicence)
        {
            throw new NotImplementedException();
        }

        public string FirstMiddleLast
        {
            get => string.IsNullOrWhiteSpace(MiddleName) ? $"{FirstName} {LastName}" : $"{FirstName} {MiddleName} {LastName}";
        }

        internal void AddAddress(CustomerVehicleManagement.Domain.ValueObjects.Address address)
        {
            AddressLine = address.AddressLine;
            AddressCity = address.City;
            AddressState = address.State;
            AddressPostalCode = address.PostalCode;
            AddressCountryCode = address.CountryCode;
        }
    }
}