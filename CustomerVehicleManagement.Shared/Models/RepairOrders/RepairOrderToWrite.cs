using CustomerVehicleManagement.Shared.Models.RepairOrders.Payments;
using CustomerVehicleManagement.Shared.Models.RepairOrders.Services;
using CustomerVehicleManagement.Shared.Models.RepairOrders.Taxes;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace CustomerVehicleManagement.Shared.Models.RepairOrders
{
    public class RepairOrderToWrite// : INotifyPropertyChanged -- was trying to get the RO totals to update immediately
    {
        public long RepairOrderNumber { get; set; } = 0;
        public long InvoiceNumber { get; set; } = 0;
        public string CustomerName { get; set; } = string.Empty;
        public string Vehicle { get; set; } = string.Empty;
        public double PartsTotal { get; set; } = 0.0;
        public double LaborTotal { get; set; } = 0.0;
        public double DiscountTotal { get; set; } = 0.0;
        public double TaxTotal { get; set; } = 0.0;
        public double HazMatTotal { get; set; } = 0.0;
        public double ShopSuppliesTotal { get; set; } = 0.0;
        //private double _total { get; set; } = 0.0;
        //public double Total
        //{
        //    get { return _total; }
        //    set
        //    {
        //        this._total = value;
        //        NotifyPropertyChanged(nameof(RepairOrderToWrite.Total));
        //    }
        //}
        public double Total { get; set; } = 0.0;
        public DateTime? DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
        public DateTime? DateInvoiced { get; set; }

        public IList<RepairOrderServiceToWrite> Services { get; set; } = new List<RepairOrderServiceToWrite>();
        public IList<RepairOrderTaxToWrite> Taxes { get; set; } = new List<RepairOrderTaxToWrite>();
        public IList<RepairOrderPaymentToWrite> Payments { get; set; } = new List<RepairOrderPaymentToWrite>();

        //public event PropertyChangedEventHandler PropertyChanged;
        //private void NotifyPropertyChanged(string propertyName)
        //{
        //    var handler = PropertyChanged;
        //    if (handler != null)
        //    {
        //        handler(this, new PropertyChangedEventArgs(propertyName));
        //    }
        //}

        public void Recalculate()
        {
            PartsTotal = 0.0;
            LaborTotal = 0.0;
            DiscountTotal = 0.0;
            TaxTotal = 0.0;
            ShopSuppliesTotal = 0.0;

            if (Services?.Count > 0)
            {
                foreach (var service in Services)
                {
                    PartsTotal += service.PartsTotal;
                    LaborTotal += service.LaborTotal;
                    DiscountTotal += service.DiscountTotal;
                    ShopSuppliesTotal += service.ShopSuppliesTotal;
                }
            }

            if (Taxes?.Count > 0)
            {
                foreach (var tax in Taxes)
                {
                    TaxTotal += (tax.PartTax + tax.LaborTax);
                }
            }

            Total = PartsTotal + LaborTotal - DiscountTotal + ShopSuppliesTotal;
        }
    }
}
