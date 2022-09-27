using Menominee.Client.Shared;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Navigations;
using System.Collections.Generic;

namespace Menominee.Client.Components.Settings
{
    public partial class SettingsMenu
    {
        [Inject]
        public NavigationManager navigationManager { get; set; }

        private static string ModuleUrl = "/settings";

        public void OnItemSelected(MenuItem selectedItem)
        {
        }

        public int menuWidth { get; set; } = 336;

        private List<MenuItem> menuItems = new List<MenuItem>
        {
#pragma warning disable BL0005
            new MenuItem
            {
                Text = "General",
                Id = "-1",//((int)SettingsMenuId.General).ToString(),
                Items = new List<MenuItem>
                {
                    new MenuItem { Text="Company Information", Url=$"{ModuleUrl}/companyinfo", Id=((int)SettingsMenuId.CompanyInformation).ToString() },
                    new MenuItem { Text="Custom Settings", Url=$"{ModuleUrl}/customsettings", Id=((int)SettingsMenuId.CustomSettings).ToString() }
                },
                Url = ""
            },
            new MenuItem
            {
                Text = "Shop",
                Id = "-1",//((int)SettingsMenuId.Shop).ToString(),
                Items = new List<MenuItem>
                {
                    new MenuItem { Text="Shop Information", Url=$"{ModuleUrl}/shopinfo", Id=((int)SettingsMenuId.ShopInformation).ToString() },
                    new MenuItem { Text="Credit Cards", Url=$"{ModuleUrl}/creditcards", Id=((int)SettingsMenuId.CreditCards).ToString() },
                    new MenuItem { Text="Sale Codes", Url=$"{ModuleUrl}/salecodes", Id=((int)SettingsMenuId.SaleCodes).ToString() },
                    new MenuItem { Text="Shop Supplies", Url=$"{ModuleUrl}/shopsupplies", Id=((int)SettingsMenuId.ShopSupplies).ToString() },
                    new MenuItem { Separator=true },
                    new MenuItem { Text="Ordering & Catalogs", Url=$"{ModuleUrl}/orderingcatalogs", Id=((int)SettingsMenuId.OrderingAndCatalogs).ToString() },
                },
                Url = ""
            },
            new MenuItem
            {
                Text = "Taxes",
                Id = "-1",//((int)SettingsMenuId.Taxes).ToString(),
                Items = new List<MenuItem>
                {
                    new MenuItem { Text="Sales Taxes", Url=$"{ModuleUrl}/salestaxes", Id=((int)SettingsMenuId.SalesTaxes).ToString() },
                    new MenuItem { Text="Excise / Disposal / HazMat Fees", Url=$"{ModuleUrl}/excisefees", Id=((int)SettingsMenuId.ExciseDisposalHazMat).ToString() },
                    new MenuItem { Text="Customer Tax Profiles", Url=$"{ModuleUrl}", Id=((int)SettingsMenuId.CustomerTaxProfiles).ToString() },
                    new MenuItem { Text="Part Tax Profiles", Url=$"{ModuleUrl}", Id=((int)SettingsMenuId.PartTaxProfiles).ToString() }
                },
                Url = ""
            },
            new MenuItem
            {
                Text = "User",
                Id = "-1",//((int)SettingsMenuId.User).ToString(),
                Items = new List<MenuItem>
                {
                    new MenuItem { Text="User Information", Url=$"{ModuleUrl}/userinfo", Id=((int)SettingsMenuId.UserInformation).ToString() }
                },
                Url = ""
            }
         };
#pragma warning restore BL0005
    }
}