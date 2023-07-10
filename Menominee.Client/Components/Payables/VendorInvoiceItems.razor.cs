using Menominee.Shared.Models.Payables.Invoices.LineItems;
using Menominee.Common.Enums;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telerik.Blazor;
using Telerik.Blazor.Components;

namespace Menominee.Client.Components.Payables
{
    public partial class VendorInvoiceItems : ComponentBase
    {
        [Parameter]
        public IList<VendorInvoiceLineItemToWrite> LineItems { get; set; }

        [CascadingParameter]
        public DialogFactory Dialogs { get; set; }

        [Parameter]
        public Action OnCalculateTotals { get; set; }

        [CascadingParameter]
        public FormMode FormMode { get; set; }

        private bool CanEdit { get; set; } = false;

        public IEnumerable<VendorInvoiceLineItemToWrite> SelectedItems { get; set; } = Enumerable.Empty<VendorInvoiceLineItemToWrite>();
        public VendorInvoiceLineItemToWrite SelectedItem { get; set; }
        public VendorInvoiceLineItemToWrite ItemToModify { get; set; } = null;

        public TelerikGrid<VendorInvoiceLineItemToWrite> Grid { get; set; }

        public int SelectedItemIndex
        {
            get => selectedItemIndex;
            set
            {
                selectedItemIndex = value;
                CanEditItem = selectedItemIndex >= 0;
                CanDeleteItem = CanEdit && selectedItemIndex >= 0;
            }
        }

        private long itemIdToSelect { get; set; } = 0;
        private int selectedItemIndex = -1;
        private bool editDialogVisible = false;

        private bool CanEditItem { get; set; } = false;
        private bool CanDeleteItem { get; set; } = false;

        private FormMode ItemFormMode { get; set; } = FormMode.Unknown;
        private bool EditDialogVisible
        {
            get => editDialogVisible;
            set
            {
                if (value == true)
                {
                    if (ItemFormMode == FormMode.Add)
                        ItemToModify = new VendorInvoiceLineItemToWrite();

                    if (ItemFormMode == FormMode.Edit || ItemFormMode == FormMode.View)
                    {
                        ItemToModify = new VendorInvoiceLineItemToWrite();
                        CopyItem(SelectedItem, ItemToModify);
                    }
                }
                else
                {
                    if (ItemToModify is not null)
                        ItemToModify = null;
                    ItemFormMode = FormMode.Unknown;
                }

                editDialogVisible = value;
            }
        }

        protected override void OnParametersSet()
        {
            CanEdit = FormMode == FormMode.Add || FormMode == FormMode.Edit;

            if (LineItems?.Count > 0)
            {
                if (itemIdToSelect == 0)
                    SelectedItem = LineItems.FirstOrDefault();

                //if (itemToSelect != 0)
                //    SelectedItem = Items.Where(x => x.Id == itemToSelect).FirstOrDefault();

                SelectedItemIndex = LineItems.IndexOf(SelectedItem);
                SelectedItems = new List<VendorInvoiceLineItemToWrite> { SelectedItem };
            }
        }

        private void OnEdit()
        {
            if (CanEditItem)
            {
                ItemFormMode = CanEdit ? FormMode.Edit : FormMode.View;
                EditDialogVisible = true;
            }
        }

        private void OnNew()
        {
            ItemFormMode = FormMode.Add;
            EditDialogVisible = true;
        }

        private async Task OnDelete()
        {
            if (SelectedItem is not null
            && await Dialogs.ConfirmAsync($"Are you sure you want to remove {SelectedItem.Item.PartNumber}?", "Remove Item"))
            {
                LineItems.Remove(SelectedItem);
                SelectedItem = LineItems.FirstOrDefault();
                SelectedItems = new List<VendorInvoiceLineItemToWrite> { SelectedItem };
                SelectedItemIndex = LineItems.IndexOf(SelectedItem);
                Grid.Rebind();
            }
        }

        private void OnSaveEdit()
        {
            if (ItemFormMode is not FormMode.Add && ItemFormMode is not FormMode.Edit)
                return;

            if (ItemFormMode == FormMode.Add)
            {
                LineItems.Add(ItemToModify);
                SelectedItemIndex = LineItems.IndexOf(ItemToModify);
                SelectedItem = LineItems[SelectedItemIndex];
                SelectedItems = new List<VendorInvoiceLineItemToWrite> { SelectedItem };
                Grid.Rebind();
            }
            else if (ItemFormMode == FormMode.Edit)
            {
                CopyItem(ItemToModify, LineItems[SelectedItemIndex]);
            }
            EditDialogVisible = false;
            OnCalculateTotals?.Invoke();
            StateHasChanged();
        }

        private void OnCancelEdit()
        {
            ItemFormMode = FormMode.Unknown;
            EditDialogVisible = false;
        }

        private void OnRowSelected(GridRowClickEventArgs args)
        {
            SelectedItem = args.Item as VendorInvoiceLineItemToWrite;
            SelectedItemIndex = LineItems.IndexOf(SelectedItem);
            SelectedItems = new List<VendorInvoiceLineItemToWrite> { SelectedItem };
        }

        private static void CopyItem(VendorInvoiceLineItemToWrite src, VendorInvoiceLineItemToWrite dst)
        {
            dst.Type = src.Type;
            dst.Item = src.Item;
            dst.Quantity = src.Quantity;
            dst.Cost = src.Cost;
            dst.Core = src.Core;
            dst.PONumber = src.PONumber;
            dst.TransactionDate = src.TransactionDate;
        }
    }
}
