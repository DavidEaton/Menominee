using CustomerVehicleManagement.Shared.Models.Payables.Invoices.LineItems;
using Menominee.Common.Enums;
using Microsoft.AspNetCore.Components;
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

        public IEnumerable<VendorInvoiceLineItemToWrite> SelectedItems { get; set; } = Enumerable.Empty<VendorInvoiceLineItemToWrite>();
        public VendorInvoiceLineItemToWrite SelectedItem { get; set; }
        public VendorInvoiceLineItemToWrite ItemToModify { get; set; } = null;

        public TelerikGrid<VendorInvoiceLineItemToWrite> Grid { get; set; }

        public long SelectedId
        {
            get => selectedId;
            set
            {
                selectedId = value;
                CanEdit = selectedId != 0;
                CanDelete = selectedId != 0;
            }
        }

        private long itemIdToSelect { get; set; } = 0;
        private long selectedId = 0;
        private long nextId = 0;
        private int selectedItemIndex = 0;
        private bool editDialogVisible = false;

        private bool CanEdit { get; set; } = false;
        private bool CanDelete { get; set; } = false;

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

        private void OnEdit()
        {
            ItemFormMode = FormMode.Edit;
            EditDialogVisible = true;
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
                selectedItemIndex = LineItems.IndexOf(ItemToModify);
                SelectedItem = LineItems[selectedItemIndex];
                SelectedItems = new List<VendorInvoiceLineItemToWrite> { SelectedItem };
                Grid.Rebind();
            }
            else if (ItemFormMode == FormMode.Edit)
            {
                //Items[selectedItemIndex] = ItemToModify;
                //SelectedItem = ItemToModify;
                CopyItem(ItemToModify, LineItems[selectedItemIndex]);
            }
            //SelectedId = SelectedItem.Id;
            EditDialogVisible = false;
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
            //SelectedId = SelectedItem.Id;
            selectedItemIndex = LineItems.IndexOf(SelectedItem);
            SelectedItems = new List<VendorInvoiceLineItemToWrite> { SelectedItem };
        }

        protected void OnSelect(IEnumerable<VendorInvoiceLineItemToWrite> items)
        {
            //SelectedItem = items.FirstOrDefault();
            //SelectedItems = new List<VendorInvoiceItemToWrite> { SelectedItem };
        }

        protected override void OnInitialized()
        {
            if (LineItems.Count > 0)
            {
                if (itemIdToSelect == 0)
                    SelectedItem = LineItems.FirstOrDefault();

                //if (itemToSelect != 0)
                //    SelectedItem = Items.Where(x => x.Id == itemToSelect).FirstOrDefault();

                selectedItemIndex = LineItems.IndexOf(SelectedItem);
                //SelectedId = SelectedItem.Id;
                SelectedItems = new List<VendorInvoiceLineItemToWrite> { SelectedItem };
            }
        }

        //private static List<VendorInvoiceItem> FormatReturnData(List<VendorInvoiceItem> items)
        //{
        //    foreach (var item in items)
        //    {
        //        item.DateAsString = item.Date?.ToShortDateString();
        //    }

        //    return items;
        //}

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
