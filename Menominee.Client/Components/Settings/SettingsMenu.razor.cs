using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Navigations;

namespace Menominee.Client.Components.Settings
{
    public partial class SettingsMenu
    {
        [Inject]
        public NavigationManager? NavigationManager { get; set; }

        private static readonly string ModuleUrl = "/settings";

        public void OnItemSelected(MenuItem selectedItem)
        {
        }

        public int MenuWidth { get; set; } = 336;

        private readonly List<MenuItem> menuItems = new()
        {
#pragma warning disable BL0005
            new MenuItem
            {
                Text = "Inventory",
                Id = "-1",
                Items = new List<MenuItem>
                {
                    new MenuItem { Text="Manufacturers", Url=$"{ModuleUrl}", Id=((int)SettingsMenuId.Manufacturers).ToString() },
                    new MenuItem { Text="Product Codes", Url=$"{ModuleUrl}", Id=((int)SettingsMenuId.ProductCodes).ToString() },
                    new MenuItem { Text="Selling Price Names", Url=$"{ModuleUrl}", Id=((int)SettingsMenuId.SellingPriceNames).ToString() }
                },
                Url = ""
            },
            new MenuItem
            {
                Text = "Integrations",
                Id = "-1",
                Items = new List<MenuItem>
                {
                    new MenuItem { Text="Ordering & Catalogs", Url=$"{ModuleUrl}/orderingcatalogs", Id=((int)SettingsMenuId.OrderingAndCatalogs).ToString() },
                },
                Url = ""
            },
            new MenuItem
            {
                Text = "Sales",
                Id = "-1",
                Items = new List<MenuItem>
                {
                    new MenuItem { Text="Sale Codes", Url=$"{ModuleUrl}/salecodes", Id=((int)SettingsMenuId.SaleCodes).ToString() },
                    new MenuItem { Text="Shop Supplies", Url=$"{ModuleUrl}/shopsupplies", Id=((int)SettingsMenuId.ShopSupplies).ToString() },
                    new MenuItem { Text="Credit Cards", Url=$"{ModuleUrl}/creditcards", Id=((int)SettingsMenuId.CreditCards).ToString() },
                    new MenuItem { Text="Discounts", Url=$"{ModuleUrl}", Id=((int)SettingsMenuId.Discounts).ToString() },
                    new MenuItem { Text="Declined Reasons", Url=$"{ModuleUrl}", Id=((int)SettingsMenuId.DeclinedReasons).ToString() },
                    new MenuItem { Text="Fiscal Periods", Url=$"{ModuleUrl}", Id=((int)SettingsMenuId.FiscalPeriods).ToString() }
                },
                Url = ""
            },
            new MenuItem
            {
                Text = "Taxes",
                Id = "-1",
                Items = new List<MenuItem>
                {
                    new MenuItem { Text="Sales Taxes", Url=$"{ModuleUrl}/salestaxes", Id=((int)SettingsMenuId.SalesTaxes).ToString() },
                    new MenuItem { Text="Excise / Disposal / HazMat Fees", Url=$"{ModuleUrl}/excisefees", Id=((int)SettingsMenuId.ExciseDisposalHazMat).ToString() },
                    new MenuItem { Text="Customer Tax Profiles", Url=$"{ModuleUrl}", Id=((int)SettingsMenuId.CustomerTaxProfiles).ToString() },
                    new MenuItem { Text="Item Tax Profiles", Url=$"{ModuleUrl}", Id=((int)SettingsMenuId.ItemTaxProfiles).ToString() }
                },
                Url = ""
            },
            new MenuItem
            {
                Text = "Customers",
                Id = "-1",
                Items = new List<MenuItem>
                {
                    new MenuItem { Text="Pricing Profiles", Url=$"{ModuleUrl}", Id=((int)SettingsMenuId.PricingProfiles).ToString() },
                    new MenuItem { Text="Rewards", Url=$"{ModuleUrl}", Id=((int)SettingsMenuId.Rewards).ToString() }
                },
                Url = ""
            },
            new MenuItem
            {
                Text = "Accounts Receivable",
                Id = "-1",
                Items = new List<MenuItem>
                {
                    new MenuItem { Text="Terms & Service Charges", Url=$"{ModuleUrl}", Id=((int)SettingsMenuId.Terms).ToString() },
                    new MenuItem { Text="Statements", Url=$"{ModuleUrl}", Id=((int)SettingsMenuId.Statements).ToString() }
                },
                Url = ""
            },
            new MenuItem
            {
                Text = "Accounts Payable",
                Id = "-1",
                Items = new List<MenuItem>
                {
                    new MenuItem { Text="Payment Methods", Url=$"{ModuleUrl}/vendorpaymentmethods", Id=((int)SettingsMenuId.PaymentMethods).ToString() }
                },
                Url = ""
            }
         };
#pragma warning restore BL0005
    }
}