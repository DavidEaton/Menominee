using Menominee.Domain.Enums;
using Menominee.Shared.Models.CreditCards;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Menominee.Client.Components.Settings
{
    public partial class CreditCardEditor
    {
        [Parameter]
        public CreditCardToWrite? CreditCard { get; set; }

        [Parameter]
        public string Title { get; set; } = "Edit Credit Card";

        [Parameter]
        public EventCallback OnSave { get; set; }

        [Parameter]
        public EventCallback OnCancel { get; set; }

        [Inject]
        private IJSRuntime? JsInterop { get; set; }

        protected override async Task OnInitializedAsync()
        {
            foreach (CreditCardFeeType feeType in Enum.GetValues(typeof(CreditCardFeeType)))
            {
                FeeTypeList.Add(new FeeTypeListItem { DisplayText = EnumExtensions.GetDisplayName(feeType), Value = feeType });
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
            if (JsInterop is not null)
                await JsInterop.InvokeVoidAsync("jsfunction.focusElement", elementId);
        }

        private List<FeeTypeListItem> FeeTypeList { get; set; } = new List<FeeTypeListItem>();

        public class FeeTypeListItem
        {
            public CreditCardFeeType Value { get; set; }
            public string DisplayText { get; set; } = string.Empty;
        }
    }
}
