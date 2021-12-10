using CustomerVehicleManagement.Shared.Models;
using Microsoft.AspNetCore.Components;

namespace Menominee.OrganizationDataContracts.Components
{
    public partial class EmailEditor : ComponentBase
    {
        [Parameter]
        public EmailToWrite Email { get; set; }

        [Parameter]
        public EventCallback Saved { get; set; }

        [Parameter]
        public EventCallback Cancelled { get; set; }

        [Parameter]
        public bool DialogVisible { get; set; }
    }
}
