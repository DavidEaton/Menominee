using CustomerVehicleManagement.Shared.Models.RepairOrders;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Menominee.Client.Components.RepairOrders
{
    public partial class RepairOrderForm
    {
        [Parameter]
        public RepairOrderToWrite RepairOrder { get; set; }

        [Parameter]
        public string Title { get; set; } //= "RO #123123123   ~   Jane Doe   ~   2019 Dodge Durango";

        [Parameter]
        public EventCallback OnDiscard { get; set; }

        [Parameter]
        public EventCallback OnSave { get; set; }

        List<Inspection> CurrentInspections { get; set; }
        List<Inspection> PreviousInspections { get; set; }
        List<Purchase> Purchases { get; set; }
        List<SerialNumber> SerialNumbers { get; set; }
        List<Warranty> Warranties { get; set; }
        //List<Payment> Payments { get; set; }

        protected override void OnInitialized()
        {
            Inspection inspection;
            Purchase purchase;
            SerialNumber serialNumber;
            Warranty warranty;
            //Payment payment;

            CurrentInspections = new List<Inspection>();

            inspection = new Inspection();
            inspection.Id = 1;
            inspection.Title = "Courtesy Check";
            inspection.Date = DateTime.Today;
            inspection.Odometer = 109244;
            inspection.Tech = "101";
            inspection.Status = "Completed";
            CurrentInspections.Add(inspection);

            inspection = new Inspection();
            inspection.Id = 2;
            inspection.Title = "Brake Inspection";
            inspection.Date = DateTime.Today;
            inspection.Odometer = 109244;
            inspection.Tech = "476";
            inspection.Status = "Not Started";
            CurrentInspections.Add(inspection);

            PreviousInspections = new List<Inspection>();

            inspection = new Inspection();
            inspection.Id = 3;
            inspection.Title = "Courtesy Check";
            inspection.Date = DateTime.Today.AddDays(-36);
            inspection.Odometer = 96801;
            inspection.Tech = "266";
            inspection.Status = "Completed";
            PreviousInspections.Add(inspection);

            Purchases = new List<Purchase>();

            purchase = new Purchase();
            purchase.Id = 1;
            purchase.PartNumber = "BP1234";
            purchase.Description = "Brake Pads";
            purchase.Quantity = 1;
            purchase.VendorName = "ABC Parts Warehouse";
            purchase.VendorCost = 21.76;
            Purchases.Add(purchase);

            purchase = new Purchase();
            purchase.Id = 2;
            purchase.PartNumber = "CL9876";
            purchase.Description = "Clamp";
            purchase.Quantity = 4;
            purchase.VendorName = "";
            purchase.VendorCost = 0;
            Purchases.Add(purchase);

            purchase = new Purchase();
            purchase.Id = 3;
            purchase.PartNumber = "WB445566";
            purchase.Description = "Wiper Blade";
            purchase.Quantity = 2;
            purchase.VendorName = "PDQ Parts Supplier";
            purchase.VendorCost = 7.34;
            Purchases.Add(purchase);

            SerialNumbers = new List<SerialNumber>();

            serialNumber = new SerialNumber();
            serialNumber.Id = 1;
            serialNumber.SequenceNumber = 1;
            serialNumber.PartNumber = "AT12123";
            serialNumber.Description = "All Terrain Tire";
            serialNumber.Quantity = 1;
            serialNumber.SerialNum = "XXX111111111";
            SerialNumbers.Add(serialNumber);

            serialNumber = new SerialNumber();
            serialNumber.Id = 1;
            serialNumber.SequenceNumber = 2;
            serialNumber.PartNumber = "AT12123";
            serialNumber.Description = "All Terrain Tire";
            serialNumber.Quantity = 1;
            serialNumber.SerialNum = "YYY22222";
            SerialNumbers.Add(serialNumber);

            serialNumber = new SerialNumber();
            serialNumber.Id = 1;
            serialNumber.SequenceNumber = 3;
            serialNumber.PartNumber = "AT12123";
            serialNumber.Description = "All Terrain Tire";
            serialNumber.Quantity = 1;
            serialNumber.SerialNum = "ZZZ3333333";
            SerialNumbers.Add(serialNumber);

            serialNumber = new SerialNumber();
            serialNumber.Id = 1;
            serialNumber.SequenceNumber = 4;
            serialNumber.PartNumber = "AT12123";
            serialNumber.Description = "All Terrain Tire";
            serialNumber.Quantity = 1;
            serialNumber.SerialNum = "";
            SerialNumbers.Add(serialNumber);

            Warranties = new List<Warranty>();

            warranty = new Warranty();
            warranty.Id = 1;
            warranty.SequenceNumber = 1;
            warranty.Type = WarrantyType.GuaranteedReplacement;
            warranty.PartNumber = "BP1234";
            warranty.Description = "Brake Pad";
            warranty.Quantity = 1;
            warranty.WarrantyNumber = "XXX111111111";
            Warranties.Add(warranty);

            warranty = new Warranty();
            warranty.Id = 2;
            warranty.SequenceNumber = 1;
            warranty.Type = WarrantyType.NewWarranty;
            warranty.PartNumber = "WC97531";
            warranty.Description = "Wheel Cylinder";
            warranty.Quantity = 1;
            warranty.WarrantyNumber = "DFG01386";
            Warranties.Add(warranty);

        }

        protected override void OnParametersSet()
        {
            // replaced these once correct fields are in place
            string title = $"RO #{RepairOrder.Id}";
            if (RepairOrder.CustomerName.Length > 0)
                title += $"   ~   {RepairOrder.CustomerName}";
            if (RepairOrder.Vehicle.Length > 0)
                title += $"   ~   {RepairOrder.Vehicle}";
            Title = title;
        }

        //private RepairOrderTab SelectedTab { get; set; }

        private bool CustSelected { get; set; } = true;
        private bool FleetSelected { get; set; }
        private bool FleetVisible { get; set; } = false;
        private bool ServiceRequestSelected { get; set; }
        private bool InspectionsSelected { get; set; }
        private bool ServicesSelected { get; set; }
        private bool PurchasesSelected { get; set; }
        private bool WarrantiesSelected { get; set; }
        private bool SerialNumbersSelected { get; set; }
        private bool PaymentSelected { get; set; }

        private int PurchaseInfoNeededCount { get; set; } = 1;
        private int WarrantyInfoNeededCount { get; set; } = 0;
        private int SerialNumberInfoNeededCount { get; set; } = 1;

        public bool HavePurchases()
        {
            return true;
        }

        public bool HaveWarranties()
        {
            return true;
        }

        public bool HaveSerialNumbers()
        {
            return true;
        }
    }

    public class Inspection
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public long Odometer { get; set; }
        public DateTime Date { get; set; }
        public string Tech { get; set; }
        public string Status { get; set; }
    }

    public class Purchase
    {
        public long Id { get; set; }
        public string PartNumber { get; set; }
        public string Description { get; set; }
        public string VendorName { get; set; }
        public double Quantity { get; set; }
        public double VendorCost { get; set; }

        public bool IsComplete()
        {
            return VendorName.Length > 0 && VendorCost > 0.0;
        }
    }

    public class SerialNumber
    {
        public long Id { get; set; }
        public long SequenceNumber { get; set; }
        public string PartNumber { get; set; }
        public string Description { get; set; }
        public string SerialNum { get; set; }
        public double Quantity { get; set; }

        public bool IsComplete()
        {
            return SerialNum.Length > 0;
        }
    }

    public enum WarrantyType
    {
        [Display(Name = "New Warranty")]
        NewWarranty,
        [Display(Name = "Guaranteed Replacement")]
        GuaranteedReplacement,
        [Display(Name = "Defective Replacement")]
        DefectiveReplacement
    }

    public class Warranty
    {
        public long Id { get; set; }
        public long SequenceNumber { get; set; }
        public WarrantyType Type { get; set; }
        public string PartNumber { get; set; }
        public string Description { get; set; }
        public string WarrantyNumber { get; set; }
        public double Quantity { get; set; }

        public bool IsComplete()
        {
            return WarrantyNumber.Length > 0;
        }
    }

    //public class Payment
    //{
    //    public long Id { get; set; }
    //    public long SequenceNumber { get; set; }
    //    public string Method { get; set; }
    //    public double Amount { get; set; }
    //}
}
