using CustomerVehicleManagement.Shared.Models.Inventory;
using CustomerVehicleManagement.Shared.Models.Manufacturers;
using CustomerVehicleManagement.Shared.Models.ProductCodes;
using CustomerVehicleManagement.Shared.Models.RepairOrders;
using CustomerVehicleManagement.Shared.Models.RepairOrders.Items;
using CustomerVehicleManagement.Shared.Models.RepairOrders.Services;
using CustomerVehicleManagement.Shared.Models.SaleCodes;
using Menominee.Client.Services.SaleCodes;
using Menominee.Common.Enums;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telerik.Blazor;
using Telerik.Blazor.Components;

namespace Menominee.Client.Components.RepairOrders
{
    public partial class RepairOrderServicesTab : ComponentBase
    {
        [Inject]
        public ISaleCodeDataService SaleCodeDataService { get; set; }

        //[Inject]
        //public IInventoryItemDataService inventoryItemDataService { get; set; }

        [Parameter]
        public IList<RepairOrderServiceToWrite> Services { get; set; }

        [CascadingParameter]
        public DialogFactory Dialogs { get; set; }

        [CascadingParameter]
        public RepairOrderToWrite RepairOrder { get; set; }

        public TelerikGrid<RepairOrderServiceToWrite> ServicesGrid { get; set; }

        public IEnumerable<RepairOrderItemToWrite> SelectedItems { get; set; } = Enumerable.Empty<RepairOrderItemToWrite>();
        public RepairOrderItemToWrite SelectedItem { get; set; }
        public RepairOrderItemToWrite ItemToModify { get; set; } = null;

        private FormMode ItemFormMode = FormMode.Unknown;
        private bool EditItemDialogVisible { get; set; } = false;
        private bool EditLaborDialogVisible { get; set; } = false;
        private bool SelectInventoryItemDialogVisible { get; set; } = false;
        private long SelectedInventoryItemId { get; set; } = 0;
        public InventoryItemToReadInList SelectedInventoryItem { get; set; }
        private bool EditTechDialogVisible { get; set; } = false;
        //private bool CanEditItem { get; set; } = false;
        //private bool CanDeleteItem { get; set; } = false;
        private bool CanAddPart { get; set; } = true;
        //private bool CanAddLabor { get; set; } = true;
        public RepairOrderServiceToWrite ServiceToEdit { get; set; }

        protected override void OnInitialized()
        {
            base.OnInitialized();
        }

        protected override void OnParametersSet()
        {
            base.OnParametersSet();
        }

        private static void CopyItem(RepairOrderItemToWrite src, RepairOrderItemToWrite dst)
        {
            dst.Id = src.Id;
            dst.RepairOrderServiceId = src.RepairOrderServiceId;
            dst.SequenceNumber = src.SequenceNumber;
            dst.Manufacturer = src.Manufacturer;
            dst.ManufacturerId = src.ManufacturerId;
            dst.PartNumber = src.PartNumber;
            dst.Description = src.Description;
            dst.SaleCode = src.SaleCode;
            dst.SaleCodeId = src.SaleCodeId;
            dst.ProductCode = src.ProductCode;
            dst.ProductCodeId = src.ProductCodeId;
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

        private void SelectInventoryItem()
        {
            SelectInventoryItemDialogVisible = true;
        }

        private void AddItem()
        {
            SelectInventoryItemDialogVisible = false;
            if (SelectedInventoryItem != null)
            {
                ItemToModify = new();
                ItemToModify.Manufacturer = new ManufacturerToWrite();// new CustomerVehicleManagement.Domain.Entities.Manufacturer();
                ItemToModify.ManufacturerId = SelectedInventoryItem.ManufacturerId;
                ItemToModify.SaleCode = new SaleCodeToWrite();// new CustomerVehicleManagement.Domain.Entities.SaleCode();
                //ItemToModify.SaleCodeId = (long)(SelectedInventoryItem.ProductCode?.SaleCode?.Id);
                ItemToModify.ProductCode = new ProductCodeToWrite();// new CustomerVehicleManagement.Domain.Entities.ProductCode();
                ItemToModify.ProductCodeId = SelectedInventoryItem.ProductCodeId;
                ItemToModify.PartNumber = SelectedInventoryItem.PartNumber;
                ItemToModify.Description = SelectedInventoryItem.Description;
                ItemToModify.SellingPrice = SelectedInventoryItem.SuggestedPrice;
                if (SelectedInventoryItem.Labor > 0)
                {
                    ItemToModify.LaborType = ItemLaborType.Flat;
                    ItemToModify.LaborEach = SelectedInventoryItem.Labor;
                }
                ItemToModify.Cost = SelectedInventoryItem.Cost;
                //ItemToModify.Core = SelectedInventoryItem.Core;

                ItemFormMode = FormMode.Add;
                EditItemDialogVisible = true;
            }
        }

        private void AddCustomItem()
        {
            ItemToModify = new();
            ItemToModify.Manufacturer = new ManufacturerToWrite();// new CustomerVehicleManagement.Domain.Entities.Manufacturer();
            ItemToModify.ManufacturerId = 1;
            ItemToModify.SaleCode = new SaleCodeToWrite();// new CustomerVehicleManagement.Domain.Entities.SaleCode();
            ItemToModify.ProductCode = new ProductCodeToWrite();// new CustomerVehicleManagement.Domain.Entities.ProductCode();
            ItemToModify.PartType = PartType.Part;
            ItemFormMode = FormMode.Add;
            EditItemDialogVisible = true;
        }

        private void AddCustomLabor()
        {
            ItemToModify = new();
            ItemToModify.Manufacturer = new ManufacturerToWrite();// new CustomerVehicleManagement.Domain.Entities.Manufacturer();
            ItemToModify.ManufacturerId = 1;
            ItemToModify.SaleCode = new SaleCodeToWrite();// new CustomerVehicleManagement.Domain.Entities.SaleCode();
            ItemToModify.ProductCode = new ProductCodeToWrite();// new CustomerVehicleManagement.Domain.Entities.ProductCode();
            ItemToModify.PartNumber = "INSTALLATION";
            ItemToModify.PartType = PartType.Labor;
            ItemFormMode = FormMode.Add;
            EditLaborDialogVisible = true;
        }

        private void EditItem(RepairOrderItemToWrite item)
        {
            SelectedItem = item;
            ItemToModify = new();
            ItemToModify.Manufacturer = new ManufacturerToWrite();
            ItemToModify.SaleCode = new SaleCodeToWrite();
            ItemToModify.ProductCode = new ProductCodeToWrite();
            CopyItem(SelectedItem, ItemToModify);
            ItemFormMode = FormMode.Edit;
            if (ItemToModify.PartType == PartType.Labor)
                EditLaborDialogVisible = true;
            else
                EditItemDialogVisible = true;
        }

        void EditTechs(RepairOrderServiceToWrite service)
        {
            ServiceToEdit = service;
            EditTechDialogVisible = true;
        }

        void OnItemRowDoubleClickHandler(GridRowClickEventArgs args)
        {
            var moops = args.Item as RepairOrderItemToWrite;

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
                RepairOrderServiceToWrite service = FindServiceByCode(SelectedItem.SaleCode.Code);
                if (service != null)
                {
                    service.Items.Remove(SelectedItem);
                    if (service.Items.Count == 0)
                    {
                        RepairOrder.Services.Remove(service);
                    }
                    else
                    {
                        service.Recalculate();
                    }
                    ServicesGrid?.Rebind();
                }

                RepairOrder?.Recalculate();
            }
        }

        private async Task OnSaveItemEdit()
        {
            if (ItemFormMode != FormMode.Add && ItemFormMode != FormMode.Edit)
                return;

            ItemToModify.Recalculate();

            (string scCode, string scName) = await GetSaleCode(ItemToModify.SaleCodeId);
            RepairOrderServiceToWrite service = FindServiceByCode(scCode); //FindService(ItemToModify.SaleCode.Code);
            if (service == null)
                service = AddService(scCode, scName);   //AddService(ItemToModify.SaleCode.Code);

            if (ItemFormMode == FormMode.Add)
                service.Items.Add(ItemToModify);
            else
                CopyItem(ItemToModify, SelectedItem);
            service.Recalculate();
            RepairOrder?.Recalculate();

            EditItemDialogVisible = false;
            EditLaborDialogVisible = false;
            if (ItemFormMode == FormMode.Add)
                ServicesGrid?.Rebind();
            StateHasChanged();
        }

        private void OnCancelItemEdit()
        {
            ItemFormMode = FormMode.Unknown;
            EditItemDialogVisible = false;
            EditLaborDialogVisible = false;
        }

        private void OnCancelInventoryItemSelect()
        {
            SelectInventoryItemDialogVisible = false;
        }

        private void OnDoneTechEdit()
        {
            EditTechDialogVisible = false;
            StateHasChanged();
        }

        public async Task<bool> ShowItemDeleteConfirm(string partNumber)
        {
            return await Dialogs.ConfirmAsync($"Are you sure you want to delete {partNumber}?", "Delete Item");
        }

        private string TechDisplayList(RepairOrderServiceToWrite service)
        {
            string techs = string.Empty;

            if (service.Techs?.Count > 0)
            {
                foreach (var tech in service.Techs)
                {
                    if (techs.Length > 0)
                        techs += ", ";
                    techs += tech.TechnicianId;
                }
            }

            return techs;
        }

        private async Task<(string sc, string name)> GetSaleCode(long id)
        {
            SaleCodeToRead saleCodeToRead = await SaleCodeDataService.GetSaleCode(id);
            if (saleCodeToRead != null)
                return (saleCodeToRead.Code, saleCodeToRead.Name);

            return ("", "");
        }

        private RepairOrderServiceToWrite FindServiceByCode(string saleCode)
        {
            return RepairOrder.Services?.Where(x => x.SaleCode == saleCode).FirstOrDefault();
        }

        private RepairOrderServiceToWrite AddService(string saleCode, string name)
        {
            RepairOrderServiceToWrite service = new();
            service.SaleCode = saleCode;
            service.ServiceName = name;
            RepairOrder.Services.Add(service);

            return service;
        }

        //private InventoryItemToRead GetInventoryItem(long id)
        //{
        //    //return RepairOrder.Services?. .Items?.Where(x => x.Id == id).FirstOrDefault();
        //    InventoryItemToRead itemToRead = await inventoryItemDataService.GetSaleCode(id);
        //    if (saleCodeToRead != null)
        //        return (saleCodeToRead.Code, saleCodeToRead.Name);

        //    return ("", "");
        //}

    }
}
