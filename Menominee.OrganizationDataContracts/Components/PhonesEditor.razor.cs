using CustomerVehicleManagement.Shared.Models;
using Microsoft.AspNetCore.Components;
using SharedKernel.Enums;
using System.Collections.Generic;

namespace Menominee.OrganizationDataContracts.Components
{
    public partial class PhonesEditor : ComponentBase
    {
        [Parameter]
        public IList<PhoneUpdateDto> PhonesToUpdate { get; set; }

        [Parameter]
        public IList<PhoneCreateDto> PhonesToAdd { get; set; }

        [Parameter]
        public FormMode FormMode { get; set; }
    }
}
