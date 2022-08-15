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
        public IList<VendorInvoiceLineItemToWrite> Items { get; set; }

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

        private long itemToSelect { get; set; } = 0;
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
                    {
                        ItemToModify = new VendorInvoiceLineItemToWrite();
                        ItemToModify.Id = --nextId;
                    }
                    if (ItemFormMode == FormMode.Edit || ItemFormMode == FormMode.View)
                    {
                        //ItemToModify = new VendorInvoiceItemToWrite()
                        //{
                        //    Id = SelectedItem.Id,
                        //    InvoiceId = SelectedItem.InvoiceId,
                        //    Type = SelectedItem.Type,
                        //    PartNumber = SelectedItem.PartNumber,
                        //    MfrId = SelectedItem.MfrId,
                        //    Description = SelectedItem.Description,
                        //    Quantity = SelectedItem.Quantity,
                        //    Cost = SelectedItem.Cost,
                        //    Core = SelectedItem.Core,
                        //    PONumber = SelectedItem.PONumber,
                        //    InvoiceNumber = SelectedItem.InvoiceNumber,
                        //    TransactionDate = SelectedItem.TransactionDate
                        //};
                        //ItemToModify = SelectedItem;
                        ItemToModify = new VendorInvoiceLineItemToWrite();
                        CopyItem(SelectedItem, ItemToModify);
                    }
                }
                else
                {
                    if (ItemToModify != null)
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
            if (SelectedItem != null
            && await Dialogs.ConfirmAsync($"Are you sure you want to remove {SelectedItem.PartNumber}?", "Remove Item"))
            {
                Items.Remove(SelectedItem);
                SelectedItem = Items.FirstOrDefault();
                SelectedItems = new List<VendorInvoiceLineItemToWrite> { SelectedItem };
                Grid.Rebind();
            }
        }

        private void OnSaveEdit()
        {
            if (ItemFormMode != FormMode.Add && ItemFormMode != FormMode.Edit)
                return;

            if (ItemFormMode == FormMode.Add)
            {
                Items.Add(ItemToModify);
                selectedItemIndex = Items.IndexOf(ItemToModify);
                SelectedItem = Items[selectedItemIndex];
                SelectedItems = new List<VendorInvoiceLineItemToWrite> { SelectedItem };
                Grid.Rebind();
            }
            else if (ItemFormMode == FormMode.Edit)
            {
                //Items[selectedItemIndex] = ItemToModify;
                //SelectedItem = ItemToModify;
                CopyItem(ItemToModify, Items[selectedItemIndex]);
            }
            SelectedId = SelectedItem.Id;
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
            SelectedId = SelectedItem.Id;
            selectedItemIndex = Items.IndexOf(SelectedItem);
            SelectedItems = new List<VendorInvoiceLineItemToWrite> { SelectedItem };
        }

        protected void OnSelect(IEnumerable<VendorInvoiceLineItemToWrite> items)
        {
            //SelectedItem = items.FirstOrDefault();
            //SelectedItems = new List<VendorInvoiceItemToWrite> { SelectedItem };
        }

        protected override void OnInitialized()
        {
            if (Items.Count > 0)
            {
                if (itemToSelect == 0)
                {
                    SelectedItem = Items.FirstOrDefault();
                }
                else
                {
                    SelectedItem = Items.Where(x => x.Id == itemToSelect).FirstOrDefault();
                }
                selectedItemIndex = Items.IndexOf(SelectedItem);
                SelectedId = SelectedItem.Id;
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
            dst.Id = src.Id;
            dst.VendorInvoiceId = src.VendorInvoiceId;
            dst.Type = src.Type;
            dst.PartNumber = src.PartNumber;
            dst.Manufacturer = src.Manufacturer;
            dst.ManufacturerId = src.ManufacturerId;
            dst.Description = src.Description;
            dst.SaleCode = src.SaleCode;
            dst.SaleCodeId = src.SaleCodeId;
            dst.Quantity = src.Quantity;
            dst.Cost = src.Cost;
            dst.Core = src.Core;
            dst.PONumber = src.PONumber;
            dst.InvoiceNumber = src.InvoiceNumber;
            dst.TransactionDate = src.TransactionDate;
        }
    }
}
