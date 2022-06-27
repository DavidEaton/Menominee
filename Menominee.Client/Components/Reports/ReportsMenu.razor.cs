using Menominee.Client.Shared;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;

namespace Menominee.Client.Components.Reports
{
    public partial class ReportsMenu
    {
        [Inject]
        public NavigationManager navigationManager { get; set; }

        private static string ModuleUrl = "/reports";

        public void OnItemSelected(ModuleMenuItem selectedItem)
        {
        }

        private List<ModuleMenuItem> menuItems = new List<ModuleMenuItem>
        {
            new ModuleMenuItem
            {
                Text = "Placeholder",
                Id = (int)ReportsMenuId.Placeholder,
                SubItems = new List<ModuleMenuItem>
                {
                    new ModuleMenuItem { Text="xxx", Url=$"{ModuleUrl}", Id=(int)ReportsMenuId.Placeholder },
                    new ModuleMenuItem { Text="xxx", Url=$"{ModuleUrl}", Id=(int)ReportsMenuId.Placeholder }
                },
                Url = ""
            }
         };
    }
}