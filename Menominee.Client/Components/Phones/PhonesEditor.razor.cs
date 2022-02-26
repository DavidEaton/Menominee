using CustomerVehicleManagement.Shared.Models;
using Menominee.Common.Enums;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;

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
        public FormMode FormMode { get; set; }

        private PhoneToWrite phoneOriginal;

        private bool Adding { get; set; }
        private bool Editing { get; set; }

        private bool DialogVisible => Phone != null && (Adding || Editing);
        List<PhoneTypeEnumModel> PhoneTypeEnumData { get; set; } = new List<PhoneTypeEnumModel>();

        protected override void OnInitialized()
        {
            foreach (PhoneType item in Enum.GetValues(typeof(PhoneType)))
            {
                PhoneTypeEnumData.Add(new PhoneTypeEnumModel { DisplayText = item.ToString(), Value = item });
            }

            base.OnInitialized();
        }

       public void Reset()
        {
            Phone.Number = phoneOriginal.Number;
            Phone.PhoneType = phoneOriginal.PhoneType;
            Phone.IsPrimary = phoneOriginal.IsPrimary;
        }

        private void Edit(PhoneToWrite item)
        {
            if (item is not null)
            {
                Phone = item;
                Editing = true;

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
            Adding = true;
        }

        private void Save()
        {
            if (Phone != null && Adding)
            {
                Phones.Add(Phone);
                Adding = false;
            }

            if (Phone != null && Editing)
                Editing = false;
        }

        private void Cancel()
        {
            if (Phone != null && Adding)
                Phone = new();

            if (Phone != null && Editing)
                Reset();

            Adding = false;
            Editing = false;
        }
    }
    internal class PhoneTypeEnumModel
    {
        public PhoneType Value { get; set; }
        public string DisplayText { get; set; }
    }
}
