using CSharpFunctionalExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using Entity = Menominee.Common.Entity;

namespace CustomerVehicleManagement.Domain.Entities.Payables
{
    public class VendorInvoicePaymentMethod : Entity
    {
        public static readonly string RequiredMessage = $"Please include all required items.";
        public static readonly int MinimumLength = 1;
        public static readonly int MaximumLength = 40;
        public static readonly string InvalidLengthMessage = $"Payment Method Name must be between {MinimumLength} and {MaximumLength} character(s) in length.";
        public static readonly string NonuniqueMessage = $"Payment Method already exists. Payment Method Name must be unique.";

        public string Name { get; private set; }
        public bool IsActive { get; private set; }
        public bool IsOnAccountPaymentType { get; private set; } // This is basically a charge-type payment method.  Eventually it will become a line item on a vendor statement (similar to an invoice). -Al
        public Vendor ReconcilingVendor { get; private set; }

        private VendorInvoicePaymentMethod(
            IList<string> paymentMethods,
            string name,
            bool isActive,
            bool isOnAccountPaymentType,
            Vendor reconcilingVendor)
        {
            name = (name ?? string.Empty).Trim();

            if (paymentMethods.Contains(name))
                throw new ArgumentOutOfRangeException(NonuniqueMessage);

            if (name.Length > MaximumLength || name.Length < MinimumLength)
                throw new ArgumentOutOfRangeException(InvalidLengthMessage);

            Name = name;
            IsActive = isActive;
            IsOnAccountPaymentType = isOnAccountPaymentType;
            ReconcilingVendor = reconcilingVendor;
        }

        public static Result<VendorInvoicePaymentMethod> Create(
            IList<string> paymentMethods,
            string name,
            bool isActive,
            bool isOnAccountPaymentType,
            Vendor reconcilingVendor)
        {
            name = (name ?? string.Empty).Trim();

            if (name.Length < MinimumLength || name.Length > MaximumLength)
                return Result.Failure<VendorInvoicePaymentMethod>($"{InvalidLengthMessage} You entered {name.Length} character(s).");

            if (paymentMethods.Contains(name))
                return Result.Failure<VendorInvoicePaymentMethod>(NonuniqueMessage);

            return Result.Success(new VendorInvoicePaymentMethod(
                paymentMethods, name, isActive, isOnAccountPaymentType, reconcilingVendor));
        }

        public Result<string> SetName(string name, IEnumerable<string> paymentMethods)
        {
            name = (name ?? string.Empty).Trim();

            if (name.Length > MaximumLength || name.Length < MinimumLength)
                return Result.Failure<string>(InvalidLengthMessage);

            if (paymentMethods.Contains(name))
                return Result.Failure<string>(NonuniqueMessage);

            return Result.Success(Name = name);
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

        public Result<Vendor> SetReconcilingVendor(Vendor reconcilingVendor)
        {
            if (reconcilingVendor is null)
                return Result.Failure<Vendor>(RequiredMessage);

            return Result.Success(ReconcilingVendor = reconcilingVendor);
        }

        #region ORM

        // EF requires a parameterless constructor
        protected VendorInvoicePaymentMethod() { }

        #endregion
    }
}
