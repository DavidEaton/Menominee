using CSharpFunctionalExtensions;
using Menominee.Domain.BaseClasses;
using Menominee.Domain.Enums;
using Menominee.Domain.Extensions;
using Menominee.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Menominee.Domain.Entities.Payables
{
    public class Vendor : Contactable
    {
        // Targeting tests at the abstract base class binds them to the code’s implementation details.
        // Always test all concrete classes; don’t test abstract classes directly
        public static readonly int MinimumLength = 2;
        public static readonly int MaximumLength = 255;
        public static readonly string InvalidLengthMessage = $"Name, Code must be between {MinimumLength} characters {MaximumLength} and in length";
        public string Name { get; private set; }
        public string VendorCode { get; private set; }
        public VendorRole VendorRole { get; private set; }
        public DefaultPaymentMethod DefaultPaymentMethod { get; private set; }
        public bool? IsActive { get; private set; }
        public IReadOnlyList<Person> Contacts => contacts.ToList();
        private readonly List<Person> contacts = new();

        // TODO: Vendor settings for requirements
        // TODO: Need another table of required fields for purchases which currently include:
        //      vendor part number
        //      vendor invoice #
        //      cost
        //      date purchased
        //      PO number
        // TODO: Then we need to add a collection property containing these requirements
        // TODO: We may also want to add a collection of contacts (e.g., billing person, sales rep)
        //       We really only need their name, department, phone (1) & email (1).
        //      Added Contacts collection (IReadOnlyList<Person>). We could create VendorContact
        //      class having Department, Role (e.g., billing person, sales rep) members. Then 
        //      replace IReadOnlyList<Person> with IReadOnlyList<VendorContact>. DE

        private Vendor(
            string name,
            string vendorCode,
            VendorRole vendorRole,
            DefaultPaymentMethod defaultPaymentMethod = null,
            string notes = null,
            Address address = null,
            IReadOnlyList<Email> emails = null,
            IReadOnlyList<Phone> phones = null)
            : base(notes, address, phones, emails)
        {
            Name = name;
            VendorCode = vendorCode;
            VendorRole = vendorRole;
            DefaultPaymentMethod = defaultPaymentMethod;
            IsActive = true;
        }

        public static Result<Vendor> Create(
            string name,
            string vendorCode,
            VendorRole vendorRole,
            string notes = null,
             DefaultPaymentMethod defaultPaymentMethod = null,
            Address address = null,
            IReadOnlyList<Email> emails = null,
            IReadOnlyList<Phone> phones = null)
        {
            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(vendorCode))
                return Result.Failure<Vendor>(RequiredMessage);

            name = (name ?? string.Empty).Trim();
            vendorCode = (vendorCode ?? string.Empty).Trim();
            notes = (notes ?? string.Empty).Trim().Truncate(NoteMaximumLength);

            if (!Enum.IsDefined(typeof(VendorRole), vendorRole))
                return Result.Failure<Vendor>(RequiredMessage);

            if (name.Length < MinimumLength ||
                name.Length > MaximumLength ||
                vendorCode.Length < MinimumLength ||
                vendorCode.Length > MaximumLength)
                return Result.Failure<Vendor>(InvalidLengthMessage);

            if (!string.IsNullOrWhiteSpace(notes) && notes.Length > NoteMaximumLength)
                return Result.Failure<Vendor>(NoteMaximumLengthMessage);

            return Result.Success(new Vendor(name, vendorCode, vendorRole, defaultPaymentMethod, notes, address, emails, phones));
        }

        public Result<string> SetName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return Result.Failure<string>(RequiredMessage);

            name = (name ?? string.Empty).Trim();

            if (name.Length < MinimumLength || name.Length > MaximumLength)
                return Result.Failure<string>(InvalidLengthMessage);

            return Result.Success(Name = name);
        }

        public Result<string> SetVendorCode(string vendorCode)
        {
            if (string.IsNullOrWhiteSpace(vendorCode))
                return Result.Failure<string>(RequiredMessage);

            vendorCode = (vendorCode ?? string.Empty).Trim();

            if (vendorCode.Length < MinimumLength || vendorCode.Length > MaximumLength)
                return Result.Failure<string>(InvalidLengthMessage);

            return Result.Success(VendorCode = vendorCode);
        }

        public Result<DefaultPaymentMethod> SetDefaultPaymentMethod(DefaultPaymentMethod defaultPaymentMethod)
        {
            if (defaultPaymentMethod is null)
                return Result.Failure<DefaultPaymentMethod>(RequiredMessage);

            return Result.Success(DefaultPaymentMethod = defaultPaymentMethod);
        }

        public Result ClearDefaultPaymentMethod()
        {
            return Result.Success(DefaultPaymentMethod = null);
        }

        public void Enable() => IsActive = true;

        public void Disable() => IsActive = false;

        public Result<VendorRole> SetVendorRole(VendorRole vendorRole)
        {
            if (!Enum.IsDefined(typeof(VendorRole), vendorRole))
                return Result.Failure<VendorRole>(RequiredMessage);

            return Result.Success(VendorRole = vendorRole);
        }

        #region ORM

        // EF requires a parameterless constructor
        protected Vendor()
        {
            contacts = new List<Person>();
        }

        #endregion
    }
}
