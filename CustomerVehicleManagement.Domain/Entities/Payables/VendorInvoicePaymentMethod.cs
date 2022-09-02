using CSharpFunctionalExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Entity = Menominee.Common.Entity;

namespace CustomerVehicleManagement.Domain.Entities.Payables
{
    public class VendorInvoicePaymentMethod : Entity
    {
        public static readonly string RequiredMessage = $"Please include all required items.";
        public static readonly int MinimumLength = 1;
        public static readonly int MaximumLength = 40;
        public static readonly string InvalidLengthMessage = $"Payment Method Name must be between {MinimumLength} and {MaximumLength} character(s) in length.";
        public static readonly string UniqueMessage = $"Payment Method already exists. Payment Method Name must be unique.";

        public string Name { get; private set; }
        public bool IsActive { get; private set; } // This is basically a charge-type payment method.  Eventually it will become a line item on a vendor statement (similar to an invoice). -Al
        public bool IsOnAccountPaymentType { get; private set; }
        public Vendor ReconcilingVendor { get; private set; }

        private VendorInvoicePaymentMethod(
            string name,
            bool isActive,
            bool isOnAccountPaymentType,
            Vendor reconcilingVendor)
        {
            if (name is null)
                throw new ArgumentOutOfRangeException(RequiredMessage);

            name = (name ?? string.Empty).Trim();

            if (name.Length > MaximumLength || name.Length < MinimumLength)
                throw new ArgumentOutOfRangeException(InvalidLengthMessage);

            Name = name;
            IsActive = isActive;
            IsOnAccountPaymentType = isOnAccountPaymentType;
            ReconcilingVendor = reconcilingVendor;
        }

        public static Result<VendorInvoicePaymentMethod> Create(
            IEnumerable<string> paymentMethods,
            string name,
            bool isActive,
            bool isOnAccountPaymentType,
            Vendor reconcilingVendor)
        {
            if (name is null)
                return Result.Failure<VendorInvoicePaymentMethod>(RequiredMessage);

             name = (name ?? string.Empty).Trim();

           if (name.Length > MaximumLength || name.Length < MinimumLength)
                return Result.Failure<VendorInvoicePaymentMethod>(InvalidLengthMessage);

            if (paymentMethods.Contains(name))
                return Result.Failure<VendorInvoicePaymentMethod>(UniqueMessage);

            if (name.Length < MinimumLength || name.Length > MaximumLength)
                return Result.Failure<VendorInvoicePaymentMethod>($"{InvalidLengthMessage} You entered {name.Length} character(s).");

            return Result.Success(new VendorInvoicePaymentMethod(
                name, isActive, isOnAccountPaymentType, reconcilingVendor));
        }

        public void SetName(string name)
        {
            if (name is null)
                throw new ArgumentOutOfRangeException(RequiredMessage);

            name = (name ?? string.Empty).Trim();

            if (name.Length > MaximumLength || name.Length < MinimumLength)
                throw new ArgumentOutOfRangeException(InvalidLengthMessage);

            Name = name;
        }

        public void Enable()
        {
            IsActive = true;
        }

        public void Disable()
        {
            IsActive = false;
        }

        public void SetIsOnAccountPaymentType(bool isOnAccountPaymentType)
        {
            IsOnAccountPaymentType = isOnAccountPaymentType;
        }

        public void SetReconcilingVendor(Vendor reconcilingVendor)
        {
            if (reconcilingVendor is null)
                throw new ArgumentOutOfRangeException(RequiredMessage);

            ReconcilingVendor = reconcilingVendor;
        }

        #region ORM

        // EF requires an empty constructor
        public VendorInvoicePaymentMethod() { }

        #endregion
    }
}
