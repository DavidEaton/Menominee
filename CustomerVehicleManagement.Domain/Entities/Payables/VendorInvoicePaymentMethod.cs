using CSharpFunctionalExtensions;
using Entity = Menominee.Common.Entity;

namespace CustomerVehicleManagement.Domain.Entities.Payables
{
    public class VendorInvoicePaymentMethod : Entity
    {
        public static readonly int MinimumLength = 2;
        public static readonly int MaximumLength = 255;
        public static readonly string InvalidMessage = $"Payment Method Name must be between {MinimumLength} and {MaximumLength} character(s) in length.";

        public string Name { get; private set; }
        public bool IsActive { get; private set; }
        public bool IsOnAccountPaymentType { get; private set; }
        public Vendor ReconcilingVendor { get; private set; }

        private VendorInvoicePaymentMethod(
            string name,
            bool isActive,
            bool isOnAccountPaymentType,
            Vendor reconcilingVendor)
        {
            Name = name;
            IsActive = isActive;
            IsOnAccountPaymentType = isOnAccountPaymentType;
            ReconcilingVendor = reconcilingVendor;
        }

        public static Result<VendorInvoicePaymentMethod> Create(
            string name,
            bool isActive,
            bool isOnAccountPaymentType,
            Vendor reconcilingVendor)
        {
            name = (name ?? string.Empty).Trim();

            if (name.Length < MinimumLength || name.Length > MaximumLength)
                return Result.Failure<VendorInvoicePaymentMethod>($"{InvalidMessage} You entered {name.Length} character(s).");

            return Result.Success(new VendorInvoicePaymentMethod(
                name, isActive, isOnAccountPaymentType, reconcilingVendor));
        }

        #region ORM

        // EF requires an empty constructor
        public VendorInvoicePaymentMethod() { }

        #endregion
    }
}
