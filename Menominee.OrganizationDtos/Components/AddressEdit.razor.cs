using CustomerVehicleManagement.Shared.Models;
using Microsoft.AspNetCore.Components;
using System;

namespace Menominee.OrganizationDtos.Components
{
    public partial class AddressEdit : ComponentBase
    {
        [Parameter]
        public AddressUpdateDto Address { get; set; }
    }
}
