using Menominee.Shared.Models.SaleCodes;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Menominee.Client.Components.Settings
{
    public partial class SaleCodeEditor
    {
        [Parameter]
        public SaleCodeToWrite SaleCode { get; set; }

        [Parameter]
        public string Title { get; set; } = "Edit Sale Code";

        [Parameter]
        public EventCallback OnSave { get; set; }

        [Parameter]
        public EventCallback OnCancel { get; set; }

        [Inject]
        IJSRuntime JsInterop { get; set; }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);
            if (firstRender)
            {
                await Focus("code");
            }
        }

        public async Task Focus(string elementId)
        {
            await JsInterop.InvokeVoidAsync("jsfunction.focusElement", elementId);
        }
    }
}
