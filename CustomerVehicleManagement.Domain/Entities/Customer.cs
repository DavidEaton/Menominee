using CustomerVehicleManagement.Domain.Interfaces;
using SharedKernel;
using SharedKernel.Enums;
using SharedKernel.Interfaces;
using SharedKernel.ValueObjects;
using System;
using System.Collections.Generic;
using CustomerVehicleManagement.Domain.Utilities;
using SharedKernel.Utilities;

namespace CustomerVehicleManagement.Domain.Entities
{
    // VK: the overarching idea here is to move Contactable's logic to Customer and then inherit Person and Organization from it
    // To store the hierarchy in the DB, there are 2 common strategies: Table-per-hierarchy and Table-per-type
    // I usually use Table-per-hierarchy: it's simple and straightforward. Here the documentation: https://docs.microsoft.com/en-us/ef/core/modeling/inheritance

    public abstract class Customer : Entity, ICustomer
    {
        // VK: no need to come up with detailed messages just for exceptions. Those are for programmers only anyway; they should never be user-facing.
        // You can keep the message small, just enough to understand what's going on when reading the logs. For example: 
        // if (phones == null)
        //    throw new Exception("phones == null");
        // Also, no need to be so precise with exception types, just throw Exception,
        // or create your own custom exception type (e.g. ContractException) and throw it whenever the contract is broken

        public static readonly string DuplicatePhoneExistsMessage = "Cannot add duplicate phone.";
        public static readonly string PrimaryPhoneExistsMessage = "Cannot add more than one Primary phone.";
        public static readonly string EmptyPhoneCollectionMessage = "Cannot add an empty phone list";
        public static readonly string DuplicateEmailExistsMessage = "Cannot add duplicate email.";
        public static readonly string PrimaryEmailExistsMessage = "Cannot add more than one Primary email.";
        public static readonly string EmptyEmailCollectionMessage = "Cannot add an empty email list";

        public virtual IList<Phone> Phones { get; private set; } = new List<Phone>();
        public virtual IList<Email> Emails { get; private set; } = new List<Email>();

        // Person or Organization
        //public EntityType EntityType { get; }
        //public IEntity Entity { get; private set; }
        //public int EntityId { get; private set; }
        public CustomerType CustomerType { get; private set; }
        public ContactPreferences ContactPreferences { get; private set; }
        public DateTime Created { get; private set; }
        public virtual IList<Vehicle> Vehicles { get; private set; } = new List<Vehicle>();
        // I've moved the Address here because it is present in both Person and Organization
        public Address Address { get; private set; }

        public Customer()
        {
            Created = DateTime.UtcNow;
        }

        public void AddPhone(Phone phone)
        {
            // VK: try to never put domain logic into helpers, it should reside either in entities or, if you need to reuse it, in value objects
            // Here, since we are removing Contactable, you can move it to this Customer class
            if (PhoneHelpers.DuplicatePhoneNumberExists(phone, Phones))
                throw new InvalidOperationException(DuplicatePhoneExistsMessage);

            if (PhoneHelpers.PrimaryPhoneExists(Phones) && phone.IsPrimary)
                throw new InvalidOperationException(PrimaryPhoneExistsMessage);

            // VK: no need for this since Phones is initialized on class instantiation
            //if (Phones == null)
            //    Phones = new List<Phone>();
            
            Phones.Add(phone);
        }

        public void RemovePhone(Phone phone)
        {
            // VK: phone number being null is usually a bug, so best to put a guard here instead of the null check
            Guard.ForNull(phone, "phone");
            
            Phones.Remove(phone);
        }
        
        public void SetPhones(IList<Phone> phones)
        {
            if (phones == null)
                throw new ArgumentException(EmptyPhoneCollectionMessage, nameof(phones));

            if (PhoneHelpers.DuplicatePhoneExists(phones))
                throw new InvalidOperationException(DuplicatePhoneExistsMessage);

            if (PhoneHelpers.PrimaryPhoneCountExceedsOne(phones))
                throw new InvalidOperationException(PrimaryPhoneExistsMessage);
            
            Phones = phones;
        }

        public void AddEmail(Email email)
        {
            if (EmailHelpers.DuplicateEmailExists(email, Emails))
                throw new InvalidOperationException(DuplicateEmailExistsMessage);

            if (EmailHelpers.PrimaryEmailExists(Emails) && email.IsPrimary)
                throw new InvalidOperationException(PrimaryEmailExistsMessage);
            
            Emails.Add(email);
        }

        public void RemoveEmail(Email email)
        {
            Guard.ForNull(email, "email");
            
            Emails.Remove(email);
        }

        public void SetEmails(IList<Email> emails)
        {
            if (emails == null)
                throw new ArgumentException(EmptyEmailCollectionMessage, nameof(emails));

            if (EmailHelpers.DuplicateEmailExists(emails))
                throw new InvalidOperationException(DuplicateEmailExistsMessage);

            if (EmailHelpers.PrimaryEmailCountExceedsOne(emails))
                throw new InvalidOperationException(PrimaryEmailExistsMessage);
            
            Emails = emails;
        }

        public void AddVehicle(Vehicle vehicle)
        {
            Vehicles.Add(vehicle);
        }

        public void RemoveVehicle(Vehicle vehicle)
        {
            Vehicles.Remove(vehicle);
        }

        public void SetAddress(Address address)
        {
            Address = address;
        }

        #region ORM

        // EF requires an empty constructor
        //protected Customer() { }

        #endregion
    }
}
