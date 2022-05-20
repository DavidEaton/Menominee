using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Navigations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Menominee.Client.Components.RepairOrders
{
    public partial class RepairOrderEditMenu
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
                    Text = "Catalogs",
                    Items = new List<MenuItem>
                    {
                        new MenuItem { Text= "MV Connect", HtmlAttributes=SubItemHtmlAttribute, Id="mvConnect" },
                        new MenuItem { Text= "Nexpart", HtmlAttributes=SubItemHtmlAttribute, Id="nexpart" },
                        new MenuItem { Text= "NAPA Punchout", HtmlAttributes=SubItemHtmlAttribute, Id="napa" },
                        new MenuItem { Text= "IAP", HtmlAttributes=SubItemHtmlAttribute, Id="iap" }
                    },
                    HtmlAttributes=ItemHtmlAttribute
                },
                new MenuItem
                {
                    Text = "Actions",
                    Items = new List<MenuItem>
                    {
                        new MenuItem { Text= "Authorize", HtmlAttributes=SubItemHtmlAttribute, Id="authorize" },
                        new MenuItem { Text= "Take Deposit", HtmlAttributes=SubItemHtmlAttribute, Id="takeDeposit" },
                        new MenuItem { Text= "Print Work Order", HtmlAttributes=SubItemHtmlAttribute, Id="printWO" },
                        new MenuItem { Text= "EMail Repair Order", HtmlAttributes=SubItemHtmlAttribute, Id="emailRO" },
                        new MenuItem { Text= "Text Messaging", HtmlAttributes=SubItemHtmlAttribute, Id="SMS" }
                    },
                    HtmlAttributes=ItemHtmlAttribute
                },
                new MenuItem
                {
                    Text = "Inspection Log",
                    Id = "inspectionLog",
                    HtmlAttributes=ItemHtmlAttribute
                },
                new MenuItem
                {
                    Text = "Caller ID",
                    Id = "callerId",
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
                    //case "roToday":
                    //case "roWaiting":
                    //case "roReady":
                    //case "roAll":
                    //    navigationManager.NavigateTo($"{url}/worklog");
                    //    break;
                    //case "invoiceUnpaid":
                    //case "invoiceToday":
                    //case "invoiceAll":
                    //    navigationManager.NavigateTo($"{url}/worklog");
                    //    break;
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
