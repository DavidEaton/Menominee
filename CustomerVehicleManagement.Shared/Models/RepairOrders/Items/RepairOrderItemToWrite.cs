﻿using CustomerVehicleManagement.Shared.Models.RepairOrders.SerialNumbers;
using CustomerVehicleManagement.Shared.Models.RepairOrders.Taxes;
using CustomerVehicleManagement.Shared.Models.RepairOrders.Warranties;
using Menominee.Common.Enums;
using System.Collections.Generic;

namespace CustomerVehicleManagement.Shared.Models.RepairOrders.Items
{
    public class RepairOrderItemToWrite
    {
        public long Id { get; set; } = 0;
        public long RepairOrderServiceId { get; set; } = 0;
        public int SequenceNumber { get; set; } = 0;
        public string ManufacturerId { get; set; } = string.Empty;
        public string PartNumber { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string SaleCode { get; set; } = string.Empty;
        public string ProductCode { get; set; } = string.Empty;
        public SaleType SaleType { get; set; } = SaleType.Regular;
        public PartType PartType { get; set; } = PartType.Part;
        public bool IsDeclined { get; set; } = false;
        public bool IsCounterSale { get; set; } = false;
        public double QuantitySold { get; set; } = 0.0;
        public double SellingPrice { get; set; } = 0.0;
        public double LaborEach { get; set; } = 0.0;
        public double Cost { get; set; } = 0.0;
        public double Core { get; set; } = 0.0;
        public double Total { get; set; } = 0.0;

        public IList<RepairOrderSerialNumberToWrite> SerialNumbers { get; set; } = new List<RepairOrderSerialNumberToWrite>();
        public IList<RepairOrderWarrantyToWrite> Warranties { get; set; } = new List<RepairOrderWarrantyToWrite>();
        public IList<RepairOrderItemTaxToWrite> Taxes { get; set; } = new List<RepairOrderItemTaxToWrite>();
    }
}