using Blazored.Toast.Services;
using Menominee.Client.Services.SaleCodes;
using Menominee.Client.Services.Settings;
using Menominee.Domain.Entities.Settings;
using Menominee.Domain.Enums;
using Menominee.Shared.Models.SaleCodes;
using Menominee.Shared.Models.Settings;
using Microsoft.AspNetCore.Components;
using Telerik.Blazor.Components;

namespace Menominee.Client.Components.Settings.Pages
{
    public partial class ShopSuppliesPage : ComponentBase
    {
        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Inject]
        public IToastService ToastService { get; set; }

        [Inject]
        public ISaleCodeDataService SaleCodeDataService { get; set; }

        [Inject]
        public ISettingDataService SettingDataService { get; set; }

        private List<SaleCodeShopSuppliesToReadInList> SaleCodes;
        private TelerikGrid<SaleCodeShopSuppliesToReadInList> Grid { get; set; }
        private long Id { get; set; } = 0;
        private ShopSuppliesSettings SuppliesSettings { get; set; } = new();
        private IReadOnlyList<SettingToRead>? Settings { get; set; }
        private SaleCodeShopSuppliesToReadInList SelectedSaleCode { get; set; } = null;

        protected override async Task OnInitializedAsync()
        {
            await LoadSettings();

            SaleCodes = (await SaleCodeDataService.GetAllShopSuppliesAsync()).ToList();

            if (SaleCodes?.Count > 0)
            {
                Id = SaleCodes.FirstOrDefault().Id;
            }

            foreach (ShopSuppliesCostType type in Enum.GetValues(typeof(ShopSuppliesCostType)))
            {
                CostTypeList.Add(new CostTypeListItem { Text = EnumExtensions.GetDisplayName(type), Value = type });
            }
        }

        private void OnRowSelected(GridRowClickEventArgs args)
        {
            Id = (args.Item as SaleCodeShopSuppliesToReadInList).Id;
            SelectedSaleCode = args.Item as SaleCodeShopSuppliesToReadInList;
        }

        private async Task OnDone()
        {
            NavigationManager.NavigateTo("/settings/");
        }

        private async Task IncludePartsChanged(object value)
        {
            await UpdateSaleCode(SelectedSaleCode);
            //if (SelectedSaleCode.IncludeParts != (bool)value)
            //{
            //    SelectedSaleCode.IncludeParts = !(bool)value;
            //    await UpdateSaleCode(SelectedSaleCode);
            //}
        }

        private async Task FieldValueChanged(object value)
        {
            //TODO: This gets fired even when the value hasn't changed.  What to do?
            await UpdateSaleCode(SelectedSaleCode);
        }

        private async Task LoadSettings()
        {
            var result = await SettingDataService.GetByGroupAsync(SettingGroup.ShopSupplies);

            if (result.IsSuccess)
                Settings = result.Value;

            if (result.IsFailure)
                ToastService.ShowError($" Failed to load with error: {result.Error}");

            if (Settings is null) return;

            var displayName = Settings
                .FirstOrDefault(setting => setting.SettingName.Equals(SettingName.DisplayName))
                ?.SettingValue;

            var saleCodeId = Settings
                .FirstOrDefault(setting => setting.SettingName.Equals(SettingName.ReportInSaleCode))
                ?.SettingValue;

            var maximumCharge = Settings
                .FirstOrDefault(setting => setting.SettingName.Equals(SettingName.MaximumCharge))
                ?.SettingValue;

            var costType = Settings
                .FirstOrDefault(setting => setting.SettingName.Equals(SettingName.CostType))
                ?.SettingValue;

            var costPerInvoice = Settings
                .FirstOrDefault(setting => setting.SettingName.Equals(SettingName.CostPerInvoice))
                ?.SettingValue;

            SuppliesSettings = new ShopSuppliesSettings
            {
                DisplayName = displayName ?? string.Empty,
                SaleCodeName = "Shop Supplies",
                SaleCodeId = saleCodeId is not null
                    ? long.Parse(saleCodeId)
                    : 0L,
                MaximumCharge = maximumCharge is not null
                    ? (double)decimal.Parse(maximumCharge)
                    : 0D,
                CostType = costType is not null
                    ? (ShopSuppliesCostType)Enum.Parse(typeof(ShopSuppliesCostType), costType)
                    : ShopSuppliesCostType.None,
                Cost = costPerInvoice is not null
                    ? (double)decimal.Parse(costPerInvoice)
                    : 0D
            };
        }

        private async Task SaveSettings()
        {
            if (Settings is null) return;

            var updatedSettings = Settings.Select(setting =>
            {
                return new SettingToWrite
                {
                    Id = setting.Id,
                    SettingName = setting.SettingName,
                    SettingGroup = setting.SettingGroup,
                    SettingValue = setting.SettingName switch
                    {
                        SettingName.DisplayName => SuppliesSettings.DisplayName,
                        SettingName.ReportInSaleCode => SuppliesSettings.SaleCodeId.ToString(),
                        SettingName.MaximumCharge => SuppliesSettings.MaximumCharge.ToString(),
                        SettingName.CostType => ((int)SuppliesSettings.CostType).ToString(),
                        SettingName.CostPerInvoice => SuppliesSettings.Cost.ToString(),
                        _ => setting.SettingValue
                    }
                };
            }).ToList();

            await SettingDataService.AddMultipleAsync(updatedSettings);
        }

        private async Task UpdateHandler(GridCommandEventArgs args)
        {
            SaleCodeShopSuppliesToReadInList itemInList = (SaleCodeShopSuppliesToReadInList)args.Item;

            await UpdateSaleCode(itemInList);
        }

        private async Task UpdateSaleCode(SaleCodeShopSuppliesToReadInList itemInList)
        {
            SaleCodeToRead itemToRead = await SaleCodeDataService.GetAsync(itemInList.Id);
            if (itemToRead == null)
                return;

            itemToRead.ShopSupplies.Percentage = itemInList.Percentage;
            itemToRead.ShopSupplies.MinimumJobAmount = itemInList.MinimumJobAmount;
            itemToRead.ShopSupplies.MinimumCharge = itemInList.MinimumCharge;
            itemToRead.ShopSupplies.MaximumCharge = itemInList.MaximumCharge;
            itemToRead.ShopSupplies.IncludeParts = itemInList.IncludeParts;
            itemToRead.ShopSupplies.IncludeLabor = itemInList.IncludeLabor;

            var itemToWrite = SaleCodeHelper.ConvertReadToWriteDto(itemToRead);

            var response = await SaleCodeDataService.UpdateAsync(itemToWrite);

            if (response.IsFailure)
            {
                ToastService.ShowError(response.Error);
            }

            SaleCodes = (await SaleCodeDataService.GetAllShopSuppliesAsync()).ToList();
        }

        public class ShopSuppliesSettings
        {
            private ShopSuppliesCostType costType;

            public string DisplayName { get; set; }
            public string SaleCodeName { get; set; }
            public long SaleCodeId { get; set; }
            public double MaximumCharge { get; set; }
            public ShopSuppliesCostType CostType
            {
                get => costType;
                set
                {
                    costType = value;
                    if (costType.Equals(ShopSuppliesCostType.None))
                    {
                        Cost = 0D;
                    }
                }
            }
            public double Cost { get; set; }
        }

        private List<CostTypeListItem> CostTypeList { get; set; } = new List<CostTypeListItem>();

        public class CostTypeListItem
        {
            public string Text { get; set; }
            public ShopSuppliesCostType Value { get; set; }
        }
    }
}
