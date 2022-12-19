using CSharpFunctionalExtensions;
using CustomerVehicleManagement.Domain.BaseClasses;
using Menominee.Common.Enums;
using Menominee.Common.Extensions;
using Menominee.Common.ValueObjects;
using System;
using System.Collections.Generic;

namespace CustomerVehicleManagement.Domain.Entities.Payables
{
    public class Vendor : Contactable
    {
        public static readonly int NoteMaximumLength = 10000;
        public static readonly int MinimumLength = 2;
        public static readonly int MaximumLength = 255;
        public static readonly string InvalidLengthMessage = $"Name, Code must be between {MinimumLength} character(s) {MaximumLength} and in length";
        public string Name { get; private set; }
        public string VendorCode { get; private set; }
        public VendorRole VendorRole { get; private set; }
        public DefaultPaymentMethod DefaultPaymentMethod { get; private set; }
        // TODO: Add this field
        //public string Notes { get; private set; }
        public bool? IsActive { get; private set; }

        // TODO: Need another table of required fields for purchases which currently include:
        //      vendor part number
        //      vendor invoice #
        //      cost
        //      date purchased
        //      PO number
        // TODO: Then we need to add a collection property containing these requirements
        // TODO: We may also want to add a collection of contacts (e.g., billing person, sales rep)
        //       We really only need their name, department, phone (1) & email (1).

        private Vendor(
            string name, 
            string vendorCode, 
            DefaultPaymentMethod defaultPaymentMethod,
            //string notes = null,
            Address address = null,
            IList<Email> emails = null,
            IList<Phone> phones = null)
            : base(address, phones, emails)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException(RequiredMessage);

            if (string.IsNullOrWhiteSpace(vendorCode))
                throw new ArgumentNullException(RequiredMessage);

            name = (name ?? string.Empty).Trim();
            vendorCode = (vendorCode ?? string.Empty).Trim();

            if (name.Length < MinimumLength ||
                name.Length > MaximumLength ||
                vendorCode.Length < MinimumLength ||
                vendorCode.Length > MaximumLength)
                throw new ArgumentOutOfRangeException(InvalidLengthMessage);

            if (defaultPaymentMethod is not null)
                DefaultPaymentMethod = defaultPaymentMethod;

            Name = name;
            VendorCode = vendorCode;
            //Notes = notes;
            IsActive = true;
        }

        public static Result<Vendor> Create(
            string name, 
            string vendorCode, 
            DefaultPaymentMethod defaultPaymentMethod = null,
            //string notes = null,
            Address address = null,
            IList<Email> emails = null,
            IList<Phone> phones = null)
        {
            if (string.IsNullOrWhiteSpace(name))
                return Result.Failure<Vendor>(RequiredMessage);

            if (string.IsNullOrWhiteSpace(vendorCode))
                return Result.Failure<Vendor>(RequiredMessage);

            name = (name ?? string.Empty).Trim();
            vendorCode = (vendorCode ?? string.Empty).Trim();
            //notes = (notes ?? string.Empty).Trim().Truncate(NoteMaximumLength);

            if (name.Length < MinimumLength ||
                name.Length > MaximumLength ||
                vendorCode.Length < MinimumLength ||
                vendorCode.Length > MaximumLength)
                return Result.Failure<Vendor>(InvalidLengthMessage);

            return Result.Success(new Vendor(name, vendorCode, defaultPaymentMethod, /*notes,*/ address, emails, phones));
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

        //public void SetNotes(string notes)
        //{
        //    Notes = notes.Trim().Truncate(10000);
        //}

        public void Enable() => IsActive = true;

        public void Disable() => IsActive = false;

        #region ORM

        // EF requires a parameterless constructor
        protected Vendor() { }

        #endregion
    }
}
