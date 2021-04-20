using SharedKernel.Enums;
using System;

namespace Menominee.Client.Models
{
    public class PersonFlatDto
    {
        public int Id { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public Gender Gender { get; set; }
        public DateTime? Birthday { get; set; }
        public string AddressLine { get; set; }
        public string AddressCity { get; set; }
        public string AddressState { get; set; }
        public string AddressPostalCode { get; set; }

        public string LastFirstMiddle
        {
            get => string.IsNullOrWhiteSpace(MiddleName) ? $"{LastName}, {FirstName}" : $"{LastName}, {FirstName} {MiddleName}";
        }
        public string LastFirstMiddleInitial
        {
            get => string.IsNullOrWhiteSpace(MiddleName) ? $"{LastName}, {FirstName}" : $"{LastName}, {FirstName} {MiddleName[0]}.";
        }
        public string FirstMiddleLast
        {
            get => string.IsNullOrWhiteSpace(MiddleName) ? $"{FirstName} {LastName}" : $"{FirstName} {MiddleName} {LastName}";
        }
        public string AddressFull
        {
            get => string.IsNullOrWhiteSpace(AddressLine) ? $"{AddressCity} {AddressState}" : $"{AddressLine} {AddressCity}, {AddressState}  {AddressPostalCode}";
        }
    }
}
