using CustomerVehicleManagement.Shared.Models;
using Menominee.Common.Enums;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;

namespace Menominee.Client.Components.Address
{
    public partial class AddressEditor : ComponentBase
    {
        [Parameter]
        public AddressToWrite Address { get; set; }

        [Parameter]
        public bool Enabled { get; set; }

        [Parameter]
        public FormMode FormMode { get; set; }

        private AddressToWrite addressOriginal;

        public void Cancel()
        {
            if (FormMode == FormMode.Edit)
                Reset();
        }

        protected override void OnInitialized()
        {
            foreach (State item in Enum.GetValues(typeof(State)))
                StateEnumData.Add(new StateEnumModel
                {
                    DisplayText = item.ToString(),
                    Value = item
                });
        }

        protected override void OnAfterRender(bool firstRender)
        {
            if (firstRender)
            {
                addressOriginal = new AddressToWrite
                {
                    AddressLine = Address.AddressLine,
                    City = Address.City,
                    State = Address.State,
                    PostalCode = Address.PostalCode
                };
            }
        }

        public void Reset()
        {
            Address.AddressLine = addressOriginal.AddressLine;
            Address.City = addressOriginal.City;
            Address.State = addressOriginal.State;
            Address.PostalCode = addressOriginal.PostalCode;
        }

        List<StateEnumModel> StateEnumData { get; set; } = new List<StateEnumModel>();
    }
    public class StateEnumModel
    {
        public State Value { get; set; }
        public string DisplayText { get; set; }
    }
}
