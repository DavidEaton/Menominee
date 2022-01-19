using CustomerVehicleManagement.Shared.Models;
using Menominee.Common.Enums;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Telerik.Blazor.Components;

namespace Menominee.Client.Components
{
    public partial class PhonesEditor : ComponentBase
    {
        public PhoneToWrite PhoneToWrite { get; set; }

        [Parameter]
        public IList<PhoneToWrite> PhonesToWrite { get; set; }

        List<PhoneTypeEnumModel> PhoneTypeEnumData { get; set; } = new List<PhoneTypeEnumModel>();
        private bool DialogVisible => Adding || Editing;
        private bool Adding { get; set; } = false;
        private bool Editing { get; set; } = false;
        private TelerikMaskedTextBox PhoneNumberControl { get; set; }
        protected override void OnInitialized()
        {
            foreach (PhoneType item in Enum.GetValues(typeof(PhoneType)))
            {
                PhoneTypeEnumData.Add(new PhoneTypeEnumModel { DisplayText = item.ToString(), Value = item });
            }

            base.OnInitialized();
        }

        private async Task EditAsync(PhoneToWrite item)
        {
            PhoneToWrite = item;
            Editing = true;

            if (PhoneNumberControl != null)
                await PhoneNumberControl.FocusAsync();
        }

        private async Task AddAsync()
        {
            PhoneToWrite = new();
            Adding = true;

            if (PhoneNumberControl != null)
                await PhoneNumberControl.FocusAsync();
        }

        private void Save()
        {
            if (PhoneToWrite != null && Adding)
            {
                PhonesToWrite.Add(PhoneToWrite);
                Adding = false;
            }

            if (PhoneToWrite != null && Editing)
                Editing = false;
        }

        private void CancelEdit()
        {
            Adding = false;
            Editing = false;
        }

    }
    public class PhoneTypeEnumModel
    {
        public PhoneType Value { get; set; }
        public string DisplayText { get; set; }
    }
}
