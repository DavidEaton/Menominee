﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MenomineePlayWASM.Shared.Entities.RepairOrders
{
    public class RepairOrderItemTax : Entity
    {
        public long RepairOrderItemId { get; set; }
        public int SequenceNumber { get; set; }
        public long TaxId { get; set; }
        public double PartTaxRate { get; set; }
        public double LaborTaxRate { get; set; }
        public double PartTax { get; set; }
        public double LaborTax { get; set; }
    }
}
