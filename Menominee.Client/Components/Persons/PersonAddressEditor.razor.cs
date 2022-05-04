using Microsoft.AspNetCore.Components;
using Menominee.Common.Enums;
using CustomerVehicleManagement.Shared.Models.Persons;

namespace Menominee.Client.Components.Persons
{
    public partial class PersonAddressEditor : ComponentBase
    {
        [Parameter] public PersonToWrite Person { get; set; }

        public FormMode FormMode { get; set; } = FormMode.Unknown;

        private void AddAddress()
        {
            Person.Address = new();
            FormMode = FormMode.Add;
        }

        public void EditAddress()
        {
            FormMode = FormMode.Edit;
        }

        private void RemoveAddress()
        {
            Person.Address = null;
        }
    }
}
