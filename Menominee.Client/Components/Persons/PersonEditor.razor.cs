using CustomerVehicleManagement.Shared.Models.Persons;
using Menominee.Common.Enums;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;

namespace Menominee.Client.Components.Persons
{
    public partial class PersonEditor : ComponentBase
    {
        [Parameter]
        public PersonToWrite Person { get; set; }

        [Parameter]
        public bool Enabled { get; set; }

        [Parameter]
        public FormMode FormMode { get; set; }

        private PersonToWrite personOriginal;

        protected override void OnAfterRender(bool firstRender)
        {
            if (firstRender)
            {
                personOriginal = new PersonToWrite
                {
                    Name = Person.Name,
                    Address = Person.Address,
                    Birthday = Person.Birthday,
                    DriversLicense = Person.DriversLicense,
                    Gender = Person.Gender,
                    Emails = Person.Emails,
                    Phones = Person.Phones
                };
            }
        }

        public void Reset()
        {
            Person.Name = personOriginal.Name;
            Person.Address = personOriginal.Address;
            Person.Birthday = personOriginal.Birthday;
            Person.DriversLicense = personOriginal.DriversLicense;
            Person.Gender = personOriginal.Gender;
            Person.Emails = personOriginal.Emails;
            Person.Phones = personOriginal.Phones;
        }

        protected override void OnInitialized()
        {
            foreach (Gender item in Enum.GetValues(typeof(Gender)))
            {
                GenderEnumData.Add(new GenderEnumModel { DisplayText = item.ToString(), Value = item });
            }
        }

        List<GenderEnumModel> GenderEnumData { get; set; } = new List<GenderEnumModel>();

        private class GenderEnumModel
        {
            public Gender Value { get; set; }
            public string DisplayText { get; set; }
        }
    }
}