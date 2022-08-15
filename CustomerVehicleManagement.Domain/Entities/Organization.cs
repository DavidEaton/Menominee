using CSharpFunctionalExtensions;
using CustomerVehicleManagement.Domain.BaseClasses;
using Menominee.Common.Extensions;
using Menominee.Common.ValueObjects;
using System.Collections.Generic;

namespace CustomerVehicleManagement.Domain.Entities
{
    public class Organization : Contactable
    {
        public static readonly int NoteMaximumLength = 10000;
        public static readonly string NoteMaximumLengthMessage = $"Note cannot be over {NoteMaximumLength} characters in length.";
        public static readonly string InvalidMessage = $"Invalid organization.";

        public OrganizationName Name { get; private set; }
        public Person Contact { get; private set; }
        public string Note { get; private set; }
        private Organization(
            OrganizationName name,
            string note,
            Person contact,
            Address address = null,
            IList<Email> emails = null,
            IList<Phone> phones = null)
            : base(address, phones, emails)
        {
            Name = name;
            Note = note;
            Contact = contact;
        }

        public static Result<Organization> Create(
            OrganizationName name,
            string note,
            Person contact,
            Address address = null,
            IList<Email> emails = null,
            IList<Phone> phones = null)
        {
            // ValueObject parameters are already validated by OrganizationValidator,
            // which runs within the asp.net request pipeline, invoking each
            // ValueObject's contract validator. For example, AddressValidator :
            // AbstractValidator<AddressToWrite>
            // Only the primitive type (vs. ValueObject type) Note property is
            // transformed and validated (parsed) here in the domaon class that
            // creates it.
            note = (note ?? string.Empty).Trim().Truncate(NoteMaximumLength);

            if (name is null)
                return Result.Failure<Organization>(InvalidMessage);

            if (!string.IsNullOrWhiteSpace(note) && note.Length > NoteMaximumLength)
                return Result.Failure<Organization>(NoteMaximumLengthMessage);

            return Result.Success(new Organization(name, note, contact, address, emails, phones));
        }

        public void SetName(OrganizationName name)
        {
            Name = name;
        }

        public void SetContact(Person contact)
        {
            Contact = contact;
        }

        public void SetNote(string note)
        {
            // ValueObject parameters are already validated by OrganizationValidator.
            // Only the primitive type (vs. ValueObject type) Note property is
            // parsed here in the domain class method that uses it.
            Note = note.Trim().Truncate(10000);
        }

        #region ORM

        // Code that pollutes our domain class (very minor impact in this case), but
        // is necessary for EntityFramework, makes our model <100% persistence ignorant.

        // EF requires an empty constructor
        protected Organization() { }

        #endregion
    }
}