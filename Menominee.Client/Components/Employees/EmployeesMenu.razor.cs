using Menominee.Client.Shared;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Navigations;
using System.Collections.Generic;

namespace Menominee.Client.Components.Employees
{
    public partial class EmployeesMenu
    {
        [Inject]
        public NavigationManager navigationManager { get; set; }

        private static string ModuleUrl = "/employees";
        public int menuWidth { get; set; } = 139;

        public void OnItemSelected(MenuItem selectedItem)
        {
        }

        private List<MenuItem> menuItems = new List<MenuItem>
        {
#pragma warning disable BL0005
            new MenuItem
            {
                Text = "Placeholder",
                Id = "-1",
                Items = new List<MenuItem>
                {
                    new MenuItem { Text="xxx", Url=$"{ModuleUrl}", Id=((int)EmployeesMenuId.Placeholder).ToString() },
                    new MenuItem { Text="xxx", Url=$"{ModuleUrl}", Id=((int)EmployeesMenuId.Placeholder).ToString() }
                },
                Url = ""
            }
        };
#pragma warning restore BL0005         
    }
}
