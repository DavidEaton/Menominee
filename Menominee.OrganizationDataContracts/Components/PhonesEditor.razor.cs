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
        [Parameter]
        public IList<PhoneToEdit> PhonesToEdit { get; set; }
        public PhoneToEdit PhoneToEdit { get; set; }

        [Parameter]
        public IList<PhoneToAdd> PhonesToAdd { get; set; }
        public PhoneToAdd PhoneToAdd { get; set; }

        [Parameter]
        public FormMode FormMode { get; set; }
        List<PhoneTypeEnumModel> PhoneTypeEnumData { get; set; } = new List<PhoneTypeEnumModel>();
        private bool DialogVisible => PhoneToEdit != null && (Adding || Editing);
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

        private void Edit(PhoneToEdit item)
        {
            PhoneToEdit = item;
            Editing = true;
        }

        private async Task AddAsync(string type)
        {
            if (type == "PhoneToAdd")
                PhoneToAdd = new();

            if (type == "PhoneToEdit")
                PhoneToEdit = new();

            if (PhoneNumberControl != null)
                await PhoneNumberControl.FocusAsync();

            Adding = true;
        }

        private void Save(string type)
        {
            if (type == "PhoneToAdd")
                PhonesToAdd.Add(PhoneToAdd);

            if (type == "PhoneToEdit" && Adding)
                PhonesToEdit.Add(PhoneToEdit);

            Adding = false;
            Editing = false;
        }

        private void CancelAddPhone()
        {
            Adding = false;
            Editing = false;

            if (PhoneToAdd != null)
                PhoneToAdd = null;
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
