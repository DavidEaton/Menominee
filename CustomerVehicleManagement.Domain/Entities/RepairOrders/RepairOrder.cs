using Menominee.Common;
using System;
using System.Collections.Generic;

namespace CustomerVehicleManagement.Domain.Entities.RepairOrders
{
    public class RepairOrder : Entity
    {
        public long RepairOrderNumber { get; set; }
        public long InvoiceNumber { get; set; }
        public string CustomerName { get; set; }
        public string Vehicle { get; set; }
        public double PartsTotal { get; set; }
        public double LaborTotal { get; set; }
        public double DiscountTotal { get; set; }
        public double TaxTotal { get; set; }
        public double HazMatTotal { get; set; }
        public double ShopSuppliesTotal { get; set; }
        public double Total { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public DateTime DateInvoiced { get; set; }

        public virtual List<RepairOrderService> Services { get; set; } = new List<RepairOrderService>();
        public virtual List<RepairOrderTax> Taxes { get; set; } = new List<RepairOrderTax>();
        public virtual List<RepairOrderPayment> Payments { get; set; } = new List<RepairOrderPayment>();

        public void AddService(RepairOrderService service)
        {
            if (service is null)
                throw new ArgumentOutOfRangeException(nameof(service), "service");

            Services.Add(service);
        }

        public void RemoveService(RepairOrderService service)
        {
            if (service is null)
                throw new ArgumentOutOfRangeException(nameof(service), "service");

            Services.Remove(service);
        }

        public void AddTax(RepairOrderTax tax)
        {
            if (tax is null)
                throw new ArgumentOutOfRangeException(nameof(tax), "tax");

            Taxes.Add(tax);
        }

        public void RemoveTax(RepairOrderTax tax)
        {
            if (tax is null)
                throw new ArgumentOutOfRangeException(nameof(tax), "tax");

            Taxes.Remove(tax);
        }

        public void AddPayment(RepairOrderPayment payment)
        {
            if (payment is null)
                throw new ArgumentOutOfRangeException(nameof(payment), "payment");

            Payments.Add(payment);
        }

        public void RemovePayment(RepairOrderPayment payment)
        {
            if (payment is null)
                throw new ArgumentOutOfRangeException(nameof(payment), "payment");

            Payments.Remove(payment);
        }

        #region ORM

        // EF requires a parameterless constructor
        public RepairOrder() { }

        #endregion
    }
}
