using Menominee.Client.Shared;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Navigations;
using System.Collections.Generic;

namespace Menominee.Client.Components.Reports
{
    public partial class ReportsMenu
    {
        [Inject]
        public NavigationManager navigationManager { get; set; }

        private static string ModuleUrl = "/reports";
        public int menuWidth { get; set; } = 139;

        public void OnItemSelected(MenuItem selectedItem)
        {
        }

        private List<MenuItem> menuItems = new List<MenuItem>
        {
            new MenuItem
            {
                Text = "Placeholder",
                Id = "-1",//((int)ReportsMenuId.Placeholder).ToString(),
                Items = new List<MenuItem>
                {
                    new MenuItem { Text="xxx", Url=$"{ModuleUrl}", Id=((int)ReportsMenuId.Placeholder).ToString() },
                    new MenuItem { Text="xxx", Url=$"{ModuleUrl}", Id=((int)ReportsMenuId.Placeholder).ToString() }
                },
                Url = ""
            }
         };
    }
}