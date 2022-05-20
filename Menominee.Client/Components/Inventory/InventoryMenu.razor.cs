using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Navigations;
using System.Collections.Generic;

namespace Menominee.Client.Components.Inventory
{
    public partial class InventoryMenu
    {
        [Inject]
        private NavigationManager navigationManager { get; set; }

        protected override void OnInitialized()
        {
#pragma warning disable BL0005            
            menuItems = new List<MenuItem>()
            {
                new MenuItem
                {
                    Text = "Items",
                    Id = "inventoryList",
                    HtmlAttributes=ItemHtmlAttribute
                },
                new MenuItem
                {
                    Text = "Orders",
                    Id = "orders",
                    HtmlAttributes=ItemHtmlAttribute
                },
                new MenuItem
                {
                    Text = "Reports",
                    Items = new List<MenuItem>
                    {
                        new MenuItem { Text= "Total Value of Stock", HtmlAttributes=SubItemHtmlAttribute, Id="report" },
                        new MenuItem { Text= "Something Else", HtmlAttributes=SubItemHtmlAttribute, Id="report" }
                    },
                    HtmlAttributes=ItemHtmlAttribute
                }
            };
#pragma warning restore BL0005            
        }

        public void OnItemSelected(string selectedItem)
        {
            if (selectedItem.Length > 0)
            {
                string url = "/inventory";

                switch (selectedItem)
                {
                    case "inventoryList":
                        navigationManager.NavigateTo($"{url}/items/listing");
                        break;
                    case "orders":
                        navigationManager.NavigateTo($"{url}/orders");
                        break;
                    default:
                        navigationManager.NavigateTo(url);
                        break;
                }
            }
        }

        private List<MenuItem> menuItems = null;

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
