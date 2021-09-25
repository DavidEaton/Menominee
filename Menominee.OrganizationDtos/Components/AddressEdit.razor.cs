using CustomerVehicleManagement.Shared.Models;
using Microsoft.AspNetCore.Components;

namespace Menominee.OrganizationDtos.Components
{
    public partial class AddressEdit : ComponentBase
    {
        [Parameter]
        public AddressUpdateDto Address { get; set; }
    }
}
