using CSharpFunctionalExtensions;
using CustomerVehicleManagement.Domain.Entities.Inventory;
using Menominee.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using Entity = Menominee.Common.Entity;

namespace CustomerVehicleManagement.Domain.Entities.RepairOrders
{
    // TODO: DDD: Rename this class to ServiceLine
    public class RepairOrderLineItem : Entity
    {
        public static readonly string RequiredMessage = $"Please include all required items.";
        public static readonly int MinimumLength = 2;
        public static readonly int MaximumLength = 255;
        public static readonly string InvalidLengthMessage = $"Must be between {MinimumLength} character(s) {MaximumLength} and in length";

        // TODO: Separate Line Item from Item (part):
        // RepairOrderItem, RepairOrderLineItem. DONE -DE
        // Invariant: check if this part's ProductCode requires
        // serial numbers to be entered for EACH sold.
        // For example, if three of same part are sold,
        // three serial numbers are required.
        public RepairOrderItem Item { get; private set; } //required
        public SaleType SaleType { get; private set; } //required
        public bool IsDeclined { get; private set; }
        public bool IsCounterSale { get; private set; }
        public double QuantitySold { get; private set; }
        public double SellingPrice { get; private set; }
        public LaborAmount LaborAmount { get; set; }
        public double Cost { get; private set; }
        public double Core { get; private set; }
        public DiscountAmount DiscountAmount { get; private set; }
        public double TotalAmount => (LaborAmount.Amount + SellingPrice + DiscountAmount.Amount) * QuantitySold;
        public double TotalTax => Taxes.Select(
            tax => (tax.PartTax.Amount + tax.PartTax.Amount) * QuantitySold).Sum();

        private readonly List<RepairOrderSerialNumber> serialNumbers = new();
        public IReadOnlyList<RepairOrderSerialNumber> SerialNumbers => serialNumbers.ToList();

        private readonly List<RepairOrderWarranty> warranties = new();
        public IReadOnlyList<RepairOrderWarranty> Warranties => warranties.ToList();

        private readonly List<RepairOrderItemTax> taxes = new();
        public IReadOnlyList<RepairOrderItemTax> Taxes => taxes.ToList();

        private readonly List<RepairOrderPurchase> purchases = new();
        public IReadOnlyList<RepairOrderPurchase> Purchases => purchases.ToList();

        private RepairOrderLineItem(
            RepairOrderItem item,
            SaleType saleType,
            bool isDeclined,
            bool isCounterSale,
            double quantitySold,
            double sellingPrice,
            LaborAmount laborAmount,
            double cost,
            double core,
            DiscountAmount discountAmount
        )
        {
            Item = item;
            SaleType = saleType;
            IsDeclined = isDeclined;
            IsCounterSale = isCounterSale;
            QuantitySold = quantitySold;
            SellingPrice = sellingPrice;
            LaborAmount = laborAmount;
            Cost = cost;
            Core = core;
            DiscountAmount = discountAmount;
        }
        public static Result<RepairOrderLineItem> Create(
            RepairOrderItem item,
            SaleType saleType,
            bool isDeclined,
            bool isCounterSale,
            double quantitySold,
            double sellingPrice,
            LaborAmount laborAmount,
            double cost,
            double core,
            DiscountAmount discountAmount
        )
        {
            // Validation...
            // TODO: awaiting invariants spreadsheet from Al
            if (item is null)
                return Result.Failure<RepairOrderLineItem>(RequiredMessage);

            if (!Enum.IsDefined(typeof(SaleType), saleType))
                return Result.Failure<RepairOrderLineItem>(RequiredMessage);

            // LaborAmount and DiscountAmount have already been validated by Fluentvalidation

            return Result.Success(new RepairOrderLineItem(item, saleType, isDeclined, isCounterSale, quantitySold, sellingPrice, laborAmount, cost, core, discountAmount));
        }


        public Result<RepairOrderItem> SetItem(RepairOrderItem item)
        {
            if (item is null)
                return Result.Failure<RepairOrderItem>(RequiredMessage);

            return Result.Success(Item = item);
        }

        public Result<SaleType> SetSaleType(SaleType saleType)
        {
            if (!Enum.IsDefined(typeof(SaleType), saleType))
                return Result.Failure<SaleType>(RequiredMessage);

            return Result.Success(SaleType = saleType);
        }

        public Result<bool> SetIsDeclined(bool isDeclined) =>
            Result.Success(IsDeclined = isDeclined);

        public Result<bool> SetIsCounterSale(bool isCounterSale) =>
            Result.Success(IsCounterSale = isCounterSale);

        public Result<double> SetQuantitySold(double quantitySold)
        {
            return Result.Success(QuantitySold = quantitySold);
        }

        public Result<double> SetSellingPrice(double sellingPrice)
        {
            return Result.Success(SellingPrice = sellingPrice);
        }

        public Result<LaborAmount> SetLaborAmount(LaborAmount laborAmount)
        {
            if (laborAmount is null)
                return Result.Failure<LaborAmount>(RequiredMessage);

            return Result.Success(LaborAmount = laborAmount);
        }

        public Result<double> SetCost(double cost)
        {
            return Result.Success(Cost = cost);
        }

        public Result<double> SetCore(double core)
        {
            return Result.Success(Core = core);
        }

        public Result<DiscountAmount> SetDiscountAmount(DiscountAmount discountAmount)
        {
            if (discountAmount is null)
                return Result.Failure<DiscountAmount>(RequiredMessage);

            return Result.Success(DiscountAmount = discountAmount);
        }

        public Result<RepairOrderSerialNumber> AddSerialNumber(RepairOrderSerialNumber serialNumber)
        {
            if (serialNumber is null)
                return Result.Failure<RepairOrderSerialNumber>(RequiredMessage);

            serialNumbers.Add(serialNumber);

            return Result.Success(serialNumber);
        }

        public Result<RepairOrderSerialNumber> RemoveSerialNumber(RepairOrderSerialNumber serialNumber)
        {
            if (serialNumber is null)
                return Result.Failure<RepairOrderSerialNumber>(RequiredMessage);

            serialNumbers.Remove(serialNumber);

            return Result.Success(serialNumber);
        }

        public Result<RepairOrderWarranty> AddWarranty(RepairOrderWarranty warranty)
        {
            if (warranty is null)
                return Result.Failure<RepairOrderWarranty>(RequiredMessage);

            warranties.Add(warranty);

            return Result.Success(warranty);
        }

        public Result<RepairOrderWarranty> RemoveWarranty(RepairOrderWarranty warranty)
        {
            if (warranty is null)
                return Result.Failure<RepairOrderWarranty>(RequiredMessage);

            warranties.Remove(warranty);

            return Result.Success(warranty);
        }

        public Result<RepairOrderItemTax> AddTax(RepairOrderItemTax tax)
        {
            if (tax is null)
                return Result.Failure<RepairOrderItemTax>(RequiredMessage);

            taxes.Add(tax);

            return Result.Success(tax);

        }

        public Result<RepairOrderItemTax> RemoveTax(RepairOrderItemTax tax)
        {
            if (tax is null)
                return Result.Failure<RepairOrderItemTax>(RequiredMessage);

            taxes.Remove(tax);

            return Result.Success(tax);
        }

        public Result<RepairOrderPurchase> AddPurchase(RepairOrderPurchase purchase)
        {
            if (purchase is null)
                return Result.Failure<RepairOrderPurchase>(RequiredMessage);

            purchases.Add(purchase);

            return Result.Success(purchase);
        }

        public Result<RepairOrderPurchase> RemovePurchase(RepairOrderPurchase purchase)
        {
            if (purchase is null)
                return Result.Failure<RepairOrderPurchase>(RequiredMessage);

            purchases.Remove(purchase);

            return Result.Success(purchase);
        }

        #region ORM

        // EF requires a parameterless constructor
        protected RepairOrderLineItem() { }

        #endregion

    }
}
