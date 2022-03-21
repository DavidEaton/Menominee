using Menominee.Client.Components.RepairOrders.Models;
using Menominee.Common.Enums;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Linq;
using Telerik.Blazor.Components;

namespace Menominee.Client.Components.RepairOrders
{
    public partial class RepairOrderPurchasesTab : ComponentBase
    {
        [Parameter]
        public List<PurchaseListItem> Purchases { get; set; } = null;
        private bool CanEdit { get; set; } = false;
        private bool CanCopy { get; set; } = false;
        private bool CanClear { get; set; } = false;

        // FIX ME - replace PurchaseListItem with DTO ???
        public IEnumerable<PurchaseListItem> SelectedPurchases { get; set; } = Enumerable.Empty<PurchaseListItem>();
        public PurchaseListItem SelectedPurchase { get; set; }
        public PurchaseListItem PurchaseToModify { get; set; } = null;
        public TelerikGrid<PurchaseListItem> PurchasesGrid { get; set; }

        public long SelectedId
        {
            get => selectedId;
            set
            {
                selectedId = value;
                CanEdit = selectedId != 0;
                CanCopy = selectedId != 0 && Purchases.Count > 0;
                CanClear = selectedId != 0;
            }
        }

        private long itemToSelect { get; set; } = 0;
        private long selectedId = 0;
        private int selectedItemIndex = 0;
        private bool EditDialogVisible { get; set; } = false;
        private FormMode PurchaseFormMode = FormMode.Unknown;

        protected override void OnInitialized()
        {
            if (Purchases.Count > 0)
            {
                if (itemToSelect == 0)
                {
                    SelectedPurchase = Purchases.FirstOrDefault();
                }
                //else
                //{
                //    SelectedPurchase = Purchases.Where(x => x.Id == itemToSelect).FirstOrDefault();
                //}
                selectedItemIndex = Purchases.IndexOf(SelectedPurchase);
                //SelectedId = SelectedPurchase.Id;
                SelectedPurchases = new List<PurchaseListItem> { SelectedPurchase };
            }
        }

        protected void OnSelect(IEnumerable<PurchaseListItem> purchase)
        {
            //SelectedItem = ros.FirstOrDefault();
            //SelectedList = new List<RepairOrderToReadInList> { SelectedItem };
        }

        private void OnRowSelected(GridRowClickEventArgs args)
        {
            SelectedPurchase = args.Item as PurchaseListItem;
            selectedItemIndex = Purchases.IndexOf(SelectedPurchase);
            SelectedPurchases = new List<PurchaseListItem> { SelectedPurchase };
        }

        private void OnEdit()
        {
            PurchaseToModify = new();
            CopyPurchase(SelectedPurchase, PurchaseToModify);
            PurchaseFormMode = FormMode.Edit;
            EditDialogVisible = true;
        }

        private void OnCopy()
        {
        }

        private void OnSaveEdit()
        {
            if (PurchaseFormMode != FormMode.Add && PurchaseFormMode != FormMode.Edit)
                return;

            var index = Purchases.IndexOf(SelectedPurchase);
            CopyPurchase(PurchaseToModify, SelectedPurchase);

            if (index >= 0)
                CopyPurchase(SelectedPurchase, Purchases[index]);

            EditDialogVisible = false;
            PurchaseFormMode = FormMode.Unknown;
            // FIX ME - trying to get the grid to reflect the changes immediately but this isn't working
            StateHasChanged();
            PurchasesGrid?.Rebind();
        }

        private void OnCancelEdit()
        {
            PurchaseFormMode = FormMode.Unknown;
            EditDialogVisible = false;
        }

        private void OnClear()
        {
        }

        private static void CopyPurchase(PurchaseListItem src, PurchaseListItem dst)
        {
            dst.ItemId = src.ItemId;
            dst.VendorId = src.VendorId;
            dst.PartNumber = src.PartNumber;
            dst.Description = src.Description;
            dst.VendorName = src.VendorName;
            dst.VendorPartNumber = src.VendorPartNumber;
            dst.VendorInvoiceNumber = src.VendorInvoiceNumber;
            dst.PONumber = src.PONumber;
            dst.Quantity = src.Quantity;
            dst.FileCost = src.FileCost;
            dst.VendorCost = src.VendorCost;
            dst.VendorCore = src.VendorCore;
            dst.DatePurchased = src.DatePurchased;
        }
    }
}
