using SharedKernel.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SharedKernel.ValueObjects
{
    public class Email : ValueObject
    {
        public const string EmailInvalidMessage = "Email address cannot be empty";
        public const string EmailErrorMessage = "Email address and/or its format is invalid";
        public Email(string address, bool primary)
        {
            try
            {
                Guard.ForNullOrEmpty(address, "address");
            }
            catch (Exception)
            {
                throw new ArgumentException(EmailInvalidMessage);
            }

            var emailAddressAttribute = new EmailAddressAttribute();

            if (!emailAddressAttribute.IsValid(address))
            {
                throw new ArgumentException(EmailErrorMessage);
            }

            else
            {
                Address = address;
                Primary = primary;
            }
        }

        public string Address { get; }
        public bool Primary { get; } = true;

        public Email NewAddress(string newAddress)
        {
            return new Email(newAddress, Primary);
        }

        public Email NewPrimary(bool newPrimary)
        {
            return new Email(Address, newPrimary);
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Address;
            yield return Primary;
        }
    }
}
