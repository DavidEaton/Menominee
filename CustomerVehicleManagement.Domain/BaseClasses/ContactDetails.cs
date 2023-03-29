using System;
using System.Collections.Generic;
using System.Linq;
using CSharpFunctionalExtensions;
using CustomerVehicleManagement.Domain.Entities;
using Menominee.Common.ValueObjects;

namespace CustomerVehicleManagement.Domain.BaseClasses
{
    public class ContactDetails : ValueObject
    {
        public IReadOnlyList<Phone> Phones { get; }
        public IReadOnlyList<Email> Emails { get; }
        public Maybe<Address> Address { get; }

        // VK: The factory method (Create) is outside of this class so that it doesn't depend on data contracts

        public ContactDetails(IReadOnlyList<Phone> phones, IReadOnlyList<Email> emails, Maybe<Address> address)
        {
            if (phones == null || emails == null)
                throw new Exception("Phones or emails is null");

            if (phones.Count(phone => phone.IsPrimary) > 1)
                throw new Exception("Must have only 1 primary phone");

            if (emails.Count(email => email.IsPrimary) > 1)
                throw new Exception("Must have only 1 primary email");

            Phones = phones;
            Emails = emails;
            Address = address;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            foreach (Phone phone in Phones)
                yield return phone;

            foreach (Email email in Emails)
                yield return email;

            yield return Address;
        }
    }
}
