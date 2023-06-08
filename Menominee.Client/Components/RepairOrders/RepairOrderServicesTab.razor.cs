using CustomerVehicleManagement.Shared.Models.Inventory.InventoryItems;
using CustomerVehicleManagement.Shared.Models.Manufacturers;
using CustomerVehicleManagement.Shared.Models.ProductCodes;
using CustomerVehicleManagement.Shared.Models.RepairOrders;
using CustomerVehicleManagement.Shared.Models.RepairOrders.Items;
using CustomerVehicleManagement.Shared.Models.RepairOrders.LineItems.Item;
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

        public IEnumerable<RepairOrderLineItemToWrite> SelectedItems { get; set; } = Enumerable.Empty<RepairOrderLineItemToWrite>();
        public RepairOrderLineItemToWrite SelectedItem { get; set; }
        public RepairOrderLineItemToWrite ItemToModify { get; set; } = null;

        private FormMode ItemFormMode = FormMode.Unknown;
        private bool EditItemDialogVisible { get; set; } = false;
        private bool EditLaborDialogVisible { get; set; } = false;
        private bool SelectInventoryItemDialogVisible { get; set; } = false;
        public InventoryItemToReadInList SelectedInventoryItem { get; set; }
        private bool EditTechDialogVisible { get; set; } = false;
        //private bool CanEditItem { get; set; } = false;
        //private bool CanDeleteItem { get; set; } = false;
        private bool CanAddPart { get; set; } = true;
        //private bool CanAddLabor { get; set; } = true;
        public RepairOrderServiceToWrite ServiceToEdit { get; set; }

        private static void CopyItem(RepairOrderLineItemToWrite src, RepairOrderLineItemToWrite dst)
        {
            dst.Id = src.Id;
            dst.Item.Manufacturer = src.Item.Manufacturer;
            dst.Item.PartNumber = src.Item.PartNumber;
            dst.Item.Description = src.Item.Description;
            dst.Item.SaleCode = src.Item.SaleCode;
            dst.Item.ProductCode = src.Item.ProductCode;
            dst.Item.PartType = src.Item.PartType;
            dst.SaleType = src.SaleType;
            dst.IsDeclined = src.IsDeclined;
            dst.IsCounterSale = src.IsCounterSale;
            dst.QuantitySold = src.QuantitySold;
            dst.SellingPrice = src.SellingPrice;
            dst.LaborAmount = src.LaborAmount;
            dst.DiscountAmount = src.DiscountAmount;
            dst.Cost = src.Cost;
            dst.Core = src.Core;
            dst.Total = src.Total;
            dst.SerialNumbers = src.SerialNumbers;
            dst.Warranties = src.Warranties;
            dst.Taxes = src.Taxes;
            dst.Purchases = src.Purchases;
        }

        private void SelectInventoryItem()
        {
            SelectInventoryItemDialogVisible = true;
        }

        private void AddItem()
        {
            SelectInventoryItemDialogVisible = false;
            if (SelectedInventoryItem is not null)
            {
                ItemToModify = new();
                ItemToModify.Item.Manufacturer = SelectedInventoryItem.Manufacturer;
                ItemToModify.Item.SaleCode = SelectedInventoryItem.ProductCode.SaleCode;
                ItemToModify.Item.ProductCode = SelectedInventoryItem.ProductCode;
                ItemToModify.Item.PartNumber = SelectedInventoryItem.ItemNumber;
                ItemToModify.Item.Description = SelectedInventoryItem.Description;
                //ItemToModify.SellingPrice = SelectedInventoryItem.SuggestedPrice;
                if (SelectedInventoryItem.Labor.LaborAmount.Amount > 0)
                {
                    ItemToModify.LaborAmount.PayType = ItemLaborType.Flat;
                    ItemToModify.LaborAmount.Amount = SelectedInventoryItem.Labor.LaborAmount.Amount;
                }
                //ItemToModify.Cost = SelectedInventoryItem.Cost;
                //ItemToModify.Core = SelectedInventoryItem.Core;

                ItemFormMode = FormMode.Add;
                EditItemDialogVisible = true;
            }
        }

        private void AddCustomItem()
        {
            ItemToModify = new()
            {
                Item = new()
                {
                    Manufacturer = new ManufacturerToRead(),
                    SaleCode = new SaleCodeToRead(),
                    ProductCode = new ProductCodeToRead(),
                    PartType = PartType.Part
                }
            };

            ItemFormMode = FormMode.Add;
            EditItemDialogVisible = true;
        }


        private void AddCustomLabor()
        {
            ItemToModify = new()
            {
                Item = new()
                {
                    Manufacturer = new ManufacturerToRead(),
                    SaleCode = new SaleCodeToRead(),
                    ProductCode = new ProductCodeToRead(),
                    PartNumber = "INSTALLATION",
                    PartType = PartType.Labor
                }
            };

            ItemFormMode = FormMode.Add;
            EditLaborDialogVisible = true;
        }

        private void EditItem(RepairOrderLineItemToWrite item)
        {
            SelectedItem = item;
            ItemToModify = new()
            {
                Id = item.Id,
                Item = new()
                {
                    Manufacturer = item.Item.Manufacturer,
                    SaleCode = item.Item.SaleCode,
                    ProductCode = item.Item.ProductCode,
                    Description = item.Item.Description,
                    PartNumber = item.Item.PartNumber,
                    PartType = item.Item.PartType
                },
                SaleType = item.SaleType,
                Core = item.Core,
                Cost = item.Cost,
                DiscountAmount = item.DiscountAmount,
                IsCounterSale = item.IsCounterSale,
                IsDeclined = item.IsDeclined,
                LaborAmount = item.LaborAmount,
                QuantitySold = item.QuantitySold,
                SellingPrice = item.SellingPrice,
                Total = item.Total,
                Purchases = item.Purchases,
                SerialNumbers = item.SerialNumbers,
                Taxes = item.Taxes,
                Warranties = item.Warranties
            };
            //CopyItem(SelectedItem, ItemToModify);
            ItemFormMode = FormMode.Edit;

            if (ItemToModify.Item.PartType == PartType.Labor)
                EditLaborDialogVisible = true;
            else
                EditItemDialogVisible = true;
        }

        private void EditTechs(RepairOrderServiceToWrite service)
        {
            ServiceToEdit = service;
            EditTechDialogVisible = true;
        }

        private void OnItemRowDoubleClickHandler(GridRowClickEventArgs args)
        {
            EditItem(args.Item as RepairOrderLineItemToWrite);
        }

        private void OnEditItemClick(GridCommandEventArgs args)
        {
            EditItem(args.Item as RepairOrderLineItemToWrite);
        }

        private async Task OnDeleteItemClick(GridCommandEventArgs args)
        {
            SelectedItem = args.Item as RepairOrderLineItemToWrite;
            if (await ShowItemDeleteConfirm(SelectedItem.Item.PartNumber))
            {
                var service = FindServiceByCode(SelectedItem.Item.SaleCode.Code);
                if (service != null)
                {
                    service.LineItems.Remove(SelectedItem);
                    if (service.LineItems.Count == 0)
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
            if (ItemFormMode is not FormMode.Add and not FormMode.Edit)
                return;

            ItemToModify.Recalculate();

            (var saleCode, var saleCodeName) = await GetSaleCode(ItemToModify.Item.SaleCode.Id);
            var service = FindServiceByCode(saleCode); //FindService(ItemToModify.SaleCode.Code);

            if (service is null)
                service = AddService(saleCode, saleCodeName);   //AddService(ItemToModify.SaleCode.Code);

            if (ItemFormMode is FormMode.Add)
                service.LineItems.Add(ItemToModify);
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
                    techs += tech.Employee.Id;
                }
            }

            return techs;
        }

        private async Task<(string saleCode, string name)> GetSaleCode(long id)
        {
            SaleCodeToRead saleCodeToRead = await SaleCodeDataService.GetSaleCodeAsync(id);
            if (saleCodeToRead != null)
                return (saleCodeToRead.Code, saleCodeToRead.Name);

            return ("", "");
        }

        private RepairOrderServiceToWrite FindServiceByCode(string saleCode)
        {
            return RepairOrder.Services?.Where(service => service.SaleCode.Name == saleCode).FirstOrDefault();
        }

        private RepairOrderServiceToWrite AddService(string saleCode, string name)
        {
            RepairOrderServiceToWrite service = new()
            {
                ServiceName = name,
                SaleCode = new(),
            };

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
