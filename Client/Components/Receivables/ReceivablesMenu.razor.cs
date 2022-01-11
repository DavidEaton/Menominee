using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Navigations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MenomineePlayWASM.Client.Components.Receivables
{
    public partial class ReceivablesMenu
    {
        [Inject]
        private NavigationManager navigationManager { get; set; }

        public void OnItemSelected(string selectedItem)
        {
            if (selectedItem.Length > 0)
            {
                string url = "/receivables";

                switch (selectedItem)
                {
                    case "customerList":
                        navigationManager.NavigateTo($"{url}/accounts/listing");
                        break;
                    case "creditHoldList":
                        navigationManager.NavigateTo($"{url}/accounts/creditHoldListing");
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
                Text = "Accounts",
                //IconCss = "em-icons e-file",
                Items = new List<MenuItem>
                {
                    new MenuItem { Text= "Edit", HtmlAttributes=SubItemHtmlAttribute, Id="customerList"/*, ChildContent=ChildContent*/},
                    new MenuItem { Text= "Credit Hold List", HtmlAttributes=SubItemHtmlAttribute, Id="creditHoldList"}
                },
                HtmlAttributes=ItemHtmlAttribute
            },
            new
            MenuItem {
                Text = "Transactions",
                //IconCss = "em-icons e-edit",
                Items = new List<MenuItem>
                {
                    new MenuItem { Text= "New Transaction", HtmlAttributes=SubItemHtmlAttribute, Id="" },
                    new MenuItem { Text= "Batch Files", HtmlAttributes=SubItemHtmlAttribute, Id="" },
                    new MenuItem { Text= "Apply Open Credits", HtmlAttributes=SubItemHtmlAttribute, Id="" },
                    new MenuItem { Text= "Clear Paid Transactions", HtmlAttributes=SubItemHtmlAttribute, Id="" },
                    new MenuItem { Text= "Unapply Transaction", HtmlAttributes=SubItemHtmlAttribute, Id="" }
                },
                HtmlAttributes=ItemHtmlAttribute
            },
            new MenuItem
            {
                Text = "Service Charges",
                Items = new List<MenuItem>
                {
                    new MenuItem { Text= "Create & Post", HtmlAttributes=SubItemHtmlAttribute, Id="" },
                    new MenuItem { Text= "Waive Charges", HtmlAttributes=SubItemHtmlAttribute, Id="" }
                },
                HtmlAttributes=ItemHtmlAttribute
            },
            new MenuItem
            {
                Text = "Reports",
                HtmlAttributes=ItemHtmlAttribute
            },
            new MenuItem
            {
                Text = "Setup",
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
