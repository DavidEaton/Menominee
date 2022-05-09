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

        private PhoneToWrite phoneOriginal;

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
            Phone.Number = phoneOriginal.Number;
            Phone.PhoneType = phoneOriginal.PhoneType;
            Phone.IsPrimary = phoneOriginal.IsPrimary;
        }

        private void Edit(PhoneToWrite phone)
        {
            if (phone is not null)
            {
                Phone = phone;
                FormMode = FormMode.Edit;

                phoneOriginal = new PhoneToWrite
                {
                    Number = Phone.Number,
                    PhoneType = Phone.PhoneType,
                    IsPrimary = Phone.IsPrimary
                };
            }
        }

        private void Add()
        {
            Phone = new();
            FormMode = FormMode.Add;
        }

        private void Save()
        {
            if (Phone != null && FormMode == FormMode.Add)
                Phones.Add(Phone);

            FormMode = FormMode.Unknown;
        }

        private void Cancel()
        {
            if (Phone != null && FormMode == FormMode.Add)
                Phone = new();

            if (Phone != null && FormMode == FormMode.Edit)
                Reset();

            FormMode = FormMode.Unknown;
        }

        private async Task RemoveAsync()
        {
            if (await RemoveConfirm())
            {
                Phones.Remove(Phone);
                FormMode = FormMode.Unknown;
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
