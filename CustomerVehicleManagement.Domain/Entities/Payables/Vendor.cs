﻿using CSharpFunctionalExtensions;
using CustomerVehicleManagement.Domain.Entities.Inventory;
using Menominee.Common.Utilities;
using System;
using Entity = Menominee.Common.Entity;

namespace CustomerVehicleManagement.Domain.Entities.Payables
{
    public class Vendor : Entity
    {
        public static readonly string RequiredMessage = $"Please include all required items.";
        public static readonly int MinimumLength = 2;
        public static readonly int MaximumLength = 255;
        public static readonly string InvalidLengthMessage = $"Name, Code must be between {MinimumLength} character(s) {MaximumLength} and in length";
        public string Name { get; private set; }
        public string VendorCode { get; private set; }
        public bool? IsActive { get; private set; }

        private Vendor(string name, string vendorCode)
        {
            Name = name;
            VendorCode = vendorCode;
            IsActive = true; // We won't be creating any new inactive Vendors
        }

        public static Result<Vendor> Create(string name, string vendorCode)
        {
            if (string.IsNullOrWhiteSpace(name))
                return Result.Failure<Vendor>(RequiredMessage);

            if (string.IsNullOrWhiteSpace(vendorCode))
                return Result.Failure<Vendor>(RequiredMessage);

            name = (name ?? string.Empty).Trim();
            vendorCode = (vendorCode ?? string.Empty).Trim();

            if (name.Length < MinimumLength || name.Length > MaximumLength
                ||
                vendorCode.Length < MinimumLength || vendorCode.Length > MaximumLength)
            {
                return Result.Failure<Vendor>(InvalidLengthMessage);
            }

            return Result.Success(new Vendor(name, vendorCode));
        }

        public void SetName(string name)
        {
            Guard.ForNullOrEmpty(name, "Name");

            name = (name ?? string.Empty).Trim();

            if (name.Length < MinimumLength || name.Length > MaximumLength)
                throw new ArgumentOutOfRangeException(InvalidLengthMessage);

            Name = name;
        }
        public void SetVendorCode(string vendorCode)
        {
            Guard.ForNullOrEmpty(vendorCode, "VendorCode");

            vendorCode = (vendorCode ?? string.Empty).Trim();

            if (vendorCode.Length < MinimumLength || vendorCode.Length > MaximumLength)
                throw new ArgumentOutOfRangeException(InvalidLengthMessage);

            VendorCode = vendorCode;
        }

        public void Enable()
        {
            IsActive = true;
        }

        public void Disable()
        {
            IsActive = false;
        }

        #region ORM

        // EF requires an empty constructor
        public Vendor() { }

        #endregion
    }
}
