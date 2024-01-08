using Menominee.Domain.Enums;
using Menominee.Shared.Models.Taxes;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

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
                TaxTypeList.Add(new TaxTypeListItem { Text = EnumExtensions.GetDisplayName(taxType), Value = taxType });
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

        private List<TaxTypeListItem> TaxTypeList { get; set; } = new List<TaxTypeListItem>();

        public class TaxTypeListItem
        {
            public string Text { get; set; }
            public SalesTaxType Value { get; set; }
        }
    }
}
