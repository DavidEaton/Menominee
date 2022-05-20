using CustomerVehicleManagement.Shared.Models.SaleCodes;
using Menominee.Client.Services.SaleCodes;
using Menominee.Common.Enums;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telerik.Blazor.Components;

namespace Menominee.Client.Components.Settings.Pages
{
    public partial class ShopSuppliesPage : ComponentBase
    {
        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Inject]
        public ISaleCodeDataService SaleCodeDataService { get; set; }

        private List<SaleCodeShopSuppliesToReadInList> SaleCodes;
        private TelerikGrid<SaleCodeShopSuppliesToReadInList> Grid { get; set; }
        private long Id { get; set; } = 0;
        private ShopSuppliesSettings SuppliesSettings { get; set; } = new();
        private SaleCodeShopSuppliesToReadInList SelectedSaleCode { get; set; } = null;

        protected override async Task OnInitializedAsync()
        {
            SaleCodes = (await SaleCodeDataService.GetAllSaleCodeShopSuppliesAsync()).ToList();

            if (SaleCodes?.Count > 0)
            {
                Id = SaleCodes.FirstOrDefault().Id;
            }

            foreach (ShopSuppliesCostType type in Enum.GetValues(typeof(ShopSuppliesCostType)))
            {
                costTypeList.Add(new CostTypeListItem { Text = EnumExtensions.GetDisplayName(type), Value = type });
            }

            SuppliesSettings.DisplayName = "Shop Supplies";
        }

        private void OnRowSelected(GridRowClickEventArgs args)
        {
            Id = (args.Item as SaleCodeShopSuppliesToReadInList).Id;
        }

        private void OnDone()
        {
            NavigationManager.NavigateTo("/settings/");
        }

        private async Task UpdateHandler(GridCommandEventArgs args)
        {
            SaleCodeShopSuppliesToReadInList itemInList = (SaleCodeShopSuppliesToReadInList)args.Item;

            await UpdateSaleCode(itemInList);
        }

        private async Task UpdateSaleCode(SaleCodeShopSuppliesToReadInList itemInList)
        {
            SaleCodeToRead itemToRead = await SaleCodeDataService.GetSaleCodeAsync(itemInList.Id);
            if (itemToRead == null)
                return;

            itemToRead.ShopSupplies.Percentage = itemInList.Percentage;
            itemToRead.ShopSupplies.MinimumJobAmount = itemInList.MinimumJobAmount;
            itemToRead.ShopSupplies.MinimumCharge = itemInList.MinimumCharge;
            itemToRead.ShopSupplies.MaximumCharge = itemInList.MaximumCharge;
            itemToRead.ShopSupplies.IncludeParts = itemInList.IncludeParts;
            itemToRead.ShopSupplies.IncludeLabor = itemInList.IncludeLabor;

            var itemToWrite = SaleCodeHelper.CreateSaleCode(itemToRead);

            await SaleCodeDataService.UpdateSaleCodeAsync(itemToWrite, itemToRead.Id);

            SaleCodes = (await SaleCodeDataService.GetAllSaleCodeShopSuppliesAsync()).ToList();
        }

        private async Task IncludePartsChanged(object value)
        {
            if (SelectedSaleCode.IncludeParts != (bool)value)
            {
                SelectedSaleCode.IncludeParts = !(bool)value;
                await UpdateSaleCode(SelectedSaleCode);
            }
        }

        public enum ShopSuppliesCostType { None, Percentage, Flat }
        public class ShopSuppliesSettings
        {
            public string DisplayName { get; set; }
            public string SaleCodeName { get; set; }
            public long SaleCodeId { get; set; }
            public double MaximumCharge { get; set; }
            public ShopSuppliesCostType CostType { get; set; }
            public double Cost { get; set; }
        }

        private List<CostTypeListItem> costTypeList { get; set; } = new List<CostTypeListItem>();

        public class CostTypeListItem
        {
            public string Text { get; set; }
            public ShopSuppliesCostType Value { get; set; }
        }
    }
}
