using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using Menominee.Client.Shared;
using Syncfusion.Blazor.Navigations;

namespace Menominee.Client.Components.Scheduler
{
    public partial class SchedulerMenu
    {
        [Inject]
        public NavigationManager navigationManager { get; set; }

        private static string ModuleUrl = "/schedule";
        public int menuWidth { get; set; } = 139;

        public void OnItemSelected(MenuItem selectedItem)
        {
        }

        private List<MenuItem> menuItems = new List<MenuItem>
        {
            new MenuItem
            {
                Text = "Placeholder",
                Id = "-1",//((int)SchedulerMenuId.Placeholder).ToString(),
                Items = new List<MenuItem>
                {
                    new MenuItem { Text="xxx", Url=$"{ModuleUrl}", Id=((int)SchedulerMenuId.Placeholder).ToString() },
                    new MenuItem { Text="xxx", Url=$"{ModuleUrl}", Id=((int)SchedulerMenuId.Placeholder).ToString() }
                },
                Url = ""
            }
         };
    }
}
