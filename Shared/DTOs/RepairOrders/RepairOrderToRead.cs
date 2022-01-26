using MenomineePlayWASM.Shared.Dtos.RepairOrders.Payments;
using MenomineePlayWASM.Shared.Dtos.RepairOrders.Services;
using MenomineePlayWASM.Shared.Dtos.RepairOrders.Taxes;
using MenomineePlayWASM.Shared.Entities.RepairOrders;
using System;
using System.Collections.Generic;

namespace MenomineePlayWASM.Shared.Dtos.RepairOrders
{
    public class RepairOrderToRead
    {
        public long Id { get; set; }
        public long RepairOrderNumber { get; set; }
        public long InvoiceNumber { get; set; }
        public string CustomerName { get; set; }
        public string Vehicle { get; set; }
        public double Total { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
        public DateTime? DateInvoiced { get; set; }

        public IReadOnlyList<RepairOrderServiceToRead> Services { get; set; } = new List<RepairOrderServiceToRead>();
        public IReadOnlyList<RepairOrderTaxToRead> Taxes { get; set; } = new List<RepairOrderTaxToRead>();
        public IReadOnlyList<RepairOrderPaymentToRead> Payments { get; set; } = new List<RepairOrderPaymentToRead>();

        public static RepairOrderToRead ConvertToDto(RepairOrder repairOrder)
        {
            if (repairOrder != null)
            {
                return new RepairOrderToRead()
                {
                    Id = repairOrder.Id,
                    RepairOrderNumber = repairOrder.RepairOrderNumber,
                    InvoiceNumber = repairOrder.InvoiceNumber,
                    CustomerName = repairOrder.CustomerName,
                    Vehicle = repairOrder.Vehicle,
                    Total = repairOrder.Total,
                    DateCreated = repairOrder.DateCreated,
                    DateModified = repairOrder.DateModified,
                    DateInvoiced = repairOrder.DateInvoiced,
                    Services = RepairOrderServiceToRead.ConvertToDto(repairOrder.Services),
                    Taxes = RepairOrderTaxToRead.ConvertToDto(repairOrder.Taxes),
                    Payments = RepairOrderPaymentToRead.ConvertToDto(repairOrder.Payments)
                };
            }

            return null;
        }
    }
}
