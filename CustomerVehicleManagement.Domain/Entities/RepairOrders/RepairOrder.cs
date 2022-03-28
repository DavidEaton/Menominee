using Menominee.Common;
using Menominee.Common.Utilities;
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

        public virtual IList<RepairOrderService> Services { get; set; } = new List<RepairOrderService>();
        public virtual IList<RepairOrderTax> Taxes { get; set; } = new List<RepairOrderTax>();
        public virtual IList<RepairOrderPayment> Payments { get; set; } = new List<RepairOrderPayment>();

        public void AddService(RepairOrderService service)
        {
            Guard.ForNull(service, "service");
            Services.Add(service);
        }

        public void RemoveService(RepairOrderService service)
        {
            Guard.ForNull(service, "service");
            Services.Remove(service);
        }

        public void SetServices(IList<RepairOrderService> services)
        {
            // Client may send an empty or null collection, signifying removal
            if (services is null || services?.Count == 0)
                Services = services;

            if (services?.Count > 0)
            {
                Services.Clear();
                foreach (var service in services)
                    AddService(service);
            }
        }

        public void AddTax(RepairOrderTax tax)
        {
            Guard.ForNull(tax, "tax");
            Taxes.Add(tax);
        }

        public void RemoveTax(RepairOrderTax tax)
        {
            Guard.ForNull(tax, "tax");
            Taxes.Remove(tax);
        }

        public void SetTaxes(IList<RepairOrderTax> taxes)
        {
            if (taxes is null || taxes?.Count == 0)
                Taxes = taxes;

            if (taxes?.Count > 0)
            {
                Taxes.Clear();
                foreach (var tax in taxes)
                    AddTax(tax);
            }
        }

        public void AddPayment(RepairOrderPayment payment)
        {
            Guard.ForNull(payment, "payment");
            Payments.Add(payment);
        }

        public void RemovePayment(RepairOrderPayment payment)
        {
            Guard.ForNull(payment, "payment");
            Payments.Remove(payment);
        }


        public void SetPayments(IList<RepairOrderPayment> payments)
        {
            if (payments is null || payments?.Count == 0)
                Payments = payments;

            Payments.Clear();
            if (payments.Count > 0)
            {
                foreach (var payment in payments)
                    AddPayment(payment);
            }
        }

        #region ORM

        // EF requires an empty constructor
        public RepairOrder() { }

        #endregion
    }
}
