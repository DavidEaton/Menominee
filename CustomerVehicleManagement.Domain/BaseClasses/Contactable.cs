using CustomerVehicleManagement.Domain.Entities;
using CustomerVehicleManagement.Domain.Interfaces;
using Menominee.Common;
using Menominee.Common.Utilities;
using Menominee.Common.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CustomerVehicleManagement.Domain.BaseClasses
{
    public abstract class Contactable : Entity, IContactLists
    {
        public virtual IList<Phone> Phones { get; private set; } = new List<Phone>();
        public virtual IList<Email> Emails { get; private set; } = new List<Email>();
        public Address Address { get; private set; }

        public void AddPhone(Phone phone)
        {
            // VK: phone number being null is usually a bug, so best to put a guard here instead of the null check.
            /* Null check silently swallows exception, hiding potential bugs. Use a guard to throw exception in
             * this exceptional case: we don't expect a null to ever reach here, so there must be a bug. -DE */
            Guard.ForNull(phone, "phone");

            if (ContactableHasPhone(phone))
                throw new Exception("Contactable already has this phone");

            if (ContactableHasPrimaryPhone() && phone.IsPrimary)
                throw new Exception("Contactable already has primary phone.");

            Phones.Add(phone);
        }

        public void RemovePhone(Phone phone)
        {
            Guard.ForNull(phone, "phone");
            Phones.Remove(phone);
        }

        // VK: make SetPhones call AddPhone. This way, you'll avoid validation duplication
        public void SetPhones(IList<Phone> phones)
        {
            Guard.ForNull(phones, "phones");

            if (phones.Count > 0)
            {
                Phones.Clear();
                var sortedPhones = phones.OrderBy(e => e.IsPrimary).ToList();
                foreach (var phone in sortedPhones)
                    AddPhone(phone);
            }
        }

        public void AddEmail(Email email)
        {
            Guard.ForNull(email, "email");

            if (ContactableHasEmail(email))
                throw new Exception("Contactable already has this email.");

            if (ContactableHasPrimaryEmail() && email.IsPrimary)
                throw new Exception("Contactable already has primary email.");

            Emails.Add(email);
        }

        public void RemoveEmail(Email email)
        {
            Guard.ForNull(email, "email");
            Emails.Remove(email);
        }

        public void SetEmails(IList<Email> emails)
        {
            Guard.ForNull(emails, "emails");

            if (emails.Count > 0)
            {
                Emails.Clear();
                // If a primary email exists before the end of the list, AddEmail()
                // fails at check ContactableHasPrimaryEmail() since the primary email
                // in the list was already added. This code will work when list is
                // sorted and the Primary email is added last:
                var sortedEmails = emails.OrderBy(e => e.IsPrimary).ToList();
                foreach (var email in sortedEmails)
                    AddEmail(email);
            }
        }

        public void SetAddress(Address address)
        {
            // Guard unnecessarily throws exception; we just need a null check.
            // Address is guaranteed to be valid; it was validated on creation.
            // Address is optional, so excluding it shouldn't throw an exception:
            // Guard.ForNull(address, "address");
            if (address != null)
                Address = address;
        }

        // VK: no need to make these methods public, they are just for the Contactable class
        // you can also keep them non-static, so that you don't need to pass in the existing collections
        private bool ContactableHasPhone(Phone phone)
        {
            return Phones.Any(x => x.Number == phone.Number);
        }

        private bool ContactableHasPrimaryPhone()
        {
            return Phones.Any(x => x.IsPrimary);
        }

        private bool ContactableHasPrimaryEmail()
        {
            return Emails.Any(x => x.IsPrimary);
        }

        private bool ContactableHasEmail(Email email)
        {
            return Emails.Any(x => x.Address == email.Address);
        }
    }
}
