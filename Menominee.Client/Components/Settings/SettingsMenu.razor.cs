using Menominee.Client.Shared;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;

namespace Menominee.Client.Components.Settings
{
    public partial class SettingsMenu
    {
        [Inject]
        public NavigationManager navigationManager { get; set; }

        private static string ModuleUrl = "/settings";

        public void OnItemSelected(ModuleMenuItem selectedItem)
        {
        }

        private List<ModuleMenuItem> menuItems = new List<ModuleMenuItem>
        {
            new ModuleMenuItem
            {
                Text = "General",
                Id = (int)SettingsMenuId.General,
                SubItems = new List<ModuleMenuItem>
                {
                    new ModuleMenuItem { Text="Company Information", Url=$"{ModuleUrl}/companyinfo", Id=(int)SettingsMenuId.CompanyInformation },
                    new ModuleMenuItem { Text="Custom Settings", Url=$"{ModuleUrl}/customsettings", Id=(int)SettingsMenuId.CustomSettings }
                },
                Url = ""
            },
            new ModuleMenuItem
            {
                Text = "Shop",
                Id = (int)SettingsMenuId.Shop,
                SubItems = new List<ModuleMenuItem>
                {
                    new ModuleMenuItem { Text="Shop Information", Url=$"{ModuleUrl}/shopinfo", Id=(int)SettingsMenuId.ShopInformation },
                    new ModuleMenuItem { Text="Credit Cards", Url=$"{ModuleUrl}/creditcards", Id=(int)SettingsMenuId.CreditCards },
                    new ModuleMenuItem { Text="Shop Supplies", Url=$"{ModuleUrl}/shopsupplies", Id=(int)SettingsMenuId.ShopSupplies },
                    new ModuleMenuItem { Separator=true },
                    new ModuleMenuItem { Text="Ordering & Catalogs", Url=$"{ModuleUrl}/orderingcatalogs", Id=(int)SettingsMenuId.OrderingAndCatalogs },
                },
                Url = ""
            },
            new ModuleMenuItem
            {
                Text = "Taxes",
                Id = (int)SettingsMenuId.Taxes,
                SubItems = new List<ModuleMenuItem>
                {
                    new ModuleMenuItem { Text="Sales Taxes", Url=$"{ModuleUrl}/salestaxes", Id=(int)SettingsMenuId.SalesTaxes },
                    new ModuleMenuItem { Text="Excise / Disposal / HazMat Fees", Url=$"{ModuleUrl}/excisefees", Id=(int)SettingsMenuId.ExciseDisposalHazMat },
                    new ModuleMenuItem { Text="Customer Tax Profiles", Url=$"{ModuleUrl}", Id=(int)SettingsMenuId.CustomerTaxProfiles },
                    new ModuleMenuItem { Text="Part Tax Profiles", Url=$"{ModuleUrl}", Id=(int)SettingsMenuId.PartTaxProfiles }
                },
                Url = ""
            },
            new ModuleMenuItem
            {
                Text = "User",
                Id = (int)SettingsMenuId.User,
                SubItems = new List<ModuleMenuItem>
                {
                    new ModuleMenuItem { Text="User Information", Url=$"{ModuleUrl}/userinfo", Id=(int)SettingsMenuId.UserInformation }
                },
                Url = ""
            }
         };
    }
}