using SharedKernel.ValueObjects;
using System;

namespace Client.Models
{
    public class PersonLoadDto
    {
        public PersonLoadDtoName Name { get; set; }
        public int Gender { get; set; }
        public DateTime Birthday { get; set; }
        public PersonLoadDtoDriverslicence DriversLicence { get; set; }
        public PersonLoadDtoAddress Address { get; set; }
        public int Id { get; set; }
    }

    public class PersonLoadDtoName
    {
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastFirstMiddle { get; set; }
        public string LastFirstMiddleInitial { get; set; }
        public string FirstMiddleLast { get; set; }
    }

    public class PersonLoadDtoDriverslicence
    {
        public string Number { get; set; }
        public DateTimeRange ValidRange { get; set; }
        public string State { get; set; }
    }

    public class PersonLoadDtoAddress
    {
        public string AddressLine { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public string CountryCode { get; set; }
    }
}