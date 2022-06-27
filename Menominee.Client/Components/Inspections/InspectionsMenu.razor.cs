using Menominee.Client.Shared;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;

namespace Menominee.Client.Components.Inspections
{
    public partial class InspectionsMenu
    {
        [Inject]
        public NavigationManager navigationManager { get; set; }

        private static string ModuleUrl = "/inspections";

        public void OnItemSelected(ModuleMenuItem selectedItem)
        {
        }

        private List<ModuleMenuItem> menuItems = new List<ModuleMenuItem>
        {
            new ModuleMenuItem
            {
                Text = "Placeholder",
                Id = (int)InspectionsMenuId.Placeholder,
                SubItems = new List<ModuleMenuItem>
                {
                    new ModuleMenuItem { Text="xxx", Url=$"{ModuleUrl}", Id=(int)InspectionsMenuId.Placeholder },
                    new ModuleMenuItem { Text="xxx", Url=$"{ModuleUrl}", Id=(int)InspectionsMenuId.Placeholder }
                },
                Url = ""
            }
         };
    }
}
