using CustomerVehicleManagement.Shared.Models.Contactable;
using Menominee.Common.Enums;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Telerik.Blazor;

namespace Menominee.Client.Components.Phones
{
    public partial class PhonesEditor : ComponentBase
    {
        public PhoneToWrite Phone { get; set; }

        [Parameter]
        public IList<PhoneToWrite> Phones { get; set; }

        [Parameter]
        public bool Enabled { get; set; }

        [Parameter]
        public FormMode FormMode { get; set; } = FormMode.Unknown;

        [CascadingParameter]
        public DialogFactory Dialogs { get; set; }

        private PhoneToWrite phoneUnchanged;

        private List<PhoneToWrite> DeletedPhones { get; set; } = new();

        List<PhoneTypeEnumModel> PhoneTypeEnumData { get; set; } = new List<PhoneTypeEnumModel>();

        protected override void OnInitialized()
        {
            foreach (PhoneType item in Enum.GetValues(typeof(PhoneType)))
            {
                PhoneTypeEnumData.Add(new PhoneTypeEnumModel { DisplayText = item.ToString(), Value = item });
            }
        }

        public void Reset()
        {
            if (FormMode == FormMode.Add)
                Phone = new();

            if (FormMode == FormMode.Edit)
                Phone = phoneUnchanged;

            FormMode = FormMode.Unknown;
        }

        private void Edit(PhoneToWrite phone)
        {
            if (phone is not null)
            {
                Phone = phone;
                FormMode = FormMode.Edit;
                phoneUnchanged = SetPhoneToUnchange(phone);
            }
        }

        private static PhoneToWrite SetPhoneToUnchange(PhoneToWrite phone)
        {
            return new PhoneToWrite
            {
                Id = phone.Id,
                Number = phone.Number,
                PhoneType = phone.PhoneType,
                IsPrimary = phone.IsPrimary
            };
        }

        private void Add()
        {
            Phone = new();
            FormMode = FormMode.Add;
        }

        private void Save()
        {
            if (Phone is not null && FormMode == FormMode.Add)
                Phones.Add(Phone);

            // Restore phones marked for deletion before saving to data service
            foreach (var phone in DeletedPhones)
                Phones.Add(phone);

            FormMode = FormMode.Unknown;
        }

        private async Task RemoveAsync()
        {
            if (FormMode == FormMode.Add || FormMode == FormMode.Edit)
            {
                if (await RemoveConfirm())
                {
                    // Existing phone from database has id not equal to zero.
                    if (Phone is not null
                        &&
                        Phone?.Id != 0)

                        // User added phone but now is removing it; has id equal to zero.
                        if (Phone is not null
                            &&
                            Phone?.Id == 0)
                            Phones.Remove(Phone);

                    // Hide phone marked for deletion from user
                    DeletedPhones.Add(Phone);
                    Phones.Remove(Phone);
                    Reset();
                }
            }
        }

        public async Task<bool> RemoveConfirm()
        {
            return await Dialogs.ConfirmAsync($"Are you sure you want to remove phone number {Regex.Replace(Phone.Number, @"(\d{3})(\d{3})(\d{4})", "$1-$2-$3")}?", "Remove Phone");
        }

    }

    internal class PhoneTypeEnumModel
    {
        public PhoneType Value { get; set; }
        public string DisplayText { get; set; }
    }
}
