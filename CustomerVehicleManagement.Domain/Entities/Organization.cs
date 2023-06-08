using CSharpFunctionalExtensions;
using CustomerVehicleManagement.Domain.BaseClasses;
using Menominee.Common.Extensions;
using Menominee.Common.ValueObjects;
using System.Collections.Generic;

namespace CustomerVehicleManagement.Domain.Entities
{
    public class Organization : Contactable
    {
        // Targeting tests at the abstract base class binds them to the code’s implementation details.
        // Always test all concrete classes; don’t test abstract classes directly
        public static readonly string InvalidMessage = $"Invalid organization.";

        public OrganizationName Name { get; private set; }
        public Person Contact { get; private set; }
        private Organization(
            OrganizationName name,
            string notes = null,
            Person contact = null,
            Address address = null,
            IReadOnlyList<Email> emails = null,
            IReadOnlyList<Phone> phones = null)
            : base(notes, address, phones, emails)
        {
            Name = name;
            Contact = contact;
        }

        public static Result<Organization> Create(
            OrganizationName name,
            string notes = null,
            Person contact = null,
            Address address = null,
            IReadOnlyList<Email> emails = null,
            IReadOnlyList<Phone> phones = null)
        {
            // ValueObject parameters are already validated by OrganizationValidator,
            // which runs within the asp.net request pipeline, invoking each
            // ValueObject's contract validator. For example, AddressValidator :
            // AbstractValidator<AddressToWrite>
            // Only the primitive type (vs. ValueObject type) Notes property is
            // transformed and validated (parsed) here in the domain class that
            // creates it.
            notes = (notes ?? string.Empty).Trim().Truncate(NoteMaximumLength);

            if (name is null)
                return Result.Failure<Organization>(InvalidMessage);

            if (!string.IsNullOrWhiteSpace(notes) && notes.Length > NoteMaximumLength)
                return Result.Failure<Organization>(NoteMaximumLengthMessage);

            return Result.Success(new Organization(name, notes, contact, address, emails, phones));
        }

        public void SetName(OrganizationName name)
        {
            Name = name;
        }

        public void SetContact(Person contact)
        {
            Contact = contact;
        }

        public override string ToString()
        {
            return Name.Name;
        }

        #region ORM

        // Code that pollutes our domain class (very minor impact in this case), but
        // is necessary for EntityFramework, makes our model <100% persistence ignorant.

        // EF requires a parameterless constructor
        protected Organization() { }

        #endregion
    }
}