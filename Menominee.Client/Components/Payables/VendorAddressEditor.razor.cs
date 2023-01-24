using CustomerVehicleManagement.Shared.Models.Payables.Vendors;
using Menominee.Common.Enums;
using Microsoft.AspNetCore.Components;

namespace Menominee.Client.Components.Payables
{
    public partial class VendorAddressEditor : ComponentBase
    {
        [Parameter]
        public VendorToWrite Vendor { get; set; }

        public FormMode FormMode { get; set; } = FormMode.Unknown;

        private void AddAddress()
        {
            Vendor.Address = new();
            FormMode = FormMode.Add;
        }

        public void EditAddress()
        {
            FormMode = FormMode.Edit;
        }

        private void RemoveAddress()
        {
            Vendor.Address = null;
        }
    }
}

