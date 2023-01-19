using CustomerVehicleManagement.Shared.Models.Contactable;
using Menominee.Client.Shared;
using Menominee.Common.Enums;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Threading.Tasks;

namespace Menominee.Client.Components.Emails
{
    public partial class EmailEditor
    {
        [Inject]
        IJSRuntime JsInterop { get; set; }

        [Parameter]
        public EmailToWrite Email { get; set; }

        [Parameter]
        public FormMode FormMode { get; set; } = FormMode.Unknown;

        [Parameter]
        public EventCallback OnSave { get; set; }

        [Parameter]
        public EventCallback OnCancel { get; set; }

        public string Title { get; set; }

        protected override void OnParametersSet()
        {
            Title = FormTitle.BuildTitle(FormMode, "Email");
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);
            if (firstRender)
            {
                await JsInterop.InvokeVoidAsync("jsfunction.focusElement", "emailaddress");
            }
        }
    }
}
