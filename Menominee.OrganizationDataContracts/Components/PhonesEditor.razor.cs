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
        private bool DialogVisible => (PhoneToWrite != null && (Adding || Editing)) || (PhoneToWrite != null && (Adding || Editing));
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

        private async Task AddAsync(string type)
        {
            if (type == "PhoneToWrite")
                PhoneToWrite = new();

            if (type == "PhoneToWrite")
                PhoneToWrite = new();

            if (PhoneNumberControl != null)
                await PhoneNumberControl.FocusAsync();

            Adding = true;
        }

        private void Save(string type)
        {
            if (type == "PhoneToWrite")
                PhonesToWrite.Add(PhoneToWrite);

            if (type == "PhoneToWrite" && Adding)
                PhonesToWrite.Add(PhoneToWrite);

            Adding = false;
            Editing = false;
        }

        private void CancelAddPhone()
        {
            Adding = false;
            Editing = false;

            if (PhoneToWrite != null)
                PhoneToWrite = null;
        }

        private void CancelEditPhone()
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
