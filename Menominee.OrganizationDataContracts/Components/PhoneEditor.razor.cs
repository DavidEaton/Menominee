using CustomerVehicleManagement.Shared.Models;
using Menominee.Common.Enums;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Telerik.Blazor.Components;

namespace Menominee.OrganizationDataContracts.Components
{
    public partial class PhoneEditor : ComponentBase
    {
        [Parameter]
        public PhoneToWrite PhoneToWrite { get; set; }

        [Parameter]
        public EventCallback Saved { get; set; }

        [Parameter]
        public EventCallback Cancelled { get; set; }

        [Parameter]
        public bool DialogVisible { get; set; }

        private TelerikMaskedTextBox PhoneNumberControl { get; set; }

        List<PhoneTypeEnumModel> PhoneTypeEnumData { get; set; } = new List<PhoneTypeEnumModel>();

        protected override void OnInitialized()
        {
            foreach (PhoneType item in Enum.GetValues(typeof(PhoneType)))
            {
                PhoneTypeEnumData.Add(new PhoneTypeEnumModel { DisplayText = item.ToString(), Value = item });
            }

            base.OnInitialized();
        }
        protected override async Task<Task> OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
                await PhoneNumberControl.FocusAsync();

            return base.OnAfterRenderAsync(firstRender);
        }
    }
    internal class PhoneTypeEnumModel
    {
        public PhoneType Value { get; set; }
        public string DisplayText { get; set; }
    }

}
