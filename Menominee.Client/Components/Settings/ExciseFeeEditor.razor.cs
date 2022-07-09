using CustomerVehicleManagement.Shared.Models.Taxes;
using Menominee.Common.Enums;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Menominee.Client.Components.Settings
{
    public partial class ExciseFeeEditor
    {
        [Parameter]
        public ExciseFeeToWrite ExciseFee { get; set; }

        [Parameter]
        public string Title { get; set; } = "Edit Fee";

        [Parameter]
        public EventCallback OnSave { get; set; }

        [Parameter]
        public EventCallback OnCancel { get; set; }

        [Inject]
        IJSRuntime JsInterop { get; set; }

        //private bool parametersSet = false;

        protected override async Task OnInitializedAsync()
        {
            foreach (ExciseFeeType feeType in Enum.GetValues(typeof(ExciseFeeType)))
            {
                feeTypeList.Add(new FeeTypeListItem { Text = EnumExtensions.GetDisplayName(feeType), Value = feeType });
            }

            await base.OnInitializedAsync();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);
            if (firstRender)
            {
                await Focus("name");
            }
        }

        //protected override async Task OnParametersSetAsync()
        //{
        //    if (parametersSet)
        //        return;

        //    parametersSet = true;

        //    StateHasChanged();
        //}

        public async Task Focus(string elementId)
        {
            await JsInterop.InvokeVoidAsync("jsfunction.focusElement", elementId);
        }

        private List<FeeTypeListItem> feeTypeList { get; set; } = new List<FeeTypeListItem>();

        public class FeeTypeListItem
        {
            public string Text { get; set; }
            public ExciseFeeType Value { get; set; }
        }
    }
}
