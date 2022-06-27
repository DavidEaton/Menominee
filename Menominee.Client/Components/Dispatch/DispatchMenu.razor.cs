using Menominee.Client.Shared;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;

namespace Menominee.Client.Components.Dispatch
{
    public partial class DispatchMenu
    {
        [Inject]
        public NavigationManager navigationManager { get; set; }

        private static string ModuleUrl = "/dispatch";

        public void OnItemSelected(ModuleMenuItem selectedItem)
        {
        }

        private List<ModuleMenuItem> menuItems = new List<ModuleMenuItem>
        {
            new ModuleMenuItem
            {
                Text = "Placeholder",
                Id = (int)DispatchMenuId.Placeholder,
                SubItems = new List<ModuleMenuItem>
                {
                    new ModuleMenuItem { Text="xxx", Url=$"{ModuleUrl}", Id=(int)DispatchMenuId.Placeholder },
                    new ModuleMenuItem { Text="xxx", Url=$"{ModuleUrl}", Id=(int)DispatchMenuId.Placeholder }
                },
                Url = ""
            }
         };
    }
}
