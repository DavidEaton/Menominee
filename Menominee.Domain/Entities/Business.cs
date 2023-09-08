using CSharpFunctionalExtensions;
using Menominee.Common.Extensions;
using Menominee.Common.ValueObjects;
using Menominee.Domain.BaseClasses;
using System.Collections.Generic;

namespace Menominee.Domain.Entities
{
    public class Business : Contactable
    {
        // Targeting tests at the abstract base class binds them to the code’s implementation details.
        // Always test all concrete classes; don’t test abstract classes directly
        public static readonly string InvalidMessage = $"Invalid business.";

        public BusinessName Name { get; private set; }
        public Person Contact { get; private set; }
        private Business(
            BusinessName name,
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

        public static Result<Business> Create(
            BusinessName name,
            string notes = null,
            Person contact = null,
            Address address = null,
            IReadOnlyList<Email> emails = null,
            IReadOnlyList<Phone> phones = null)
        {
            // ValueObject parameters are already validated by BusinessValidator,
            // which runs within the asp.net request pipeline, invoking each
            // ValueObject's contract validator. For example, AddressValidator :
            // AbstractValidator<AddressToWrite>
            // Only the primitive type (vs. ValueObject type) Notes property is
            // transformed and validated (parsed) here in the domain class that
            // creates it.
            notes = (notes ?? string.Empty).Trim().Truncate(NoteMaximumLength);

            if (name is null)
                return Result.Failure<Business>(InvalidMessage);

            if (!string.IsNullOrWhiteSpace(notes) && notes.Length > NoteMaximumLength)
                return Result.Failure<Business>(NoteMaximumLengthMessage);

            return Result.Success(new Business(name, notes, contact, address, emails, phones));
        }

        // BusinessName has already been validated; no need to validate
        public void SetName(BusinessName name)
        {
            Name = name;
        }

        // Person has already been validated; no need to validate
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
        protected Business() { }

        #endregion
    }
}