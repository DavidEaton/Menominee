using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Navigations;
using System.Collections.Generic;

namespace Menominee.Client.Components.Customers
{
    public partial class CustomersMenu
    {
        [Inject]
        public NavigationManager NavigationManager { get; set; }

        private static readonly string ModuleUrl = "/customers";
        public int MenuWidth { get; set; } = 270;

        public void OnItemSelected(MenuItem selectedItem)
        {
            //if (int.Parse(selectedItem.Id) >= 0 && selectedItem.Url.Length > 0)
            //{
            //    NavigationManager.NavigateTo(selectedItem.Url);
            //}

            
        }

        private List<MenuItem> menuItems = new()
        {
#pragma warning disable BL0005
            new MenuItem
            {
                Text = "Customers",
                Id = "-1",
                Items = new List<MenuItem>
                {
                    new MenuItem { Text="Customer List", Url=$"{ModuleUrl}/listing", Id=((int)CustomersMenuId.CustomerList).ToString() },
                    new MenuItem { Text="Advanced Search", Url=$"{ModuleUrl}", Id=((int)CustomersMenuId.AdvancedSearch).ToString() }
                },
                Url = ""
            },
            new MenuItem
            {
                Text = "Reports",
                Id = ((int)CustomersMenuId.Reports).ToString(),
                Url = ""
            },
            new MenuItem
            {
                Text = "Utilities",
                Id = ((int)CustomersMenuId.Utilities).ToString(),
                Url = ""
            }
         };
#pragma warning restore BL0005
    }
}
