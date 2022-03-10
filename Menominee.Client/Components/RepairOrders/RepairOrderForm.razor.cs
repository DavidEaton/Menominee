using CustomerVehicleManagement.Shared.Helpers;
using CustomerVehicleManagement.Shared.Models.RepairOrders;
using CustomerVehicleManagement.Shared.Models.RepairOrders.Items;
using Menominee.Client.Services.RepairOrders;
using Menominee.Common.Enums;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Menominee.Client.Components.RepairOrders
{
    public partial class RepairOrderForm
    {
        public RepairOrderToWrite RepairOrderToEdit { get; set; }

        [Inject]
        private IRepairOrderDataService DataService { get; set; }

        [Parameter]
        public long Id { get; set; }

        [Parameter]
        public RepairOrderToRead RepairOrder { get; set; }

        [Parameter]
        public string Title { get; set; } //= "RO #123123123   ~   Jane Doe   ~   2019 Dodge Durango";

        [Parameter]
        public EventCallback OnDiscard { get; set; }

        [Parameter]
        public EventCallback OnSave { get; set; }

        List<Inspection> CurrentInspections { get; set; }
        List<Inspection> PreviousInspections { get; set; }
        List<PurchaseListItem> PurchaseList { get; set; } = new();
        List<SerialNumberListItem> SerialNumberList { get; set; } = new();
        List<WarrantyListItem> WarrantyList { get; set; } = new();
        //List<Payment> Payments { get; set; }

        protected override void OnInitialized()
        {
            Inspection inspection;
            //PurchaseListItem purchase;
            //SerialNumberListItem serialNumber;
            //Warranty warranty;
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

            //PurchasesList = new List<PurchaseListItem>();

            //purchase = new PurchaseListItem();
            //purchase.Id = 1;
            //purchase.PartNumber = "BP1234";
            //purchase.Description = "Brake Pads";
            //purchase.Quantity = 1;
            //purchase.VendorName = "ABC Parts Warehouse";
            //purchase.VendorCost = 21.76;
            //PurchasesList.Add(purchase);

            //purchase = new PurchaseListItem();
            //purchase.Id = 2;
            //purchase.PartNumber = "CL9876";
            //purchase.Description = "Clamp";
            //purchase.Quantity = 4;
            //purchase.VendorName = "";
            //purchase.VendorCost = 0;
            //PurchasesList.Add(purchase);

            //purchase = new PurchaseListItem();
            //purchase.Id = 3;
            //purchase.PartNumber = "WB445566";
            //purchase.Description = "Wiper Blade";
            //purchase.Quantity = 2;
            //purchase.VendorName = "PDQ Parts Supplier";
            //purchase.VendorCost = 7.34;
            //PurchasesList.Add(purchase);

            //serialNumber = new SerialNumberListItem();
            //serialNumber.Id = 1;
            //serialNumber.PartNumber = "AT12123";
            //serialNumber.Description = "All Terrain Tire";
            //serialNumber.SerialNum = "XXX111111111";
            //SerialNumberList.Add(serialNumber);

            //serialNumber = new SerialNumberListItem();
            //serialNumber.Id = 2;
            //serialNumber.PartNumber = "AT12123";
            //serialNumber.Description = "All Terrain Tire";
            //serialNumber.SerialNum = "YYY22222";
            //SerialNumberList.Add(serialNumber);

            //serialNumber = new SerialNumberListItem();
            //serialNumber.Id = 3;
            //serialNumber.PartNumber = "AT12123";
            //serialNumber.Description = "All Terrain Tire";
            //serialNumber.SerialNum = "ZZZ3333333";
            //SerialNumberList.Add(serialNumber);

            //serialNumber = new SerialNumberListItem();
            //serialNumber.Id = 4;
            //serialNumber.PartNumber = "AT12123";
            //serialNumber.Description = "All Terrain Tire";
            //serialNumber.SerialNum = "";
            //SerialNumberList.Add(serialNumber);

            //Warranties = new List<Warranty>();

            //warranty = new Warranty();
            //warranty.Id = 1;
            //warranty.SequenceNumber = 1;
            //warranty.Type = WarrantyType.GuaranteedReplacement;
            //warranty.PartNumber = "BP1234";
            //warranty.Description = "Brake Pad";
            //warranty.Quantity = 1;
            //warranty.WarrantyNumber = "XXX111111111";
            //Warranties.Add(warranty);

            //warranty = new Warranty();
            //warranty.Id = 2;
            //warranty.SequenceNumber = 1;
            //warranty.Type = WarrantyType.NewWarranty;
            //warranty.PartNumber = "WC97531";
            //warranty.Description = "Wheel Cylinder";
            //warranty.Quantity = 1;
            //warranty.WarrantyNumber = "DFG01386";
            //Warranties.Add(warranty);

        }

        protected override void OnParametersSet()
        {
            RepairOrderToEdit = Id == 0
                ? RepairOrderToEdit = new()
                : RepairOrderToEdit = RepairOrderHelper.ConvertReadDtoToWriteDto(RepairOrder);

            // replace these once correct fields are in place
            string title = $"RO #{RandomInt()}";
            if (RepairOrder.CustomerName.Length > 0)
                title += $"   ~   {RepairOrder.CustomerName}";
            if (RepairOrder.Vehicle.Length > 0)
                title += $"   ~   {RepairOrder.Vehicle}";
            Title = title;

            BuildSerialNumberList();
        }

        private async Task Save()
        {
            if (Valid())
            {
                if (Id == 0)
                {
                    await DataService.AddRepairOrder(RepairOrderToEdit);
                }
                else
                {
                    await DataService.UpdateRepairOrder(RepairOrderToEdit, Id);
                }

                await OnSave.InvokeAsync();
            }
        }
        private bool Valid()
        {
            //if (Invoice.VendorId > 0 && Invoice.Date.HasValue)
            //    return true;

            //return false;
            return true;
        }

        private static int RandomInt()
        {
            var random = new Random();
            return random.Next();
        }

        private bool CustomerSelected { get; set; } = true;
        private bool FleetSelected { get; set; }
        private bool FleetVisible { get; set; } = false;
        private bool ServiceRequestSelected { get; set; }
        private bool InspectionsSelected { get; set; }
        private bool ServicesSelected { get; set; }
        private bool PurchasesSelected { get; set; }
        private bool WarrantiesSelected { get; set; }
        private bool SerialNumbersSelected { get; set; }
        private bool PaymentSelected { get; set; }

        private int PurchaseInfoNeededCount { get; set; } = 0;
        private int WarrantyInfoNeededCount { get; set; } = 0;
        private int SerialNumberInfoNeededCount { get; set; } = 0;

        public void SerialNumbersUpdated()
        {
            // This should fire on RepairOrderSerialNumberEditor.OnSave
            // fires RepairOrderSerialNumbersTab.Save
            // invokes RepairOrderForm.SerialNumbersUpdated (this method)
            var services = RepairOrder.Services;

            List<RepairOrderItemToWrite> items = new();
            //foreach (var item in services[0].Items)
            //{
            //    items.Add(item);
            //}

            var originalItems = items;
            var SerialNumbersToUpdate = SerialNumberList;

            foreach (var item in originalItems)
            {
                Console.WriteLine($"Part Number: {item.PartNumber}");
                Console.WriteLine($"Part Number: {item.SerialNumbers}");
            }

            // Do NOT save the entire form ti the dtaa service yet!!!
            //OnSave.InvokeAsync();
        }

        private void BuildSerialNumberList()
        {
            // Search through each item on each service to find the ones needing serial numbers
            if (RepairOrder?.Services?.Count > 0)
            {
                foreach (var service in RepairOrder.Services)
                {
                    if (service?.Items.Count > 0)
                    {
                        foreach (var item in service.Items)
                        {
                            // check if serial numbers are required on this item
                            if (SerialNumberRequired(item))
                            {
                                SerialNumberListItem serialNumber = new SerialNumberListItem
                                {
                                    ItemId = item.Id,
                                    PartNumber = item.PartNumber,
                                    Description = item.Description
                                    //SerialNum = string.Empty
                                };
                                SerialNumberList.Add(serialNumber);
                            }
                        }
                    }
                }
            }

            SerialNumberInfoNeededCount = SerialNumberList.FindAll(serialNumberListItem =>
                                                                   string.IsNullOrWhiteSpace(serialNumberListItem.SerialNumber))
                                                                   .Count;
        }

        private bool SerialNumberRequired(RepairOrderItemToRead item)
        {
            if ((item.PartType == PartType.Part || item.PartType == PartType.Tire) && item.QuantitySold > 0)
            {
                // check if this part's product code requires serial numbers
                // if (ProductCodeRequiresSerialNumber())
                return true;
            }
            return false;
        }

        private void BuildPurchaseList()
        {
            var tempId = 0;

            // Search through each item on each service to find the ones needing purchase info
            //if (RepairOrder?.Services?.Count > 0)
            //{
            //    foreach (var service in RepairOrder.Services)
            //    {
            //        if (service.Items?.Count > 0)
            //        {
            //            foreach (var item in service.Items)
            //            {
            //                // check if purchase info is required on this item
            //                if (PurchaseInfoRequired(item))
            //                {
            //                    PurchaseListItem purchase = new PurchaseListItem
            //                    {
            //                        Id = ++tempId,
            //                        VendorId = 0,
            //                        ItemId = item.Id,
            //                        PartNumber = item.PartNumber,
            //                        Description = item.Description,

            //                        VendorName = string.Empty,
            //                        VendorPartNumber = item.PartNumber,
            //                        VendorInvoiceNumber = string.Empty,
            //                        PONumber = string.Empty,
            //                        FileCost = item.Cost,
            //                        VendorCost = 0.0,
            //                        VendorCore = 0.0,
            //                        DatePurchased = DateTime.Today
            //                    };

            //                    PurchaseList.Add(purchase);
            //                }
            //            }
            //        }
            //    }
            //}
            PurchaseInfoNeededCount = PurchaseList.FindAll(
                                    delegate (PurchaseListItem purchase) { return !purchase.IsComplete(); }).Count;
        }

        private bool PurchaseInfoRequired(RepairOrderItemToWrite item)
        {
            return ((item.PartType == PartType.Part || item.PartType == PartType.Tire) && item.QuantitySold > 0 /*&& item.IsBuyout*/);
        }

        private void BuildWarrantyList()
        {
            var tempId = 0;

            // Search through each item on each service to find the ones needing warranty info
            //if (RepairOrder?.Services?.Count > 0)
            //{
            //    foreach (var service in RepairOrder.Services)
            //    {
            //        if (service.Items?.Count > 0)
            //        {
            //            foreach (var item in service.Items)
            //            {
            //                var nextSequenceNumber = 0;

            //                // check if warranty info is required on this item
            //                if (WarrantyInfoRequired(item))
            //                {
            //                    WarrantyListItem warranty = new WarrantyListItem
            //                    {
            //                        Id = ++tempId,
            //                        ItemId = item.Id,
            //                        PartNumber = item.PartNumber,
            //                        Description = item.Description,
            //                        SequenceNumber = ++nextSequenceNumber,
            //                        Type = WarrantyType.NewWarranty,
            //                        WarrantyNumber = string.Empty,
            //                        Quantity = item.QuantitySold
            //                    };

            //                    WarrantyList.Add(warranty);
            //                }
            //            }
            //        }
            //    }
            //}
            WarrantyInfoNeededCount = WarrantyList.FindAll(
                                    delegate (WarrantyListItem warranty) { return !warranty.IsComplete(); }).Count;
        }

        private bool WarrantyInfoRequired(RepairOrderItemToWrite item)
        {
            // FIX ME -
            // if (ProductCodeRequiresWarranty())
            return ((item.PartType == PartType.Part || item.PartType == PartType.Tire) && item.QuantitySold > 0);
        }

        public bool HavePurchases()
        {
            return PurchaseList.Count > 0;
        }

        public bool HaveWarranties()
        {
            return WarrantyList.Count > 0;
        }

        public bool HaveSerialNumbers()
        {
            return SerialNumberList.Count > 0;
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

    public class PurchaseListItem
    {
        public long Id { get; set; }
        public long ItemId { get; set; }
        public long VendorId { get; set; }
        public string PartNumber { get; set; }
        public string Description { get; set; }
        public string VendorName { get; set; }
        public string VendorPartNumber { get; set; }
        public string VendorInvoiceNumber { get; set; }
        public string PONumber { get; set; }
        public double Quantity { get; set; }
        public double FileCost { get; set; }
        public double VendorCost { get; set; }
        public double VendorCore { get; set; }
        public DateTime? DatePurchased { get; set; }

        public bool IsComplete()
        {
            return !string.IsNullOrWhiteSpace(VendorName) && !string.IsNullOrWhiteSpace(VendorInvoiceNumber) && VendorCost > 0.0;
        }
    }

    public class SerialNumberListItem
    {
        public long Id { get; set; }
        public long ItemId { get; set; }
        public string PartNumber { get; set; }
        public string Description { get; set; }
        public string SerialNumber { get; set; }

        public bool IsComplete()
        {
            return !string.IsNullOrWhiteSpace(SerialNumber);
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

    public class WarrantyListItem
    {
        public long Id { get; set; }
        public long ItemId { get; set; }
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
