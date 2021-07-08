using SharedKernel;
using SharedKernel.Enums;
using SharedKernel.ValueObjects;
using System;
using System.Collections.Generic;
using SharedKernel.Utilities;
using System.Linq;

namespace CustomerVehicleManagement.Domain.Entities
{
    // VK: the overarching idea here is to move Contactable's logic to Customer and then inherit Person and Organization from it
    // To store the hierarchy in the DB, there are 2 common strategies: Table-per-hierarchy and Table-per-type
    // I usually use Table-per-hierarchy: it's simple and straightforward. Here the documentation: https://docs.microsoft.com/en-us/ef/core/modeling/inheritance

    public abstract class Customer : Entity
    {
        public virtual IList<Phone> Phones { get; private set; } = new List<Phone>();
        public virtual IList<Email> Emails { get; private set; } = new List<Email>();
        public CustomerType CustomerType { get; private set; }
        public ContactPreferences ContactPreferences { get; private set; }
        public DateTime Created { get; private set; }
        public virtual IList<Vehicle> Vehicles { get; private set; } = new List<Vehicle>();
        // VK: I've moved the Address here because it is present in both Person and Organization
        public Address Address { get; private set; }

        public Customer()
        {
            Created = DateTime.UtcNow;
        }

        public void AddPhone(Phone phone)
        {
            // VK: phone number being null is usually a bug, so best to put a guard here instead of the null check
            Guard.ForNull(phone, "phone");

            if (CustomerHasPhone(phone))
                throw new Exception("customer already has this phone");

            if (CustomerHasPrimaryPhone())
                throw new Exception("customer already has primary phone.");

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

            foreach (var phone in phones)
                AddPhone(phone);
        }

        public void AddEmail(Email email)
        {
            Guard.ForNull(email, "email");

            if (CustomerHasEmail(email))
                throw new Exception("customer already has this email.");

            if (CustomerHasPrimaryEmail())
                throw new Exception("customer already has primary email.");

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

            foreach (var email in emails)
                AddEmail(email);
        }

        public void AddVehicle(Vehicle vehicle)
        {
            Guard.ForNull(vehicle, "vehicle");

            if (CustomerHasVehicle(vehicle))
                throw new Exception("customer already has this vehicle.");

            Vehicles.Add(vehicle);
        }

        public void RemoveVehicle(Vehicle vehicle)
        {
            Guard.ForNull(vehicle, "vehicle");
            Vehicles.Remove(vehicle);
        }

        public void SetAddress(Address address)
        {
            Guard.ForNull(address, "address");
            Address = address;
        }

        // VK: no need to make these methods public, they are just for the Customer class
        // you can also keep them non-static, so that you don't need to pass in the existing collection of phones
        private bool CustomerHasPhone(Phone phone)
        {
            return Phones.Any(x => x.Number == phone.Number);
        }

        private bool CustomerHasPrimaryPhone()
        {
            return Phones.Any(x => x.IsPrimary);
        }

        private bool CustomerHasPrimaryEmail()
        {
            return Emails.Any(x => x.IsPrimary);
        }

        private bool CustomerHasEmail(Email email)
        {
            return Emails.Any(x => x.Address == email.Address);
        }
        private bool CustomerHasVehicle(Vehicle vehicle)
        {
            return Vehicles.Any(x => x.Id == vehicle.Id);
        }
    }
}
