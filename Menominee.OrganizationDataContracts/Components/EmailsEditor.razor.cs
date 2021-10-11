using CustomerVehicleManagement.Shared.Models;
using Microsoft.AspNetCore.Components;
using Menominee.Common.Enums;
using System.Collections.Generic;

namespace Menominee.OrganizationDataContracts.Components
{
    public partial class EmailsEditor : ComponentBase
    {
        [Parameter]
        public FormMode FormMode { get; set; }

        [Parameter]
        public IList<EmailToAdd> EmailsToAdd { get; set; }

        [Parameter]
        public IList<EmailToEdit> EmailsToUpdate { get; set; }

    }
}
