using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MenomineePlayWASM.Client.Components.RepairOrders
{
    public partial class RepairOrderPurchasesTab : ComponentBase
    {
        [Parameter]
        public List<Purchase> Purchases { get; set; } = null;

    }
}
