using Menominee.Client.Shared;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;

namespace Menominee.Client.Components.Customers
{
    public partial class CustomersMenu
    {
        [Inject]
        public NavigationManager navigationManager { get; set; }

        private static string ModuleUrl = "/customers";

        public void OnItemSelected(ModuleMenuItem selectedItem)
        {
        }

        private List<ModuleMenuItem> menuItems = new List<ModuleMenuItem>
        {
            new ModuleMenuItem
            {
                Text = "Placeholder",
                Id = (int)CustomersMenuId.Customers,
                SubItems = new List<ModuleMenuItem>
                {
                    new ModuleMenuItem { Text="Customer List", Url=$"{ModuleUrl}/listing", Id=(int)CustomersMenuId.CustomerList },
                    new ModuleMenuItem { Text="Advanced Search", Url=$"{ModuleUrl}", Id=(int)CustomersMenuId.AdvancedSearch }
                },
                Url = ""
            },
            new ModuleMenuItem
            {
                Text = "Reports",
                Id = (int)CustomersMenuId.Reports,
                Url = ""
            },
            new ModuleMenuItem
            {
                Text = "Utilities",
                Id = (int)CustomersMenuId.Utilities,
                Url = ""
            }
         };
    }
}
