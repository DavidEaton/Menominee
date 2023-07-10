using Menominee.Shared.Models.Contactable;
using Menominee.Client.Shared;
using Menominee.Common.Enums;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Menominee.Client.Components.Phones
{
    public partial class PhoneEditor
    {
        [Inject]
        IJSRuntime JsInterop { get; set; }

        [Parameter]
        public PhoneToWrite Phone { get; set; }

        [Parameter]
        public FormMode FormMode { get; set; } = FormMode.Unknown;

        [Parameter]
        public EventCallback OnSave { get; set; }

        [Parameter]
        public EventCallback OnCancel { get; set; }

        public string Title { get; set; }

        List<PhoneTypeEnumModel> PhoneTypeEnumData { get; set; } = new List<PhoneTypeEnumModel>();

        protected override void OnInitialized()
        {
            foreach (PhoneType item in Enum.GetValues(typeof(PhoneType)))
            {
                if (item != PhoneType.Unknown)
                    PhoneTypeEnumData.Add(new PhoneTypeEnumModel { DisplayText = item.ToString(), Value = item });
            }
        }

        protected override void OnParametersSet()
        {
            Title = FormTitle.BuildTitle(FormMode, "Phone");
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);
            if (firstRender)
            {
                // TODO: this focuses the element but leaves the cursor at the end
                //await JsInterop.InvokeVoidAsync("jsfunction.focusElement", "phoneNumber");
            }
        }

        internal class PhoneTypeEnumModel
        {
            public PhoneType Value { get; set; }
            public string DisplayText { get; set; }
        }
    }
}
