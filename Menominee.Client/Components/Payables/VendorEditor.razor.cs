using CustomerVehicleManagement.Shared.Models.Payables.Vendors;
using Menominee.Client.Shared;
using Menominee.Common.Enums;
using Microsoft.AspNetCore.Components;

namespace Menominee.Client.Components.Payables
{
    public partial class VendorEditor : ComponentBase
    {
        [Parameter]
        public VendorToWrite Vendor { get; set; }

        [Parameter]
        public EventCallback OnValidSubmit { get; set; }
        [Parameter]
        public EventCallback OnDiscard { get; set; }

        [Parameter]
        public FormMode FormMode { get; set; }

        private string Title { get; set; }

        protected override void OnParametersSet()
        {
            Title = FormTitle.BuildTitle(FormMode, "Vendor");

            if (Vendor?.Address is null)
                Vendor.Address = new();
        }
    }
}
