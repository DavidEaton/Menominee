using Menominee.Client.Shared;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;

namespace Menominee.Client.Components.PartsOrders
{
    public partial class PartsOrdersMenu
    {
        [Inject]
        public NavigationManager navigationManager { get; set; }

        private static string ModuleUrl = "/partsorders";

        public void OnItemSelected(ModuleMenuItem selectedItem)
        {
        }

        private List<ModuleMenuItem> menuItems = new List<ModuleMenuItem>
        {
            new ModuleMenuItem
            {
                Text = "Placeholder",
                Id = (int)PartsOrdersMenuId.Placeholder,
                SubItems = new List<ModuleMenuItem>
                {
                    new ModuleMenuItem { Text="xxx", Url=$"{ModuleUrl}", Id=(int)PartsOrdersMenuId.Placeholder },
                    new ModuleMenuItem { Text="xxx", Url=$"{ModuleUrl}", Id=(int)PartsOrdersMenuId.Placeholder }
                },
                Url = ""
            }
         };
    }
}