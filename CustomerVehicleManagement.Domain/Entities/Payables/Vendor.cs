﻿using CSharpFunctionalExtensions;
using Menominee.Common.Enums;
using System;
using Entity = Menominee.Common.Entity;

namespace CustomerVehicleManagement.Domain.Entities.Payables
{
    public class Vendor : Entity //TODO: Vendor : Contactable
    {
        public static readonly string RequiredMessage = $"Please include all required items.";
        public static readonly int MinimumLength = 2;
        public static readonly int MaximumLength = 255;
        public static readonly string InvalidLengthMessage = $"Name, Code must be between {MinimumLength} character(s) {MaximumLength} and in length";
        public string Name { get; private set; }
        public string VendorCode { get; private set; }
        //TODO: need address, phone, email
        //TODO: public VendorType VendorType { get; private set; }
        //TODO: public VendorInvocePaymentMethod? DefaultPaymentMethod { get; private set; }
        //TODO: public bool AutoCompleteDocuments { get; private set; } // can only be true if DefaultPaymentMethod is set 
        public bool? IsActive { get; private set; }

        private Vendor(string name, string vendorCode)
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

            Name = name;
            VendorCode = vendorCode;
            IsActive = true;
        }

        public static Result<Vendor> Create(string name, string vendorCode)
        {
            if (string.IsNullOrWhiteSpace(name))
                return Result.Failure<Vendor>(RequiredMessage);

            if (string.IsNullOrWhiteSpace(vendorCode))
                return Result.Failure<Vendor>(RequiredMessage);

            name = (name ?? string.Empty).Trim();
            vendorCode = (vendorCode ?? string.Empty).Trim();

            if (name.Length < MinimumLength ||
                name.Length > MaximumLength ||
                vendorCode.Length < MinimumLength ||
                vendorCode.Length > MaximumLength)
                return Result.Failure<Vendor>(InvalidLengthMessage);

            return Result.Success(new Vendor(name, vendorCode));
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

        public void Enable() => IsActive = true;

        public void Disable() => IsActive = false;

        #region ORM

        // EF requires a parameterless constructor
        protected Vendor() { }

        #endregion
    }
}
