using CustomerVehicleManagement.Shared.Models;
using Microsoft.AspNetCore.Components;
using Menominee.Common.Enums;
using System.Collections.Generic;

namespace Menominee.OrganizationDataContracts.Components
{
    public partial class PhonesEditor : ComponentBase
    {
        [Parameter]
        public IList<PhoneToEdit> PhonesToUpdate { get; set; }

        [Parameter]
        public IList<PhoneToAdd> PhonesToAdd { get; set; }

        [Parameter]
        public FormMode FormMode { get; set; }
    }
}
