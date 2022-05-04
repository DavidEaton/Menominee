using CustomerVehicleManagement.Shared.Models.CreditCards;
using Menominee.Client.Services.CreditCards;
using Menominee.Common.Enums;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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

        private List<FeeTypeListItem> feeTypeList { get; set; } = new List<FeeTypeListItem>();

        public class FeeTypeListItem
        {
            public string Text { get; set; }
            public CreditCardFeeType Value { get; set; }
        }
    }
}
