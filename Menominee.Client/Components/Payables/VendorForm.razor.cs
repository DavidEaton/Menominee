using CustomerVehicleManagement.Shared.CustomerVehicleManagement.Shared.Models.Payables.Vendors;
using Microsoft.AspNetCore.Components;

namespace Menominee.Client.Components.Payables
{
    public partial class VendorForm : ComponentBase
    {
        [Parameter]
        public VendorToWrite Vendor { get; set; }

        [Parameter]
        public string Title { get; set; } = "Edit Vendor";

        [Parameter]
        public EventCallback OnValidSubmit { get; set; }
        [Parameter]
        public EventCallback OnDiscard { get; set; }

        //[Parameter]
        //public TItem Item { get; set; }

        //[Parameter]
        //public string Title { get; set; } = "Edit Vendor";

        //[Parameter]
        //public string VendorCodeProperty { get; set; }

        //[Parameter]
        //public string NameProperty { get; set; }

        //[Parameter]
        //public EventCallback Updated { get; set; }

        //public string ItemName
        //{
        //    get
        //    {
        //        return Item.GetType().Name;
        //    }
        //}

        //public string VendorCode
        //{
        //    get
        //    {
        //        return Item.GetType()
        //            .GetProperty(VendorCodeProperty)
        //            .GetValue(Item).ToString();
        //    }
        //    set
        //    {
        //        Item.GetType()
        //            .GetProperty(VendorCodeProperty)
        //            .SetValue(Item, value);
        //        Updated.InvokeAsync();
        //    }
        //}

        //public string Name
        //{
        //    get
        //    {
        //        return Item.GetType()
        //            .GetProperty(NameProperty)
        //            .GetValue(Item).ToString();
        //    }
        //    set
        //    {
        //        Item.GetType()
        //            .GetProperty(NameProperty)
        //            .SetValue(Item, value);
        //        Updated.InvokeAsync();
        //    }
        //}

        //[Parameter]
        //public EventCallback OnDiscard { get; set; }

        //[Inject]
        //public IVendorRepository vendorRepository { get; set; }

        //protected async override Task OnInitializedAsync()
        //{

        //}
    }
}
