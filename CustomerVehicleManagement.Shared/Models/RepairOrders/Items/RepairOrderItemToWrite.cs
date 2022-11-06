using CustomerVehicleManagement.Shared.Models.Manufacturers;
using CustomerVehicleManagement.Shared.Models.ProductCodes;
using CustomerVehicleManagement.Shared.Models.RepairOrders.Purchases;
using CustomerVehicleManagement.Shared.Models.RepairOrders.SerialNumbers;
using CustomerVehicleManagement.Shared.Models.RepairOrders.Taxes;
using CustomerVehicleManagement.Shared.Models.RepairOrders.Warranties;
using CustomerVehicleManagement.Shared.Models.SaleCodes;
using Menominee.Common.Enums;
using System.Collections.Generic;

namespace CustomerVehicleManagement.Shared.Models.RepairOrders.Items
{
    public class RepairOrderItemToWrite
    {
        public long Id { get; set; }
        public long RepairOrderServiceId { get; set; } = 0;
        public ManufacturerToRead Manufacturer { get; set; }
        public long ManufacturerId { get; set; } = 0;
        public string PartNumber { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public SaleCodeToRead SaleCode { get; set; }
        public ProductCodeToRead ProductCode { get; set; }
        public SaleType SaleType { get; set; } = SaleType.Regular;
        public PartType PartType { get; set; } = PartType.Part;
        public bool IsDeclined { get; set; } = false;
        public bool IsCounterSale { get; set; } = false;
        public double QuantitySold { get; set; } = 0.0;
        public double SellingPrice { get; set; } = 0.0;
        public ItemLaborType LaborType { get; set; } = ItemLaborType.None;
        public double LaborEach { get; set; } = 0.0;
        public ItemDiscountType DiscountType { get; set; } = ItemDiscountType.None;
        public double DiscountEach { get; set; } = 0.0;
        public double Cost { get; set; } = 0.0;
        public double Core { get; set; } = 0.0;
        public double Total { get; set; } = 0.0;

        public List<RepairOrderSerialNumberToWrite> SerialNumbers { get; set; } = new();
        public List<RepairOrderWarrantyToWrite> Warranties { get; set; } = new();
        public List<RepairOrderItemTaxToWrite> Taxes { get; set; } = new();
        public List<RepairOrderPurchaseToWrite> Purchases { get; set; } = new();

        // TODO: Move this logic down into the domain aggregate class: Domain.Entities.RepairOrders.RepairOrderItem.cs
        public void Recalculate()
        {
            Total = (SellingPrice + LaborEach - DiscountEach) * QuantitySold;
        }
    }
}
