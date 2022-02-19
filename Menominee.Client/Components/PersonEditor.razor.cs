using CustomerVehicleManagement.Shared.Models;
using Menominee.Common.Enums;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;

namespace Menominee.Client.Components
{
    public partial class PersonEditor : ComponentBase
    {
        [Parameter]
        public PersonToWrite Person { get; set; }

        [Parameter]
        public bool EnableEditor { get; set; }

        [Parameter]
        public EventCallback Updated { get; set; }

        [Parameter]
        public FormMode Mode
        {
            get => formMode;
            set
            {
                formMode = value;
                if (formMode == FormMode.Add)
                    Title = "Add";
                else if (formMode == FormMode.Edit)
                    Title = "Edit";
                else
                    Title = "View";
                Title += " Person";
            }
        }

        //private Gender Gender { get; set; } = Gender.Male;

        private FormMode formMode;
        private string Title { get; set; }

        protected override void OnInitialized()
        {
            foreach (Gender item in Enum.GetValues(typeof(Gender)))
            {
                GenderEnumData.Add(new GenderEnumModel { DisplayText = item.ToString(), Value = item });
            }

            base.OnInitialized();
        }

        List<GenderEnumModel> GenderEnumData { get; set; } = new List<GenderEnumModel>();

        private class GenderEnumModel
        {
            public Gender Value { get; set; }
            public string DisplayText { get; set; }
        }
    }
}