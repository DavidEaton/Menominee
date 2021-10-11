using CustomerVehicleManagement.Domain.Entities;
using CustomerVehicleManagement.Shared.Helpers;
using System;

namespace CustomerVehicleManagement.Shared.Models
{
    public class PersonToReadInList
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string AddressLine { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public string AddressFull { get => string.IsNullOrWhiteSpace(AddressLine) ? $"{string.Empty}" : $"{AddressLine} {City}, {State} {PostalCode}"; }
        public DateTime? Birthday { get; set; }
        public string PrimaryPhone { get; set; }
        public string PrimaryPhoneType { get; set; }
        public string PrimaryEmail { get; set; }
        public static PersonToReadInList ConvertToDto(Person person)
        {
            if (person != null)
            {
                return new PersonToReadInList()
                {
                    AddressLine = person?.Address?.AddressLine,
                    Birthday = person?.Birthday,
                    City = person?.Address?.City,
                    Id = person.Id,
                    Name = person.Name.LastFirstMiddle,
                    PostalCode = person?.Address?.PostalCode,
                    State = person?.Address?.State.ToString(),
                    PrimaryPhone = PhoneHelper.GetPrimaryPhone(person) ?? PhoneHelper.GetOrdinalPhone(person, 0),
                    PrimaryPhoneType = PhoneHelper.GetPrimaryPhoneType(person) ?? PhoneHelper.GetOrdinalPhoneType(person, 0),
                    PrimaryEmail = EmailHelper.GetPrimaryEmail(person) ?? EmailHelper.GetOrdinalEmail(person, 0)
                };
            }

            return null;
        }
    }
}
