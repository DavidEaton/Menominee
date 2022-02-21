using CustomerVehicleManagement.Domain.Enums;
using Menominee.Common;
using Menominee.Common.Enums;
using System.Collections.Generic;

namespace CustomerVehicleManagement.Domain.Entities.RepairOrders
{
    public class RepairOrderItem : Entity
    {
        public long RepairOrderServiceId { get; set; }
        public int SequenceNumber { get; set; }
        public virtual Manufacturer Manufacturer { get; set; }
        public long ManufacturerId { get; set; }
        public string PartNumber { get; set; }
        public string Description { get; set; }
        public virtual SaleCode SaleCode { get; set; }
        public long SaleCodeId { get; set; }
        public virtual ProductCode ProductCode { get; set; }
        public long ProductCodeId { get; set; }
        public SaleType SaleType { get; set; }
        public PartType PartType { get; set; }
        public bool IsDeclined { get; set; }
        public bool IsCounterSale { get; set; }
        public double QuantitySold { get; set; }
        public double SellingPrice { get; set; }
        public ItemLaborType LaborType { get; set; }
        public double LaborEach { get; set; }
        public double Cost { get; set; }
        public double Core { get; set; }
        public ItemDiscountType DiscountType { get; set; }
        public double DiscountEach { get; set; }
        public double Total { get; set; }

        public virtual IList<RepairOrderSerialNumber> SerialNumbers { get; set; } = new List<RepairOrderSerialNumber>();
        public virtual IList<RepairOrderWarranty> Warranties { get; set; } = new List<RepairOrderWarranty>();
        public virtual IList<RepairOrderItemTax> Taxes { get; set; } = new List<RepairOrderItemTax>();

        public void AddSerialNumber(RepairOrderSerialNumber serialNumber)
        {
            SerialNumbers.Add(serialNumber);
        }

        public void RemoveSerialNumber(RepairOrderSerialNumber serialNumber)
        {
            SerialNumbers.Remove(serialNumber);
        }

        public void SetSerialNumbers(IList<RepairOrderSerialNumber> serialNumbers)
        {
            SerialNumbers.Clear();
            if (serialNumbers.Count > 0)
            {
                foreach (var serialNumber in serialNumbers)
                    AddSerialNumber(serialNumber);
            }
        }

        public void AddWarranty(RepairOrderWarranty warranty)
        {
            Warranties.Add(warranty);
        }

        public void RemoveWarranty(RepairOrderWarranty warranty)
        {
            Warranties.Remove(warranty);
        }

        public void SetWarranties(IList<RepairOrderWarranty> warranties)
        {
            Warranties.Clear();
            if (warranties.Count > 0)
            {
                foreach (var warranty in warranties)
                    AddWarranty(warranty);
            }
        }

        public void AddTax(RepairOrderItemTax tax)
        {
            Taxes.Add(tax);
        }

        public void RemoveTax(RepairOrderItemTax tax)
        {
            Taxes.Remove(tax);
        }

        public void SetTaxes(IList<RepairOrderItemTax> taxes)
        {
            Taxes.Clear();
            if (taxes.Count > 0)
            {
                foreach (var tax in taxes)
                    AddTax(tax);
            }
        }

        #region ORM

        // EF requires an empty constructor
        public RepairOrderItem() { }

        #endregion

    }
}
