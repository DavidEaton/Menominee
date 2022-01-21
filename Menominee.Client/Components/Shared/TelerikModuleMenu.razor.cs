using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace Menominee.Client.Components.Shared
{
    public partial class TelerikModuleMenu
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
        public string ModuleIconCss { get; set; } = "m-empty-icon";

        protected async Task OnClickHandler(ModuleMenuItem item)
        {
            await _js.InvokeVoidAsync("closeMenu");
            await OnItemSelected.InvokeAsync(item);
        }
    }

    public class ModuleMenuItem
    {
        public string Text { get; set; }
        public string Url { get; set; }
        public List<ModuleMenuItem> SubItems { get; set; }
    }
}
