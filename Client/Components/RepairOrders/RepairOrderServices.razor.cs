using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MenomineePlayWASM.Client.Components.RepairOrders
{
    public partial class RepairOrderServices
    {
        List<Service> Services { get; set; }

        protected override void OnInitialized()
        {
            Services = GenerateData();
        }

        public string ValueOrBlank(double val)
        {
            if (val == 0.0)
                return "";
            return val.ToString("C");// string.Format("C", Convert.ToString(val));
        }

        private List<Service> GenerateData()
        {
            List<Service> data = new List<Service>();
            Service service;
            ServiceItem item;

            service = new Service();
            service.Items = new List<ServiceItem>();
            service.Id = 1;
            service.Order = 1;
            service.SaleCode = "B";
            service.Name = "Brakes";
            service.Parts = 100.00;
            service.Labor = 50.00;
            service.HazMat = 3.0;
            service.Discount = 0.0;
            service.Supplies = 2.99;
            service.Tax = 10.0;
            service.Total = 162.99;

            item = new ServiceItem();
            item.PartNumber = "BP1000";
            item.Description = "Brake Pads";
            item.SaleType = "R";
            item.QuantitySold = 2.0;
            item.Each = 10.99;
            item.Labor = 0.0;
            item.HazMat = 2.0;
            item.Discount = 0.0;
            item.Total = 21.98;
            service.Items.Add(item);

            item = new ServiceItem();
            item.PartNumber = "BP1000";
            item.Description = "Brake Pads";
            item.SaleType = "R";
            item.QuantitySold = 2.0;
            item.Each = 10.99;
            item.Labor = 0.0;
            item.HazMat = 0.0;
            item.Discount = 0.0;
            item.Total = 21.98;
            service.Items.Add(item);

            item = new ServiceItem();
            item.PartNumber = "BP1000";
            item.Description = "Brake Pads";
            item.SaleType = "R";
            item.QuantitySold = 2.0;
            item.Each = 10.99;
            item.Labor = 0.0;
            item.HazMat = 1.0;
            item.Discount = 0.0;
            item.Total = 21.98;
            service.Items.Add(item);

            item = new ServiceItem();
            item.PartNumber = "BP1000";
            item.Description = "Brake Pads";
            item.SaleType = "R";
            item.QuantitySold = 2.0;
            item.Each = 10.99;
            item.Labor = 0.0;
            item.HazMat = 0.0;
            item.Discount = 0.0;
            item.Total = 21.98;
            service.Items.Add(item);

            data.Add(service);

            service = new Service();
            service.Items = new List<ServiceItem>();
            service.Id = 2;
            service.Order = 2;
            service.SaleCode = "L";
            service.Name = "Lube/Oil/Filter";
            service.Parts = 100.00;
            service.Labor = 50.00;
            service.HazMat = 0.0;
            service.Discount = 0.0;
            service.Supplies = 2.99;
            service.Tax = 10.0;
            service.Total = 162.99;

            item = new ServiceItem();
            item.PartNumber = "BP1000";
            item.Description = "Brake Pads";
            item.SaleType = "R";
            item.QuantitySold = 2.0;
            item.Each = 10.99;
            item.Labor = 0.0;
            item.HazMat = 0.0;
            item.Discount = 0.0;
            item.Total = 21.98;
            service.Items.Add(item);

            item = new ServiceItem();
            item.PartNumber = "BP1000";
            item.Description = "Brake Pads";
            item.SaleType = "R";
            item.QuantitySold = 2.0;
            item.Each = 10.99;
            item.Labor = 0.0;
            item.HazMat = 0.0;
            item.Discount = 0.0;
            item.Total = 21.98;
            service.Items.Add(item);

            data.Add(service);

            return data;
        }

        public class Service
        {
            public long Id { get; set; }
            public int Order { get; set; }
            public string SaleCode { get; set; }
            [DisplayFormat(DataFormatString = "{0:C}")]
            public string Name { get; set; }
            [DisplayFormat(DataFormatString = "{0:C}")]
            public double Parts { get; set; }
            [DisplayFormat(DataFormatString = "{0:C}")]
            public double Labor { get; set; }
            [DisplayFormat(DataFormatString = "{0:C}")]
            public double HazMat { get; set; }
            [DisplayFormat(DataFormatString = "{0:C}")]
            public double Discount { get; set; }
            [DisplayFormat(DataFormatString = "{0:C}")]
            public double Supplies { get; set; }
            [DisplayFormat(DataFormatString = "{0:C}")]
            public double Tax { get; set; }
            [DisplayFormat(DataFormatString = "{0:C}")]
            public double Total { get; set; }
            public string Techs { get; set; }
            public List<ServiceItem> Items { get; set; }
        }

        public class ServiceItem
        {
            public long Id { get; set; }
            public long JobId { get; set; }
            public string PartNumber { get; set; }
            public string Description { get; set; }
            public string SaleType { get; set; }
            public double QuantitySold { get; set; }
            [DisplayFormat(DataFormatString = "{0:C}")]
            public double Each { get; set; }
            [DisplayFormat(DataFormatString = "{0:C}")]
            public double Labor { get; set; }
            [DisplayFormat(DataFormatString = "{0:C}")]
            public double HazMat { get; set; }
            [DisplayFormat(DataFormatString = "{0:C}")]
            public double Discount { get; set; }
            [DisplayFormat(DataFormatString = "{0:C}")]
            public double Total { get; set; }

        }
    }
}
