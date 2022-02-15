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
                string url = "/reports";

                switch (selectedItem)
                {
                    case "placeholder":
                        navigationManager.NavigateTo($"{url}/placeholder");
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
                Text = "Placeholder",
                Items = new List<MenuItem>
                {
                    new MenuItem { Text= "Item 1", HtmlAttributes=SubItemHtmlAttribute, Id="xxx" },
                    new MenuItem { Text= "Item 2", HtmlAttributes=SubItemHtmlAttribute, Id="xxx" }
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
