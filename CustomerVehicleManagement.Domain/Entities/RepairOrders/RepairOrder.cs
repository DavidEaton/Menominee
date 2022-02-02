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
        public double Total { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public DateTime DateInvoiced { get; set; }

        public virtual IList<RepairOrderService> Services { get; set; } = new List<RepairOrderService>();
        public virtual IList<RepairOrderTax> Taxes { get; set; } = new List<RepairOrderTax>();
        public virtual IList<RepairOrderPayment> Payments { get; set; } = new List<RepairOrderPayment>();

        public void AddService(RepairOrderService service)
        {
            Services.Add(service);
        }

        public void RemoveService(RepairOrderService service)
        {
            Services.Remove(service);
        }

        public void SetServices(IList<RepairOrderService> services)
        {
            Services.Clear();
            if (services.Count > 0)
            {
                foreach (var service in services)
                    AddService(service);
            }
        }

        public void AddTax(RepairOrderTax tax)
        {
            Taxes.Add(tax);
        }

        public void RemoveTax(RepairOrderTax tax)
        {
            Taxes.Remove(tax);
        }

        public void SetTaxes(IList<RepairOrderTax> taxes)
        {
            Services.Clear();
            if (taxes.Count > 0)
            {
                foreach (var tax in taxes)
                    AddTax(tax);
            }
        }

        public void AddPayment(RepairOrderPayment payment)
        {
            Payments.Add(payment);
        }

        public void RemovePayment(RepairOrderPayment payment)
        {
            Payments.Remove(payment);
        }


        public void SetPayments(IList<RepairOrderPayment> payments)
        {
            Payments.Clear();
            if (payments.Count > 0)
            {
                foreach (var payment in payments)
                    AddPayment(payment);
            }
        }
    }
}
