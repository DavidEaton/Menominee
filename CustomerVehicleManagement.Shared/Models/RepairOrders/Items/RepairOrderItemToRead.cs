using CustomerVehicleManagement.Shared.Models.Manufacturers;
using CustomerVehicleManagement.Shared.Models.ProductCodes;
using CustomerVehicleManagement.Shared.Models.RepairOrders.SerialNumbers;
using CustomerVehicleManagement.Shared.Models.RepairOrders.Taxes;
using CustomerVehicleManagement.Shared.Models.RepairOrders.Warranties;
using CustomerVehicleManagement.Shared.Models.SaleCodes;
using Menominee.Common.Enums;
using System.Collections.Generic;

namespace CustomerVehicleManagement.Shared.Models.RepairOrders.Items
{
    public class RepairOrderItemToRead
    {
        public long Id { get; set; }
        public long RepairOrderServiceId { get; set; }
        public ManufacturerToRead Manufacturer { get; set; }
        public long ManufacturerId { get; set; }
        public string PartNumber { get; set; }
        public string Description { get; set; }
        public SaleCodeToRead SaleCode { get; set; }
        public long SaleCodeId { get; set; }
        public ProductCodeToRead ProductCode { get; set; }
        public long ProductCodeId { get; set; }
        public SaleType SaleType { get; set; }
        public PartType PartType { get; set; }
        public bool IsDeclined { get; set; }
        public bool IsCounterSale { get; set; }
        public double QuantitySold { get; set; }
        public double SellingPrice { get; set; }
        public ItemLaborType LaborType { get; set; } = ItemLaborType.None;
        public double LaborEach { get; set; }
        public ItemDiscountType DiscountType { get; set; } = ItemDiscountType.None;
        public double DiscountEach { get; set; } = 0.0;
        public double Cost { get; set; }
        public double Core { get; set; }
        public double Total { get; set; }

        public List<RepairOrderSerialNumberToRead> SerialNumbers { get; set; } = new List<RepairOrderSerialNumberToRead>();
        public List<RepairOrderWarrantyToRead> Warranties { get; set; } = new List<RepairOrderWarrantyToRead>();
        public List<RepairOrderItemTaxToRead> Taxes { get; set; } = new List<RepairOrderItemTaxToRead>();
    }
}
