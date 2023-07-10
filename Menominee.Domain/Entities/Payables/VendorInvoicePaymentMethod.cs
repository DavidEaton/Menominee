using CSharpFunctionalExtensions;
using Menominee.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using Entity = Menominee.Common.Entity;

namespace Menominee.Domain.Entities.Payables
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
        // TODO: public bool IsOnAccountPaymentType { get; private set; } // This is basically a charge-type payment method.  Eventually it will become a line item on a vendor statement (similar to an invoice). -Al
        public VendorInvoicePaymentMethodType PaymentType { get; private set; }
        public Vendor ReconcilingVendor { get; private set; }

        private VendorInvoicePaymentMethod(
            IReadOnlyList<string> paymentMethods,
            string name,
            bool isActive,
            VendorInvoicePaymentMethodType paymentType,
            Vendor reconcilingVendor = null)
        {
            Name = name;
            IsActive = isActive;
            PaymentType = paymentType;
            ReconcilingVendor = reconcilingVendor;
        }

        public static Result<VendorInvoicePaymentMethod> Create(
            IReadOnlyList<string> paymentMethodNames,
            string name,
            bool isActive,
            VendorInvoicePaymentMethodType paymentType,
            Vendor reconcilingVendor = null)
        {
            name = (name ?? string.Empty).Trim();

            if (name.Length < MinimumLength || name.Length > MaximumLength)
                return Result.Failure<VendorInvoicePaymentMethod>($"{InvalidLengthMessage} You entered {name.Length} character(s).");

            if (paymentMethodNames.Contains(name))
                return Result.Failure<VendorInvoicePaymentMethod>(NonuniqueMessage);

            if (!Enum.IsDefined(typeof(VendorInvoicePaymentMethodType), paymentType))
                return Result.Failure<VendorInvoicePaymentMethod>(RequiredMessage);

            return Result.Success(new VendorInvoicePaymentMethod(
                paymentMethodNames, name, isActive, paymentType, reconcilingVendor));
        }

        public Result<string> SetName(string name, IReadOnlyList<string> paymentMethods)
        {
            name = (name ?? string.Empty).Trim();

            if (name.Length > MaximumLength || name.Length < MinimumLength)
                return Result.Failure<string>(InvalidLengthMessage);

            if (paymentMethods.Contains(name))
                return Result.Failure<string>(NonuniqueMessage);

            return Result.Success(Name = name);
        }

        public void Activate()
        {
            IsActive = true;
        }

        public void Deactivate()
        {
            IsActive = false;
        }

        public Result RemoveReconcilingVendor()
        {
            return Result.Success(ReconcilingVendor = null);
        }

        public Result<Vendor> SetReconcilingVendor(Vendor reconcilingVendor)
        {
            if (reconcilingVendor is null)
                return Result.Failure<Vendor>(RequiredMessage);

            return Result.Success(ReconcilingVendor = reconcilingVendor);
        }

        public Result<VendorInvoicePaymentMethodType> SetPaymentType(VendorInvoicePaymentMethodType paymentType)
        {
            if (!Enum.IsDefined(typeof(VendorInvoicePaymentMethodType), paymentType))
                return Result.Failure<VendorInvoicePaymentMethodType>(RequiredMessage);

            return Result.Success(PaymentType = paymentType);
        }

        #region ORM

        // EF requires a parameterless constructor
        protected VendorInvoicePaymentMethod() { }

        #endregion
    }
}
