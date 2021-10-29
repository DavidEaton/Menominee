using Menominee.Common.Enums;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;

namespace Menominee.OrganizationDataContracts.Components
{
    public partial class AddressEditor<TItem> : ComponentBase
    {
        [Parameter]
        public TItem Item { get; set; }

        [Parameter]
        public bool EnableEditor { get; set; }

        [Parameter]
        public string AddressLineProperty { get; set; }

        [Parameter]
        public string CityProperty { get; set; }

        [Parameter]
        public string StateProperty { get; set; }

        [Parameter]
        public string PostalCodeProperty { get; set; }

        [Parameter]
        public EventCallback Updated { get; set; }

        public string ItemName
        {
            get
            {
                return Item.GetType().Name;
            }
        }

        public string AddressLine
        {
            get
            {
                return Item.GetType()
                    .GetProperty(AddressLineProperty)
                    .GetValue(Item).ToString();
            }
            set
            {
                Item.GetType()
                    .GetProperty(AddressLineProperty)
                    .SetValue(Item, value);
                Updated.InvokeAsync();
            }
        }

        public string City
        {
            get
            {
                return Item.GetType()
                    .GetProperty(CityProperty)
                    .GetValue(Item).ToString();
            }
            set
            {
                Item.GetType()
                    .GetProperty(CityProperty)
                    .SetValue(Item, value);
                Updated.InvokeAsync();
            }
        }

        public State State
        {
            get
            {
                return (State)Convert.ToInt32(Item.GetType()
                    .GetProperty(StateProperty)
                    .GetValue(Item));
            }
            set
            {
                Item.GetType()
                    .GetProperty(StateProperty)
                    .SetValue(Item, value);
                Updated.InvokeAsync();
            }
        }

        public string PostalCode
        {
            get
            {
                return Item.GetType()
                    .GetProperty(PostalCodeProperty)
                    .GetValue(Item).ToString();
            }
            set
            {
                Item.GetType()
                    .GetProperty(PostalCodeProperty)
                    .SetValue(Item, value);
                Updated.InvokeAsync();
            }
        }

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
