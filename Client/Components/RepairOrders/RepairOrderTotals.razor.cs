using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MenomineePlayWASM.Client.Components.RepairOrders
{
    public partial class RepairOrderTotals
    {
        [Parameter]
        public EventCallback OnDiscard { get; set; }

    }
}
