using CSharpFunctionalExtensions;
using CustomerVehicleManagement.Domain.Entities;
using CustomerVehicleManagement.Domain.Interfaces;
using Menominee.Common.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using Entity = Menominee.Common.Entity;

namespace CustomerVehicleManagement.Domain.BaseClasses
{
    public abstract class Contactable : Entity, IContactLists
    {
        public static readonly string RequiredMessage = $"Please enter all required items.";
        public static readonly string NonuniqueMessage = $"Item has already been entered; each item must be unique.";
        public static readonly string PrimaryExistsMessage = $"Primary has already been entered.";

        public IList<Phone> Phones { get; private set; } = new List<Phone>();
        public IList<Email> Emails { get; private set; } = new List<Email>();
        public Address Address { get; private set; } //VK: Initialize?

        internal Contactable(Address address, IList<Phone> phones, IList<Email> emails)
        {
            if (address is not null)
                SetAddress(address);

            if (phones is not null)
                foreach (var phone in phones)
                    Phones.Add(phone);

            if (emails is not null)
                foreach (var email in emails)
                    Emails.Add(email);
        }

        public Result<Email> AddEmail(Email email)
        {
            if (email is null)
                return Result.Failure<Email>(RequiredMessage);

            if (ContactableHasEmail(email))
                return Result.Failure<Email>(NonuniqueMessage);

            if (ContactableHasPrimaryEmail() && email.IsPrimary)
                return Result.Failure<Email>(PrimaryExistsMessage);

            Emails.Add(email);
            return Result.Success(email);
        }

        public Result RemoveEmail(Email email)
        {
            if (email is null)
                return Result.Failure<Email>(RequiredMessage);

            return Result.Success(Emails.Remove(email));
        }

        public Result<Phone> AddPhone(Phone phone)
        {
            if (phone is null)
                return Result.Failure<Phone>(RequiredMessage);

            if (ContactableHasPhone(phone))
                return Result.Failure<Phone>(NonuniqueMessage);

            if (ContactableHasPrimaryPhone() && phone.IsPrimary)
                return Result.Failure<Phone>(PrimaryExistsMessage);

            Phones.Add(phone);
            return Result.Success(phone);
        }

        public Result RemovePhone(Phone phone)
        {
            if (phone is null)
                return Result.Failure<Phone>(RequiredMessage);

            return Result.Success(Phones.Remove(phone));
        }

        // VK Im.2: you don't need to return REsult here, just void is fine
        public Result SetAddress(Address address)
        {
            // Address is guaranteed to be valid; it was validated on creation.
            // Address is optional, so excluding it shouldn't throw an exception:
            return Result.Success(Address = address);
        }

        public void ClearAddress()
        {
            Address = null;
        }

        private bool ContactableHasPhone(Phone phone)
        {
            return Phones.Any(existingPhone => existingPhone.Number == phone.Number);
        }

        private bool ContactableHasPrimaryPhone()
        {
            return Phones.Any(existingPhone => existingPhone.IsPrimary);
        }

        private bool ContactableHasEmail(Email email)
        {
            return Emails.Any(existingEmail => existingEmail.Address == email.Address);
        }

        private bool ContactableHasPrimaryEmail()
        {
            return Emails.Any(email => email.IsPrimary);
        }

        // VK: I haven't tested how it would work with EF Core, but the logic of syncing should be as follows
        public void SyncContactDetails(ContactDetails contactDetails)
        {
            SyncPhones(contactDetails.Phones);
            SyncEmails(contactDetails.Emails);
            Address = contactDetails.Address.GetValueOrDefault();
        }

        private void SyncPhones(IReadOnlyList<Phone> phones)
        {
            Phone[] toAdd = phones
                .Where(phone => phone.Id == 0)
                .ToArray();

            Phone[] toDelete = Phones
                .Where(phone => phones.Any(callerPhone => callerPhone.Id == phone.Id) == false)
                .ToArray();

            Phone[] toModify = Phones
                .Where(phone => phones.Any(callerPhone => callerPhone.Id == phone.Id))
                .ToArray();

            foreach (Phone phone in toDelete)
            {
                RemovePhone(phone);
            }

            foreach (Phone phone in toModify)
            {
                Phone phoneFromCaller = phones.Single(phone => phone.Id == phone.Id);

                if (phone.Number != phoneFromCaller.Number)
                    phone.SetNumber(phoneFromCaller.Number);

                if (phone.PhoneType != phoneFromCaller.PhoneType)
                    phone.SetPhoneType(phoneFromCaller.PhoneType);

                if (phone.IsPrimary != phoneFromCaller.IsPrimary)
                    phone.SetIsPrimary(phoneFromCaller.IsPrimary);
            }

            foreach (Phone phone in toAdd)
            {
                Result result = AddPhone(phone);
                if (result.IsFailure)
                    throw new Exception(result.Error);
            }
        }

        private void SyncEmails(IReadOnlyList<Email> emails)
        {
            Email[] toAdd = emails
                .Where(email => email.Id == 0)
                .ToArray();

            Email[] toDelete = Emails
                .Where(email => emails.Any(callerEmail => callerEmail.Id == email.Id) == false)
                .ToArray();

            Email[] toModify = Emails
                .Where(email => emails.Any(callerEmail => callerEmail.Id == email.Id))
                .ToArray();

            foreach (Email email in toDelete)
            {
                RemoveEmail(email);
            }

            foreach (Email email in toModify)
            {
                Email emailFromCaller = emails.Single(callerEmail => callerEmail.Id == email.Id);

                if (email.Address != emailFromCaller.Address)
                    email.SetAddress(emailFromCaller.Address);

                if (email.IsPrimary != emailFromCaller.IsPrimary)
                    email.SetIsPrimary(emailFromCaller.IsPrimary);
            }

            foreach (Email email in toAdd)
            {
                Result result = AddEmail(email);
                if (result.IsFailure)
                    throw new Exception(result.Error);
            }
        }

        #region ORM

        // EF requires parameterless constructor
        protected Contactable() { }

        #endregion
    }
}
