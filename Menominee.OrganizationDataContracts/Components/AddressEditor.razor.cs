using CustomerVehicleManagement.Shared.Models;
using Menominee.Common.Enums;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;

namespace Menominee.OrganizationDataContracts.Components
{
    public partial class AddressEditor : ComponentBase
    {
        [Parameter]
        public AddressToWrite Address { get; set; }

        [Parameter]
        public bool Enabled { get; set; }

        [Parameter]
        public EventCallback EditingAddress { get; set; }

        [Parameter]
        public EventCallback Ok { get; set; }

        [Parameter]
        public EventCallback Cancel { get; set; }

        [Parameter]
        public EventCallback Updated { get; set; }

        //[CascadingParameter]
        //BlazoredModalInstance BlazoredModal { get; set; }

        protected override void OnInitialized()
        {
            foreach (State item in Enum.GetValues(typeof(State)))
            {
                StateEnumData.Add(new StateEnumModel { DisplayText = item.ToString(), Value = item });
            }

            base.OnInitialized();
        }

        List<StateEnumModel> StateEnumData { get; set; } = new List<StateEnumModel>();
    }
    public class StateEnumModel
    {
        public State Value { get; set; }
        public string DisplayText { get; set; }
    }
}
