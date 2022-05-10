using CustomerVehicleManagement.Shared.Models.Taxes;
using Menominee.Common.Enums;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Menominee.Client.Components.Settings
{
    public partial class SalesTaxEditor
    {
        [Parameter]
        public SalesTaxToWrite SalesTax { get; set; }

        [Parameter]
        public string Title { get; set; } = "Edit Sales Tax";

        [Parameter]
        public EventCallback OnSave { get; set; }

        [Parameter]
        public EventCallback OnCancel { get; set; }

        [Inject]
        IJSRuntime JsInterop { get; set; }

        //private bool parametersSet = false;

        protected override async Task OnInitializedAsync()
        {
            foreach (SalesTaxType taxType in Enum.GetValues(typeof(SalesTaxType)))
            {
                taxTypeList.Add(new TaxTypeListItem { Text = EnumExtensions.GetDisplayName(taxType), Value = taxType });
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

        public async Task Focus(string elementId)
        {
            await JsInterop.InvokeVoidAsync("jsfunction.focusElement", elementId);
        }

        //protected override void OnParametersSet()
        //{
        //    if (parametersSet)
        //        return;

        //    parametersSet = true;

        //    StateHasChanged();
        //}

        private List<TaxTypeListItem> taxTypeList { get; set; } = new List<TaxTypeListItem>();

        public class TaxTypeListItem
        {
            public string Text { get; set; }
            public SalesTaxType Value { get; set; }
        }
    }
}
