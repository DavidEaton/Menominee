using CustomerVehicleManagement.Shared.Models;
using Microsoft.AspNetCore.Components;

namespace Menominee.OrganizationDtos.Components
{
    public partial class AddressFormTemplate<TItem> : ComponentBase
        where TItem : class
    {
        [Parameter]
        public RenderFragment<TItem> EditTemplate { set; get; }

        [Parameter]
        public AddressUpdateDto AddressToEdit { get; set; }
    }
}
