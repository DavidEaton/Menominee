using CustomerVehicleManagement.Shared.Models;
using Menominee.Common.Enums;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Telerik.Blazor.Components;

namespace Menominee.OrganizationDataContracts.Components
{
    public partial class PhonesEditor : ComponentBase
    {
        public PhoneToWrite PhoneToWrite { get; set; }

        [Parameter]
        public IList<PhoneToWrite> PhonesToWrite { get; set; }

        [Parameter]
        public FormMode FormMode { get; set; }
        List<PhoneTypeEnumModel> PhoneTypeEnumData { get; set; } = new List<PhoneTypeEnumModel>();
        private bool DialogVisible => PhoneToWrite != null && (Adding || Editing);
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

        private void Edit(PhoneToWrite item)
        {
            PhoneToWrite = item;
            Editing = true;
        }

        private async Task AddAsync()
        {
            if (PhoneToWrite == null)
                PhoneToWrite = new();

            if (PhoneNumberControl != null)
                await PhoneNumberControl.FocusAsync();

            Adding = true;
        }

        private void Save()
        {
            if (PhoneToWrite == null)
                PhonesToWrite.Add(PhoneToWrite);

            Adding = false;
            Editing = false;
        }

        private void CancelAddPhone()
        {
            if (PhoneToWrite != null)
                PhoneToWrite = null;

            Adding = false;
            Editing = false;
        }

        private void CancelEditPhone()
        {
            if (PhoneToWrite != null)
                PhoneToWrite = null;

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
