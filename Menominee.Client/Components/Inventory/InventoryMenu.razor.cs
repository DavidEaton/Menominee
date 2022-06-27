using Menominee.Client.Shared;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;

namespace Menominee.Client.Components.Inventory
{
    public partial class InventoryMenu
    {
        [Inject]
        public NavigationManager navigationManager { get; set; }

        private static string ModuleUrl = "/inventory";

        public void OnItemSelected(ModuleMenuItem selectedItem)
        {
        }

        private List<ModuleMenuItem> menuItems = new List<ModuleMenuItem>
        {
            new ModuleMenuItem
            {
                Text = "Items",
                Id = (int)InventoryMenuId.Items,
                Url = $"{ModuleUrl}/items/listing"
            },
            new ModuleMenuItem
            {
                Text = "Orders",
                Id = (int)InventoryMenuId.Orders,
                Url = $"{ModuleUrl}/orders"
            },
            new ModuleMenuItem
            {
                Text = "Reports",
                Id = (int)InventoryMenuId.Reports,
                SubItems = new List<ModuleMenuItem>
                {
                    new ModuleMenuItem { Text="Total Value Of Inventory", Url=$"{ModuleUrl}", Id=(int)InventoryMenuId.TotalValue },
                    new ModuleMenuItem { Text="Top Sellers", Url=$"{ModuleUrl}", Id=(int)InventoryMenuId.TopSellers }
                },
                Url = ""
            }
         };
    }
}