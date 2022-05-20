using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Navigations;
using System.Collections.Generic;

namespace Menominee.Client.Components.RepairOrders
{
    public partial class RepairOrdersMenu
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
                    Text = "Repair Orders",
                    Items = new List<MenuItem>
                    {
                        new MenuItem { Text= "Today's Repair Orders", HtmlAttributes=SubItemHtmlAttribute, Id="roToday" },
                        new MenuItem { Text= "Waiting On Parts", HtmlAttributes=SubItemHtmlAttribute, Id="roWaiting" },
                        new MenuItem { Text= "Ready For Pickup", HtmlAttributes=SubItemHtmlAttribute, Id="roReady" },
                        new MenuItem { Text= "All Repair Orders", HtmlAttributes=SubItemHtmlAttribute, Id="roAll" }
                    },
                    HtmlAttributes=ItemHtmlAttribute
                },
                new MenuItem
                {
                    Text = "Invoices",
                    Items = new List<MenuItem>
                    {
                        new MenuItem { Text= "Unpaid Invoices", HtmlAttributes=SubItemHtmlAttribute, Id="invoiceUnpaid" },
                        new MenuItem { Text= "Today's Invoices", HtmlAttributes=SubItemHtmlAttribute, Id="invoiceToday" },
                        new MenuItem { Text= "All Invoices", HtmlAttributes=SubItemHtmlAttribute, Id="invoiceAll" }
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
                string url = "/repairorders";

                switch (selectedItem)
                {
                    case "roToday":
                    case "roWaiting":
                    case "roReady":
                    case "roAll":
                        navigationManager.NavigateTo($"{url}/worklog");
                        break;
                    case "invoiceUnpaid":
                    case "invoiceToday":
                    case "invoiceAll":
                        navigationManager.NavigateTo($"{url}/worklog");
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
