using CustomerVehicleManagement.Domain.Entities.Inventory;
using Menominee.Common;
using Menominee.Common.Enums;
using System;
using System.Collections.Generic;

namespace CustomerVehicleManagement.Domain.Entities.RepairOrders
{
    public class RepairOrderItem : Entity
    {
        // TODO: DDD Notes
        // Invariant: check if this part's ProductCode requires serial numbers to be entered for EACH sold.
        // For example, if three of same part are sold, three serial numbers are required.
        public long RepairOrderServiceId { get; set; }
        public virtual Manufacturer Manufacturer { get; set; }
        public long ManufacturerId { get; set; }
        public string PartNumber { get; set; }
        public string Description { get; set; }
        public virtual SaleCode SaleCode { get; set; }
        public long SaleCodeId { get; set; }
        public virtual ProductCode ProductCode { get; set; }
        public long ProductCodeId { get; set; }
        public SaleType SaleType { get; set; }
        public PartType PartType { get; set; }
        public bool IsDeclined { get; set; }
        public bool IsCounterSale { get; set; }
        public double QuantitySold { get; set; }
        public double SellingPrice { get; set; }
        public ItemLaborType LaborType { get; set; }
        public double LaborEach { get; set; }
        public double Cost { get; set; }
        public double Core { get; set; }
        public ItemDiscountType DiscountType { get; set; }
        public double DiscountEach { get; set; }
        public double Total { get; set; }

        public virtual List<RepairOrderSerialNumber> SerialNumbers { get; set; } = new();
        public virtual List<RepairOrderWarranty> Warranties { get; set; } = new();
        public virtual List<RepairOrderItemTax> Taxes { get; set; } = new();
        public virtual List<RepairOrderPurchase> Purchases { get; set; } = new();

        public void AddSerialNumber(RepairOrderSerialNumber serialNumber)
        {
            if (serialNumber is null)
                throw new ArgumentOutOfRangeException(nameof(serialNumber), "serialNumber");

            SerialNumbers.Add(serialNumber);
        }

        public void RemoveSerialNumber(RepairOrderSerialNumber serialNumber)
        {
            if (serialNumber is null)
                throw new ArgumentOutOfRangeException(nameof(serialNumber), "serialNumber");
            SerialNumbers.Remove(serialNumber);
        }

        public void AddWarranty(RepairOrderWarranty warranty)
        {
            if (warranty is null)
                throw new ArgumentOutOfRangeException(nameof(warranty), "warranty");

            Warranties.Add(warranty);
        }

        public void RemoveWarranty(RepairOrderWarranty warranty)
        {
            if (warranty is null)
                throw new ArgumentOutOfRangeException(nameof(warranty), "warranty");

            Warranties.Remove(warranty);
        }

        public void AddTax(RepairOrderItemTax tax)
        {
            if (tax is null)
                throw new ArgumentOutOfRangeException(nameof(tax), "tax");

            Taxes.Add(tax);
        }

        public void RemoveTax(RepairOrderItemTax tax)
        {
            if (tax is null)
                throw new ArgumentOutOfRangeException(nameof(tax), "tax");

            Taxes.Remove(tax);
        }

        public void AddPurchase(RepairOrderPurchase purchase)
        {
            if (purchase is null)
                throw new ArgumentOutOfRangeException(nameof(purchase), "purchase");

            Purchases.Add(purchase);
        }

        public void RemovePurchase(RepairOrderPurchase purchase)
        {
            if (purchase is null)
                throw new ArgumentOutOfRangeException(nameof(purchase), "purchase");

            Purchases.Remove(purchase);
        }


        #region ORM

        // EF requires a parameterless constructor
        public RepairOrderItem() { }

        #endregion

    }
}
