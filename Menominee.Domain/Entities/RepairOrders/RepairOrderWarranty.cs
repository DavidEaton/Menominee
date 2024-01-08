using CSharpFunctionalExtensions;
using Menominee.Domain.Enums;
using System;
using Entity = Menominee.Domain.BaseClasses.Entity;

namespace Menominee.Domain.Entities.RepairOrders
{
    // TODO: DDD: Rename this class to ServiceLineWarranty
    public class RepairOrderWarranty : Entity
    {
        public static readonly string RequiredMessage = $"Please include all required items.";
        public static readonly int MinimumValue = -99999;
        public static readonly int MaximumValue = 99999;
        public static readonly string InvalidValueMessage = $"Value must be between {MinimumValue} and {MaximumValue}.";
        public static readonly int MinimumLength = 2;
        public static readonly int MaximumLength = 255;
        public static readonly string InvalidLengthMessage = $"Each item must be between {MinimumLength} and {MaximumLength} characters";
        public double Quantity { get; private set; }
        public WarrantyType Type { get; private set; }
        public string NewWarranty { get; private set; }
        public string OriginalWarranty { get; private set; }
        public long OriginalInvoiceId { get; private set; }

        private RepairOrderWarranty(double quantity, WarrantyType type, string newWarranty, string originalWarranty, long originalInvoiceId)
        {
            Quantity = quantity;
            Type = type;
            NewWarranty = newWarranty;
            OriginalWarranty = originalWarranty;
            OriginalInvoiceId = originalInvoiceId;
        }

        public static Result<RepairOrderWarranty> Create(
            double quantity,
            WarrantyType type,
            string newWarranty,
            string originalWarranty,
            long originalInvoiceId)
        {
            if (quantity > MaximumValue || quantity < MinimumValue)
                return Result.Failure<RepairOrderWarranty>(InvalidValueMessage);

            if (!Enum.IsDefined(typeof(WarrantyType), type))
                return Result.Failure<RepairOrderWarranty>(RequiredMessage);

            newWarranty = (newWarranty ?? string.Empty).Trim();
            originalWarranty = (originalWarranty ?? string.Empty).Trim();

            if (newWarranty.Length > MaximumLength
             || newWarranty.Length < MinimumLength
             || originalWarranty.Length > MaximumLength
             || originalWarranty.Length < MinimumLength)
                return Result.Failure<RepairOrderWarranty>(InvalidLengthMessage);

            return Result.Success(new RepairOrderWarranty(quantity, type, newWarranty, originalWarranty, originalInvoiceId));
        }

        public Result<double> SetQuantity(double quantity)
        {
            if (quantity > MaximumValue || quantity < MinimumValue)
                return Result.Failure<double>(InvalidValueMessage);

            return Result.Success(Quantity = quantity);
        }

        public Result<WarrantyType> SetType(WarrantyType type)
        {
            if (!Enum.IsDefined(typeof(WarrantyType), type))
                return Result.Failure<WarrantyType>(RequiredMessage);

            return Result.Success(Type = type);
        }

        public Result<string> SetNewWarranty(string newWarranty)
        {
            newWarranty = (newWarranty ?? string.Empty).Trim();

            if (newWarranty.Length > MaximumLength || newWarranty.Length < MinimumLength)
                return Result.Failure<string>(InvalidLengthMessage);

            return Result.Success(NewWarranty = newWarranty);
        }

        public Result<string> SetOriginalWarranty(string originalWarranty)
        {
            originalWarranty = (originalWarranty ?? string.Empty).Trim();

            if (originalWarranty.Length > MaximumLength || originalWarranty.Length < MinimumLength)
                return Result.Failure<string>(InvalidLengthMessage);

            return Result.Success(OriginalWarranty = originalWarranty);
        }

        #region ORM

        // EF requires a parameterless constructor
        protected RepairOrderWarranty() { }

        #endregion
    }
}
