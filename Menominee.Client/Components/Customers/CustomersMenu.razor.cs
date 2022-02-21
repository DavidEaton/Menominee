using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Navigations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Menominee.Client.Components.Customers
{
    public partial class CustomersMenu
    {
        [Inject]
        private NavigationManager navigationManager { get; set; }

        public void OnItemSelected(string selectedItem)
        {
            if (selectedItem.Length > 0)
            {
                string url = "/customers";

                switch (selectedItem)
                {
                    case "customerList":
                        navigationManager.NavigateTo($"{url}/listing");
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
                Text = "Customers",
                Items = new List<MenuItem>
                {
                    new MenuItem { Text= "Customer List", HtmlAttributes=SubItemHtmlAttribute, Id="customerList" },
                    new MenuItem { Text= "Advanced Search", HtmlAttributes=SubItemHtmlAttribute, Id="advancedSearch" }
                },
                HtmlAttributes=ItemHtmlAttribute
            },
            new MenuItem
            {
                Text = "Reports",
                Items = new List<MenuItem>
                {
                    new MenuItem { Text= "Some Report", HtmlAttributes=SubItemHtmlAttribute, Id="report" },
                    new MenuItem { Text= "Another Report", HtmlAttributes=SubItemHtmlAttribute, Id="report" }
                },
                HtmlAttributes=ItemHtmlAttribute
            },
            new MenuItem
            {
                Text = "Utilities",
                Items = new List<MenuItem>
                {
                    new MenuItem { Text= "Some Utility", HtmlAttributes=SubItemHtmlAttribute, Id="utility" },
                    new MenuItem { Text= "Another Utility", HtmlAttributes=SubItemHtmlAttribute, Id="utility" }
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
