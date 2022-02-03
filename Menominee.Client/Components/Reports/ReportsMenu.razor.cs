using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using Syncfusion.Blazor.Navigations;

namespace Menominee.Client.Components.Reports
{
    public partial class ReportsMenu
    {
        [Inject]
        private NavigationManager navigationManager { get; set; }

        public void OnItemSelected(string selectedItem)
        {
            if (selectedItem.Length > 0)
            {
                string url = "/payables";

                switch (selectedItem)
                {
                    case "invoices":
                        navigationManager.NavigateTo($"{url}/invoices/listing");
                        break;
                    case "creditReturns":
                        navigationManager.NavigateTo($"{url}/returns/listing");
                        break;
                    case "vendors":
                        navigationManager.NavigateTo($"{url}/vendors/listing");
                        break;
                    default:
                        navigationManager.NavigateTo(url);
                        break;
                }
            }
        }

        private List<MenuItem> menuItems = new List<MenuItem>
        {
            new MenuItem
            {
                Text = "Invoices",
                Id = "invoices",
                HtmlAttributes=ItemHtmlAttribute
            },
            new MenuItem
            {
                Text = "Credit Returns",
                Id = "creditReturns",
                HtmlAttributes=ItemHtmlAttribute
            },
            new MenuItem
            {
                Text = "Statements",
                Id = "statements",
                HtmlAttributes=ItemHtmlAttribute
            },
            new MenuItem
            {
                Text = "Vendors",
                Id = "vendors",
                HtmlAttributes=ItemHtmlAttribute
            },
            new MenuItem
            {
                Text = "Reports",
                Items = new List<MenuItem>
                {
                    new MenuItem { Text= "Vendor List", HtmlAttributes=SubItemHtmlAttribute, Id="vendorListReport" },
                    new MenuItem { Separator= true, HtmlAttributes=SubItemHtmlAttribute },
                    new MenuItem { Text= "Vendor Detail", HtmlAttributes=SubItemHtmlAttribute, Id="vendorListReport" },
                    new MenuItem { Text= "Vendor Summary", HtmlAttributes=SubItemHtmlAttribute, Id="vendorListReport" },
                    new MenuItem { Text= "Invoice Summary", HtmlAttributes=SubItemHtmlAttribute, Id="vendorListReport" },
                },
                HtmlAttributes=ItemHtmlAttribute
            }
         };

        static Dictionary<string, object> SubItemHtmlAttribute = new Dictionary<string, object>()
        {
            {"class", "m-menu-sub-item" }
        };

        static Dictionary<string, object> ItemHtmlAttribute = new Dictionary<string, object>()
        {
            {"class", "m-menu-item" }
        };
    }
}
