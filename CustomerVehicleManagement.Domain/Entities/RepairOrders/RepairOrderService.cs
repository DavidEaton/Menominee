using Menominee.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerVehicleManagement.Domain.Entities.RepairOrders
{
    public class RepairOrderService : Entity
    {
        public long RepairOrderId { get; set; }
        public int SequenceNumber { get; set; }
        public string ServiceName { get; set; }
        public string SaleCode { get; set; }
        public bool IsCounterSale { get; set; }
        public bool IsDeclined { get; set; }
        public double PartsTotal { get; set; }
        public double LaborTotal { get; set; }
        public double TaxTotal { get; set; }
        public double ShopSuppliesTotal { get; set; }
        public double Total { get; set; }

        public virtual IList<RepairOrderItem> Items { get; set; } = new List<RepairOrderItem>();
        public virtual IList<RepairOrderTech> Techs { get; set; } = new List<RepairOrderTech>();
        public virtual IList<RepairOrderServiceTax> Taxes { get; set; } = new List<RepairOrderServiceTax>();

        public void AddItem(RepairOrderItem item)
        {
            Items.Add(item);
        }

        public void RemoveItem(RepairOrderItem item)
        {
            Items.Remove(item);
        }

        public void SetItems(IList<RepairOrderItem> items)
        {
            Items.Clear();
            if (items.Count > 0)
            {
                foreach (var item in items)
                    AddItem(item);
            }
        }

        public void AddTech(RepairOrderTech tech)
        {
            Techs.Add(tech);
        }

        public void RemoveTech(RepairOrderTech tech)
        {
            Techs.Remove(tech);
        }

        public void SetTechs(IList<RepairOrderTech> techs)
        {
            Techs.Clear();
            if (techs.Count > 0)
            {
                foreach (var tech in techs)
                    AddTech(tech);
            }
        }

        public void AddTax(RepairOrderServiceTax tax)
        {
            Taxes.Add(tax);
        }

        public void RemoveTax(RepairOrderServiceTax tax)
        {
            Taxes.Remove(tax);
        }

        public void SetTaxes(IList<RepairOrderServiceTax> taxes)
        {
            Taxes.Clear();
            if (taxes.Count > 0)
            {
                foreach (var tax in taxes)
                    AddTax(tax);
            }
        }
    }
}
