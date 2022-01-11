using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Navigations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MenomineePlayWASM.Client.Components.Scheduler
{
    public partial class SchedulerMenu
    {
        [Inject]
        private NavigationManager navigationManager { get; set; }

        public void OnItemSelected(string selectedItem)
        {
            if (selectedItem.Length > 0)
            {
                string url = "/schedule";

                switch (selectedItem)
                {
                    //case "customerList":
                    //    navigationManager.NavigateTo($"{url}/listing");
                    //    break;
                    case "placeholder":
                        navigationManager.NavigateTo($"{url}/version2");
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
                Text = "Appointments",
                Items = new List<MenuItem>
                {
                    new MenuItem { Text= "New", HtmlAttributes=SubItemHtmlAttribute, Id="placeholder" },
                    new MenuItem { Text= "Find", HtmlAttributes=SubItemHtmlAttribute, Id="placeholder" }
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
                Text = "Reminders",
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
