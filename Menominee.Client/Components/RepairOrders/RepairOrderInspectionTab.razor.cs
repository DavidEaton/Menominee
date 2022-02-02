using Microsoft.AspNetCore.Components;
using System.Collections.Generic;

namespace Menominee.Client.Components.RepairOrders
{
    public partial class RepairOrderInspectionTab : ComponentBase
    {
        [Parameter]
        public List<Inspection> CurrentInspections { get; set; }

        [Parameter]
        public List<Inspection> PreviousInspections { get; set; }

    }
}
