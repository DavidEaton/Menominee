using Menominee.Client.Shared.Models;
using Menominee.Common.Enums;
using Menominee.Shared.Models.Addresses;
using Microsoft.AspNetCore.Components;

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
            {
                StateProvinceEnumData.Add(new StateProvinceEnumModel
                {
                    DisplayText = item.ToString(),
                    FullDisplayText = item.ToString() + " - " + EnumExtensions.GetDisplayName(item),
                    Value = item
                });
            }
            StateProvinceEnumData = StateProvinceEnumData.OrderBy(state => state.DisplayText).ToList();
        }

        protected override void OnAfterRender(bool firstRender)
        {
            if (firstRender)
            {
                addressOriginal = new AddressToWrite
                {
                    AddressLine1 = Address.AddressLine1,
                    City = Address.City,
                    State = Address.State,
                    PostalCode = Address.PostalCode,
                    AddressLine2 = Address.AddressLine2,
                };
            }
        }

        public void Reset()
        {
            Address.AddressLine1 = addressOriginal.AddressLine1;
            Address.City = addressOriginal.City;
            Address.State = addressOriginal.State;
            Address.PostalCode = addressOriginal.PostalCode;
            Address.AddressLine2 = addressOriginal.AddressLine2;
        }

        List<StateProvinceEnumModel> StateProvinceEnumData { get; set; } = new List<StateProvinceEnumModel>();
    }
}
