using CustomerVehicleManagement.Shared.Models.Inventory;
using CustomerVehicleManagement.Shared.Models.ProductCodes;
using Menominee.Client.Services.Manufacturers;
using Menominee.Client.Services.ProductCodes;
using Menominee.Common.Enums;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telerik.Blazor.Components;

namespace Menominee.Client.Components.Inventory
{
    public partial class InventoryLaborEditor
    {
        [Inject]
        public IManufacturerDataService manufacturerDataService { get; set; }

        [Inject]
        public IProductCodeDataService productCodeDataService { get; set; }

        [Parameter]
        public InventoryItemToWrite Item { get; set; }

        [Parameter]
        public string Title { get; set; } = String.Empty;

        [Parameter]
        public EventCallback OnValidSubmit { get; set; }

        [Parameter]
        public EventCallback OnDiscard { get; set; }

        //TelerikDropDownList<SkillLevelListItem, SkillLevel> skillLevelDropDownRef;
        //TelerikDropDownList<LaborTypeListItem, ItemLaborType> laborTypeDropDownRef;
        //TelerikDropDownList<LaborTypeListItem, ItemLaborType> techPayTypeDropDownRef;

        //private SkillLevel selectedSkillLevel { get; set; } = SkillLevel.A;
        //private ItemLaborType selectedLaborType { get; set; } = ItemLaborType.None;
        //private ItemLaborType selectedTechPayType { get; set; } = ItemLaborType.None;

        private IReadOnlyList<ProductCodeToReadInList> ProductCodes = null;
        private string SaleCode = string.Empty;
        private long MiscMfrId = 0;
        private bool parametersSet = false;

        protected override async Task OnInitializedAsync()
        {
            MiscMfrId = (await manufacturerDataService.GetAllManufacturersAsync()).Where(mfr => mfr.Code == "1").FirstOrDefault().Id;
            ProductCodes = (await productCodeDataService.GetAllProductCodesAsync(MiscMfrId)).ToList();
            //ProductCodes = (await productCodeDataService.GetAllProductCodesAsync()).Where(pc => pc?.Manufacturer?.Code == "1").ToList()
            //                ?? new List<ProductCodeToReadInList>();

            if (Item.Labor != null)
            {
                //selectedSkillLevel = Item.Labor.SkillLevel;
                //selectedLaborType = Item.Labor.LaborType;
                //selectedTechPayType = Item.Labor.TechPayType;
            }

            foreach (ItemLaborType item in Enum.GetValues(typeof(ItemLaborType)))
            {
                laborTypeList.Add(new LaborTypeListItem { Text = EnumExtensions.GetDisplayName(item), Value = item });
            }

            foreach (SkillLevel item in Enum.GetValues(typeof(SkillLevel)))
            {
                skillLevelList.Add(new SkillLevelListItem { Text = EnumExtensions.GetDisplayName(item), Value = item });
            }

            base.OnInitialized();
        }

        protected override void OnParametersSet()
        {
            if (parametersSet)
                return;

            parametersSet = true;
            OnProductCodeChange();
            if (Item.Labor == null)
            {
                Item.ManufacturerId = MiscMfrId;
                Item.Labor = new();
                Item.ItemType = InventoryItemType.Labor;

                Title = "Add Labor";
            }
            else
            {
                Title = "Edit Labor";
            }

            //laborTypeDropDownRef?.Rebind();
            //techPayTypeDropDownRef?.Rebind();
            //skillLevelDropDownRef?.Rebind();

            //OnLaborTypeChange();
            //OnTechPayTypeChange();

            StateHasChanged();
        }

        private void OnProductCodeChange()
        {
            if (Item != null && ProductCodes != null)
            {
                var saleCode = ProductCodes.FirstOrDefault(pc => pc.Id == Item.ProductCodeId)?.SaleCode;
                if (saleCode != null)
                    SaleCode = saleCode.Code + " - " + saleCode.Name;
                else
                    SaleCode = string.Empty;
            }
        }

        //private void OnLaborTypeChange()
        //{
        //    Item.Labor.LaborType = selectedLaborType;
        //}

        //private void OnTechPayTypeChange()
        //{
        //    Item.Labor.TechPayType = selectedTechPayType;
        //}

        //private void OnSkillLevelChange()
        //{
        //    Item.Labor.SkillLevel = selectedSkillLevel;
        //}

        private List<LaborTypeListItem> laborTypeList { get; set; } = new List<LaborTypeListItem>();
        private List<SkillLevelListItem> skillLevelList { get; set; } = new List<SkillLevelListItem>();

        public class LaborTypeListItem
        {
            public string Text { get; set; }
            public ItemLaborType Value { get; set; }
        }

        public class SkillLevelListItem
        {
            public string Text { get; set; }
            public SkillLevel Value { get; set; }
        }
    }
}
