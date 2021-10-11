using CustomerVehicleManagement.Shared.Models;
using Microsoft.AspNetCore.Components;
using SharedKernel.Enums;
using System.Collections.Generic;

namespace Menominee.OrganizationDataContracts.Components
{
    public partial class EmailsEditor : ComponentBase
    {
        [Parameter]
        public FormMode FormMode { get; set; }

        [Parameter]
        public IList<EmailCreateDto> EmailsToAdd { get; set; }

        [Parameter]
        public IList<EmailUpdateDto> EmailsToUpdate { get; set; }

    }
}
