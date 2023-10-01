using Menominee.Common.Enums;
using Menominee.Shared.Models.CreditCards;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Menominee.Client.Components.Settings
{
    public partial class CreditCardEditor
    {
        [Parameter]
        public CreditCardToWrite CreditCard { get; set; }

        [Parameter]
        public string Title { get; set; } = "Edit Credit Card";

        [Parameter]
        public EventCallback OnSave { get; set; }

        [Parameter]
        public EventCallback OnCancel { get; set; }

        [Inject]
        IJSRuntime JsInterop { get; set; }

        //private bool parametersSet = false;

        protected override async Task OnInitializedAsync()
        {
            foreach (CreditCardFeeType feeType in Enum.GetValues(typeof(CreditCardFeeType)))
            {
                feeTypeList.Add(new FeeTypeListItem { Text = EnumExtensions.GetDisplayName(feeType), Value = feeType });
            }

            await base.OnInitializedAsync();
        }

        //protected override void OnParametersSet()
        //{
        //    if (parametersSet)
        //        return;

        //    parametersSet = true;

        //    StateHasChanged();
        //}

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

        private List<FeeTypeListItem> feeTypeList { get; set; } = new List<FeeTypeListItem>();

        public class FeeTypeListItem
        {
            public string Text { get; set; }
            public CreditCardFeeType Value { get; set; }
        }
    }
}
