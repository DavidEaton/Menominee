﻿using CustomerVehicleManagement.Domain.Entities;
using CustomerVehicleManagement.Domain.Interfaces;
using CustomerVehicleManagement.Domain.Utilities;
using SharedKernel;
using System;
using System.Collections.Generic;

namespace CustomerVehicleManagement.Domain.BaseClasses
{
    public abstract class Contactable : Entity, IListOfPhone
    {
        public static readonly string DuplicatePhoneExistsMessage = "Cannot add duplicate phone.";
        public static readonly string PrimaryPhoneExistsMessage = "Cannot add more than one Primary phone.";
        public static readonly string EmptyPhoneCollectionMessage = "Cannot add an empty phone list";
        public static readonly string DuplicateEmailExistsMessage = "Cannot add duplicate email.";
        public static readonly string PrimaryEmailExistsMessage = "Cannot add more than one Primary email.";
        public static readonly string EmptyEmailCollectionMessage = "Cannot add an empty email list";

        public virtual IList<Phone> Phones { get; private set; } = new List<Phone>();
        public virtual IList<Email> Emails { get; private set; } = new List<Email>();

        public void AddPhone(Phone phone)
        {
            if (PhoneHelpers.DuplicatePhoneNumberExists(phone, Phones))
                throw new InvalidOperationException(DuplicatePhoneExistsMessage);

            if (PhoneHelpers.PrimaryPhoneExists(Phones) && phone.IsPrimary)
                throw new InvalidOperationException(PrimaryPhoneExistsMessage);

            if (Phones == null)
                Phones = new List<Phone>();

            if (Phones != null)
                Phones.Add(phone);
        }

        public void RemovePhone(Phone phone)
        {
            if (Phones != null && phone != null) 
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

            if (Phones == null)
                Phones = new List<Phone>();

            if (Phones != null)
                Phones = phones;
        }

        public void AddEmail(Email email)
        {
            if (EmailHelpers.DuplicateEmailExists(email, Emails))
                throw new InvalidOperationException(DuplicateEmailExistsMessage);

            if (EmailHelpers.PrimaryEmailExists(Emails) && email.IsPrimary)
                throw new InvalidOperationException(PrimaryEmailExistsMessage);

            if (Emails == null)
                Emails = new List<Email>();

            if (Emails != null)
                Emails.Add(email);
        }

        public void RemoveEmail(Email email)
        {
            if (Emails != null && email != null)
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

            if (Emails == null)
                Emails = new List<Email>();

            if (Emails != null)
                Emails = emails;
        }
    }
}