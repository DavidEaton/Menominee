using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MenomineePlayWASM.Client.Components.RepairOrders
{
    public partial class RepairOrderInspectionTab : ComponentBase
    {
        [Parameter]
        public List<Inspection> CurrentInspections { get; set; }

        [Parameter]
        public List<Inspection> PreviousInspections { get; set; }

    }
}
