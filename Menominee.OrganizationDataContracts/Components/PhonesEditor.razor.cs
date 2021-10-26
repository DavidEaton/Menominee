using CustomerVehicleManagement.Shared.Models;
using Microsoft.AspNetCore.Components;
using Menominee.Common.Enums;
using System.Collections.Generic;
using System;
using Telerik.Blazor.Components;
using System.Threading.Tasks;

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

        private bool AddingPhone { get; set; } = false;
        private TelerikMaskedTextBox PhoneNumberControl { get; set; }
        protected override void OnInitialized()
        {
            foreach (PhoneType item in Enum.GetValues(typeof(PhoneType)))
            {
                PhoneTypeEnumData.Add(new PhoneTypeEnumModel { DisplayText = item.ToString(), Value = item });
            }

            base.OnInitialized();
        }

        private async Task AddPhoneAsync(string type)
        {
            if (type == "PhoneToAdd")
                PhoneToAdd = new();

            if (type == "PhoneToEdit")
                PhoneToEdit = new();

            if (PhoneNumberControl != null)
                await PhoneNumberControl.FocusAsync();

            AddingPhone = true;
        }

        private void SavePhone(string type)
        {
            if (type == "PhoneToAdd")
                PhonesToAdd.Add(PhoneToAdd);

            if (type == "PhoneToEdit")
                PhonesToEdit.Add(PhoneToEdit);

            AddingPhone = false;
        }

        private void Cancel()
        {
            PhoneToAdd = null;
            PhoneToEdit = null;
            AddingPhone = false;
        }
    }
    public class PhoneTypeEnumModel
    {
        public PhoneType Value { get; set; }
        public string DisplayText { get; set; }
    }
}
