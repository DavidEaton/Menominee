using CustomerVehicleManagement.Shared.Models.RepairOrders.Items;
using CustomerVehicleManagement.Shared.Models.RepairOrders.Services;
using Menominee.Common.Enums;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Telerik.Blazor;
using Telerik.Blazor.Components;

namespace Menominee.Client.Components.RepairOrders
{
    public partial class RepairOrderServices
    {
        [Parameter]
        public IList<RepairOrderServiceToWrite> Services { get; set; }

        [CascadingParameter]
        public DialogFactory Dialogs { get; set; }

        public TelerikGrid<RepairOrderServiceToWrite> ServicesGrid { get; set; }

        public IEnumerable<RepairOrderItemToWrite> SelectedItems { get; set; } = Enumerable.Empty<RepairOrderItemToWrite>();
        public RepairOrderItemToWrite SelectedItem { get; set; }
        public RepairOrderItemToWrite ItemToModify { get; set; } = null;

        //public long SelectedItemId
        //{
        //    get => selectedItemId;
        //    set
        //    {
        //        selectedItemId = value;
        //        CanEditItem = selectedItemId != 0;
        //        CanDeleteItem = selectedItemId != 0 && Services[selectedServiceIndex].Items.Count > 0;
        //        //CanAddItem = true;
        //    }
        //}

        //private long itemToSelect { get; set; } = 0;
        //private long selectedItemId = 0;
        //private int selectedItemIndex = 0;
        //private int selectedServiceIndex = 0;
        //private long nextItemId = 0;
        //private long nextServiceId = 0;
        //private bool shouldRender = true;
        private FormMode ItemFormMode = FormMode.Unknown;
        private bool EditItemDialogVisible { get; set; } = false;
        private bool CanEditItem { get; set; } = false;
        private bool CanDeleteItem { get; set; } = false;
        private bool CanAddPart { get; set; } = true;
        private bool CanAddLabor { get; set; } = true;

        protected override void OnInitialized()
        {
        }

        private static void CopyItem(RepairOrderItemToWrite src, RepairOrderItemToWrite dst)
        {
            dst.Id = src.Id;
            dst.RepairOrderServiceId = src.RepairOrderServiceId;
            dst.SequenceNumber = src.SequenceNumber;
            dst.Manufacturer = src.Manufacturer;
            dst.PartNumber = src.PartNumber;
            dst.Description = src.Description;
            dst.SaleCode = src.SaleCode;
            dst.ProductCode = src.ProductCode;
            dst.SaleType = src.SaleType;
            dst.PartType = src.PartType;
            dst.IsDeclined = src.IsDeclined;
            dst.IsCounterSale = src.IsCounterSale;
            dst.QuantitySold = src.QuantitySold;
            dst.SellingPrice = src.SellingPrice;
            dst.LaborType = src.LaborType;
            dst.LaborEach = src.LaborEach;
            dst.DiscountType = src.DiscountType;
            dst.DiscountEach = src.DiscountEach;
            dst.Cost = src.Cost;
            dst.Core = src.Core;
            dst.Total = src.Total;
        }

        private void AddItem()
        {
            ItemToModify = new();
            ItemFormMode = FormMode.Add;
            EditItemDialogVisible = true;
        }

        private void EditItem(RepairOrderItemToWrite item)
        {
            SelectedItem = item;
            ItemToModify = new();
            CopyItem(SelectedItem, ItemToModify);
            ItemFormMode = FormMode.Edit;
            EditItemDialogVisible = true;
        }

        void OnItemRowDoubleClickHandler(GridRowClickEventArgs args)
        {
            EditItem(args.Item as RepairOrderItemToWrite);
        }

        private void OnEditItemClick(GridCommandEventArgs args)
        {
            EditItem(args.Item as RepairOrderItemToWrite);
        }

        private async Task OnDeleteItemClick(GridCommandEventArgs args)
        {
            SelectedItem = args.Item as RepairOrderItemToWrite;
            if (await ShowItemDeleteConfirm(SelectedItem.PartNumber))
            {
                //
            }
        }

        private void OnSaveItemEdit()
        {
            if (ItemFormMode != FormMode.Add && ItemFormMode != FormMode.Edit)
                return;

            RecalcItem(ItemToModify);
            //if (ItemFormMode == FormMode.Add)
            //{
            //    int serviceIndex = FindServiceIndex(ItemToModify.SaleCode);
            //    if (serviceIndex < 0)
            //    {
            //        serviceIndex = AddService(ItemToModify.SaleCode);
            //        ItemToModify.RepairOrderServiceId = nextServiceId;
            //    }

            //    Services[serviceIndex].Items.Add(ItemToModify);
            //}
            //else if (ItemFormMode == FormMode.Edit)
            //{
            //    CopyItem(ItemToModify, SelectedItem);
            //}
            //RecalcService(ItemToModify.RepairOrderServiceId);

            RepairOrderServiceToWrite service = null;
            if (ItemFormMode == FormMode.Add)
            {
                service = FindService(ItemToModify.SaleCode.Code);
                if (service == null)
                {
                    service = AddService(ItemToModify.SaleCode.Code);
                }

                service.Items.Add(ItemToModify);
            }
            else if (ItemFormMode == FormMode.Edit)
            {
                service = FindService(ItemToModify.SaleCode.Code);
                CopyItem(ItemToModify, SelectedItem);
            }
            RecalcService(service);

            EditItemDialogVisible = false;
            //shouldRender = true;
            if (ItemFormMode == FormMode.Add)
                ServicesGrid?.Rebind();
            StateHasChanged();
        }

        private void OnCancelItemEdit()
        {
            ItemFormMode = FormMode.Unknown;
            EditItemDialogVisible = false;
        }

        public async Task<bool> ShowItemDeleteConfirm(string partNumber)
        {
            return await Dialogs.ConfirmAsync($"Are you sure you want to delete {partNumber}?", "Delete Item");
        }

        private RepairOrderServiceToWrite FindService(string saleCode)
        {
            return Services?.Where(x => x.SaleCode == saleCode).FirstOrDefault();
        }

        private RepairOrderServiceToWrite AddService(string saleCode)
        {
            RepairOrderServiceToWrite service = new();
            service.SaleCode = saleCode;
            service.ServiceName = saleCode + " Service";    // replace when we have sale codes
            Services.Add(service);

            return service;
        }

        private void RecalcService(RepairOrderServiceToWrite service)
        {
            double partsTotal = 0.0;
            double laborTotal = 0.0;

            foreach (var item in service.Items)
            {
                partsTotal += item.SellingPrice * item.QuantitySold;
                laborTotal += item.LaborEach * item.QuantitySold;
            }

            service.PartsTotal = partsTotal;
            service.LaborTotal = laborTotal;
            service.Total = partsTotal + laborTotal;
        }

        //private int FindServiceIndex(string saleCode)
        //{
        //    for (var index = 0; index < Services.Count; index++)
        //    {
        //        if (Services[index].SaleCode == saleCode)
        //        {
        //            return index;
        //        }
        //    }

        //    return -1;
        //}

        //private int AddService(string saleCode)
        //{
        //    RepairOrderServiceToWrite service = new();
        //    service.Id = --nextServiceId;
        //    service.SaleCode = saleCode;
        //    service.ServiceName = saleCode + " Service";

        //    Services.Add(service);

        //    return Services.Count - 1;
        //}

        //private void RecalcService(long serviceId)
        //{
        //    RepairOrderServiceToWrite service = Services.Where(x => x.Id == serviceId).FirstOrDefault();
        //    double partsTotal = 0.0;
        //    double laborTotal = 0.0;

        //    foreach (var item in service.Items)
        //    {
        //        partsTotal += item.SellingPrice * item.QuantitySold;
        //        laborTotal += item.LaborEach * item.QuantitySold;
        //    }

        //    service.PartsTotal = partsTotal;
        //    service.LaborTotal = laborTotal;
        //    service.Total = partsTotal + laborTotal;
        //}

        private void RecalcItem(RepairOrderItemToWrite item)
        {
            item.Total = (item.SellingPrice + item.LaborEach - item.DiscountEach) * item.QuantitySold;
        }
    }
}
