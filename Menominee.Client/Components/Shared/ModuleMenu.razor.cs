using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Navigations;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Menominee.Client.Components.Shared
{
    public partial class ModuleMenu
    {
        [Parameter]
        public string ModuleName { get; set; }

        [Parameter]
        public List<MenuItem> MenuItems { get; set; }

        [Parameter]
        public EventCallback<string> OnItemSelected { get; set; }

        [Parameter]
        public string ModuleIconCss { get; set; } = "m-empty-icon";

        private async Task OnMenuItemSelected(MenuEventArgs<MenuItem> args)
        {
            await OnItemSelected.InvokeAsync(args.Item.Id);
        }

    }
}
