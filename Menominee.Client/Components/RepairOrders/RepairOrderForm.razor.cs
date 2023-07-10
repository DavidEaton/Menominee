using Menominee.Shared.Models.RepairOrders;
using Menominee.Shared.Models.RepairOrders.Purchases;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
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

        private int PurchasesMissingCount { get; set; } = 0;
        private int WarrantiesMissingCount { get; set; } = 0;
        private int SerialNumbersMissingCount { get; set; } = 0;

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
            //warranty.Type = WarrantyType.GuaranteedReplacement;
            //warranty.PartNumber = "BP1234";
            //warranty.Description = "Brake Pad";
            //warranty.Quantity = 1;
            //warranty.WarrantyNumber = "XXX111111111";
            //Warranties.Add(warranty);

            //warranty = new Warranty();
            //warranty.Id = 2;
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
                : RepairOrderToEdit = RepairOrderHelper.CovertReadToWriteDto(RepairOrder);

            // replace these once correct fields are in place
            var title = $"RO #{RandomInt()}";

            if (RepairOrder?.Customer?.Name?.Length > 0)
                title += $"   ~   {RepairOrder?.Customer?.Name}";
            if (RepairOrder?.Vehicle is not null)
                title += $"   ~   {RepairOrder?.Vehicle?.ToString()}";

            Title = title;

            SerialNumbersMissingCount = RepairOrderHelper.SerialNumberRequiredMissingCount(RepairOrderToEdit.Services);
            WarrantiesMissingCount = RepairOrderHelper.WarrantyRequiredMissingCount(RepairOrderToEdit.Services);
            PurchasesMissingCount = RepairOrderHelper.PurchaseRequiredMissingCount(RepairOrderToEdit.Services);
        }

        private void UpdateSerialNumbersMissingCount(int count)
        {
            SerialNumbersMissingCount = count;
        }

        private void UpdateWarrantiesMissingCount(int count)
        {
            WarrantiesMissingCount = count;
        }

        private void UpdatePurchasesMissingCount(int count)
        {
            PurchasesMissingCount = count;
        }

        private async Task Save()
        {
            RemoveIncompleteSerialNumbers();
            RemoveIncompleteWarranties();
            RemoveIncompletePurchases();

            if (Valid())
            {
                if (Id == 0)
                    await DataService.AddRepairOrder(RepairOrderToEdit);

                if (Id != 0)
                    await DataService.UpdateRepairOrder(RepairOrderToEdit, Id);

                await OnSave.InvokeAsync();
            }
        }

        private void RemoveIncompletePurchases()
        {
            foreach (var service in RepairOrderToEdit?.Services)
            {
                foreach (var item in service?.LineItems)
                {
                    if (item?.Purchases is not null)
                        item.Purchases.RemoveAll(purchase =>
                                                     string.IsNullOrWhiteSpace(purchase.VendorPartNumber));
                }
            }
        }

        private void RemoveIncompleteSerialNumbers()
        {
            foreach (var service in RepairOrderToEdit?.Services)
            {
                foreach (var item in service?.LineItems)
                {
                    if (item?.SerialNumbers is not null)
                        item.SerialNumbers.RemoveAll(serialNumber =>
                                                     string.IsNullOrWhiteSpace(serialNumber.SerialNumber));
                }
            }
        }

        private void RemoveIncompleteWarranties()
        {
            foreach (var service in RepairOrderToEdit?.Services)
            {
                foreach (var item in service?.LineItems)
                {
                    if (item?.Warranties is not null)
                        item.Warranties.RemoveAll(warranty =>
                                                  warranty.Quantity == 0);
                }
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

        public bool HasPurchases()
        {
            return true;
        }

        public bool HasWarranties()
        {
            return RepairOrder.Services
                .Select(service =>
                        service.Items
                        .Select(item =>
                                item.Warranties.Any())).Any();
        }

        public bool HasSerialNumbers()
        {
            return RepairOrder.Services
                .Select(service =>
                        service.Items
                        .Select(item =>
                                item.SerialNumbers.Any())).Any();
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

    //public class Payment
    //{
    //    public long Id { get; set; }
    //    public string Method { get; set; }
    //    public double Amount { get; set; }
    //}
}
