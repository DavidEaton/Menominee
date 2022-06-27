using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using Menominee.Client.Shared;

namespace Menominee.Client.Components.Scheduler
{
    public partial class SchedulerMenu
    {
        [Inject]
        public NavigationManager navigationManager { get; set; }

        private static string ModuleUrl = "/schedule";

        public void OnItemSelected(ModuleMenuItem selectedItem)
        {
        }

        private List<ModuleMenuItem> menuItems = new List<ModuleMenuItem>
        {
            new ModuleMenuItem
            {
                Text = "Placeholder",
                Id = (int)SchedulerMenuId.Placeholder,
                SubItems = new List<ModuleMenuItem>
                {
                    new ModuleMenuItem { Text="xxx", Url=$"{ModuleUrl}", Id=(int)SchedulerMenuId.Placeholder },
                    new ModuleMenuItem { Text="xxx", Url=$"{ModuleUrl}", Id=(int)SchedulerMenuId.Placeholder }
                },
                Url = ""
            }
         };
    }
}
