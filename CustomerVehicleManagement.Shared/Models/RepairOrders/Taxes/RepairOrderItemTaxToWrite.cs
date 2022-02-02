﻿namespace CustomerVehicleManagement.Shared.Models.RepairOrders.Taxes
{
    public class RepairOrderItemTaxToWrite
    {
        public long Id { get; set; } = 0;
        public long RepairOrderItemId { get; set; } = 0;
        public int SequenceNumber { get; set; } = 0;
        public long TaxId { get; set; } = 0;
        public double PartTaxRate { get; set; } = 0.0;
        public double LaborTaxRate { get; set; } = 0.0;
        public double PartTax { get; set; } = 0.0;
        public double LaborTax { get; set; } = 0.0;
    }
}
