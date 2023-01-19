using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Navigations;
using System.Collections.Generic;

namespace Menominee.Client.Components.Dispatch
{
    public partial class DispatchMenu
    {
        [Inject]
        public NavigationManager NavigationManager { get; set; }

        private static string ModuleUrl = "/dispatch";
        public int MenuWidth { get; set; } = 139;

        public void OnItemSelected(MenuItem selectedItem)
        {
            //if (Int32.Parse(selectedItem.Id) >= 0 && selectedItem.Url.Length > 0)
            //{
            //    navigationManager.NavigateTo(selectedItem.Url);
            //}
        }

        private List<MenuItem> menuItems = new()
        {
#pragma warning disable BL0005
            new MenuItem
            {
                Text = "Placeholder",
                Id = "-1",
                Items = new List<MenuItem>
                {
                    new MenuItem { Text="xxx", Url=$"{ModuleUrl}", Id=((int)DispatchMenuId.Placeholder).ToString() },
                    new MenuItem { Text="xxx", Url=$"{ModuleUrl}", Id=((int)DispatchMenuId.Placeholder).ToString() }
                },
                Url = ""
            }
         };
#pragma warning restore BL0005
    }
}
