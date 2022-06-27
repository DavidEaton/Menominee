using Menominee.Client.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Menominee.Client.Components.Shared
{
    public partial class ModuleMenu
    {
        [Inject]
        public IJSRuntime _js { get; set; }

        [Parameter]
        public string ModuleName { get; set; }

        [Parameter]
        public List<ModuleMenuItem> MenuItems { get; set; }

        [Parameter]
        public EventCallback<ModuleMenuItem> OnItemSelected { get; set; }

        [Parameter]
        public string IconName { get; set; } = "m-empty-icon";

        protected async Task OnClickHandler(ModuleMenuItem item)
        {
            await _js.InvokeVoidAsync("closeMenu");
            await OnItemSelected.InvokeAsync(item);
        }
    }
}
