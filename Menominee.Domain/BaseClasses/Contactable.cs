using CSharpFunctionalExtensions;
using Menominee.Common.Extensions;
using Menominee.Common.ValueObjects;
using Menominee.Domain.Entities;
using Menominee.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using Entity = Menominee.Common.Entity;

namespace Menominee.Domain.BaseClasses
{
    public abstract class Contactable : Entity, IContactable
    {
        // Targeting tests at the abstract base class binds them to the code’s implementation details.
        // Always test only concrete classes; don’t test abstract classes directly
        public static readonly int NoteMaximumLength = 10000;
        public static readonly string NoteMaximumLengthMessage = $"Notes cannot be over {NoteMaximumLength} characters in length.";
        public static readonly string RequiredMessage = "Please include all required items.";
        public static readonly string NonuniqueMessage = $"Item has already been entered; each item must be unique.";
        public static readonly string PrimaryExistsMessage = $"Primary has already been entered.";
        public static readonly string InvalidValueMessage = $"Value was invalid";
        public static readonly string NotFoundMessage = $"Item was not found";
        public string Notes { get; private set; }
        public Address Address { get; private set; }

        private readonly List<Phone> phones = new();
        public IReadOnlyList<Phone> Phones => phones.ToList();
        private readonly List<Email> emails = new();
        public IReadOnlyList<Email> Emails => emails.ToList();

        internal Contactable(string notes, Address address, IReadOnlyList<Phone> phones, IReadOnlyList<Email> emails)
        {
            Notes = notes;

            if (address is not null)
                SetAddress(address);

            if (phones is not null)
                foreach (var phone in phones)
                    AddPhone(phone);

            if (emails is not null)
                foreach (var email in emails)
                    AddEmail(email);
        }

        public Result<Email> AddEmail(Email email)
        {
            if (email is null)
                return Result.Failure<Email>(RequiredMessage);

            if (HasEmail(email))
                return Result.Failure<Email>(NonuniqueMessage);

            if (HasPrimaryEmail() && email.IsPrimary)
                return Result.Failure<Email>(PrimaryExistsMessage);

            emails.Add(email);

            return Result.Success(email);
        }

        public Result<Email> RemoveEmail(Email email)
        {
            if (email is null)
                return Result.Failure<Email>(RequiredMessage);

            if (!emails.Contains(email))
                return Result.Failure<Email>(NotFoundMessage);

            emails.Remove(email);

            return Result.Success(email);
        }

        public Result<Phone> AddPhone(Phone phone)
        {
            if (phone is null)
                return Result.Failure<Phone>(RequiredMessage);

            if (HasPhone(phone))
                return Result.Failure<Phone>(NonuniqueMessage);

            if (HasPrimaryPhone() && phone.IsPrimary)
                return Result.Failure<Phone>(PrimaryExistsMessage);

            phones.Add(phone);

            return Result.Success(phone);
        }

        public Result<Phone> RemovePhone(Phone phone)
        {
            if (phone is null)
                return Result.Failure<Phone>(RequiredMessage);

            if (!phones.Contains(phone))
                return Result.Failure<Phone>(NotFoundMessage);

            phones.Remove(phone);

            return Result.Success(phone);
        }

        public Result<string> SetNotes(string note)
        {
            return Result.Success(Notes = note.Trim().Truncate(NoteMaximumLength));
        }

        public Result SetAddress(Address address)
        {
            if (address is null)
                return Result.Failure<Address>(RequiredMessage);

            // Address (if present) is guaranteed to be valid;
            // it was validated on creation.
            return Result.Success(Address = address);
        }

        public Result ClearAddress()
        {
            return Result.Success(Address = null);
        }

        public bool HasPhone(Phone phone)
        {
            return Phones.Any(existingPhone => existingPhone.Number == phone.Number);
        }

        public bool HasPrimaryPhone()
        {
            return Phones.Any(existingPhone => existingPhone.IsPrimary);
        }

        public bool HasEmail(Email email)
        {
            return Emails.Any(existingEmail => existingEmail.Address == email.Address);
        }

        public bool HasPrimaryEmail()
        {
            return Emails.Any(email => email.IsPrimary);
        }

        public void UpdateContactDetails(ContactDetails contactDetails)
        {
            UpdatePhones(contactDetails.Phones);
            UpdateEmails(contactDetails.Emails);
            Address = contactDetails.Address.GetValueOrDefault();
        }

        private void UpdatePhones(IReadOnlyList<Phone> phones)
        {
            var toAdd = phones
                .Where(phone => phone.Id == 0)
                .ToArray();

            var toDelete = Phones
                .Where(phone => !phones.Any(callerPhone => callerPhone.Id == phone.Id))
                .ToArray();

            var toModify = Phones
                .Where(phone => phones.Any(callerPhone => callerPhone.Id == phone.Id))
                .ToArray();

            toModify.ToList()
                .ForEach(phone =>
                {
                    var phoneFromCaller = phones.Single(callerPhone => callerPhone.Id == phone.Id);

                    if (phone.Number != phoneFromCaller.Number)
                        phone.SetNumber(phoneFromCaller.Number);

                    if (phone.PhoneType != phoneFromCaller.PhoneType)
                        phone.SetPhoneType(phoneFromCaller.PhoneType);

                    if (phone.IsPrimary != phoneFromCaller.IsPrimary)
                        phone.SetIsPrimary(phoneFromCaller.IsPrimary);
                });

            toDelete.ToList()
                .ForEach(phone => RemovePhone(phone));

            toAdd.ToList()
                .ForEach(phone =>
                {
                    var result = AddPhone(phone);
                    if (result.IsFailure)
                        throw new Exception(result.Error);
                });
        }

        private void UpdateEmails(IReadOnlyList<Email> emails)
        {
            var toAdd = emails
                .Where(email => email.Id == 0)
                .ToArray();

            var toDelete = Emails
                .Where(email => !emails.Any(callerEmail => callerEmail.Id == email.Id))
                .ToArray();

            var toModify = Emails
                .Where(email => emails.Any(callerEmail => callerEmail.Id == email.Id))
                .ToArray();

            toModify.ToList()
                .ForEach(email =>
                {
                    var emailFromCaller = emails.Single(callerEmail => callerEmail.Id == email.Id);

                    if (email.Address != emailFromCaller.Address)
                        email.SetAddress(emailFromCaller.Address);

                    if (email.IsPrimary != emailFromCaller.IsPrimary)
                        email.SetIsPrimary(emailFromCaller.IsPrimary);
                });

            toDelete.ToList()
                .ForEach(email => RemoveEmail(email));

            toAdd.ToList()
                .ForEach(email =>
                {
                    Result result = AddEmail(email);
                    if (result.IsFailure)
                        throw new Exception(result.Error);
                });
        }

        #region ORM

        // EF requires parameterless constructor
        protected Contactable()
        {
            phones = new List<Phone>();
            emails = new List<Email>();
        }

        #endregion
    }
}
