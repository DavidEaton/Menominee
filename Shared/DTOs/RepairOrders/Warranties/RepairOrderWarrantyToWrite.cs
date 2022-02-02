﻿using MenomineePlayWASM.Shared.Entities.RepairOrders.Enums;

namespace MenomineePlayWASM.Shared.Dtos.RepairOrders.Warranties
{
    public class RepairOrderWarrantyToWrite
    {
        public long Id { get; set; } = 0;
        public long RepairOrderItemId { get; set; } = 0;
        public int SequenceNumber { get; set; } = 0;
        public double Quantity { get; set; } = 0.0;
        public WarrantyType Type { get; set; } = WarrantyType.NewWarranty;
        public string NewWarranty { get; set; } = string.Empty;
        public string OriginalWarranty { get; set; } = string.Empty;
        public long OriginalInvoiceId { get; set; } = 0;
    }
}