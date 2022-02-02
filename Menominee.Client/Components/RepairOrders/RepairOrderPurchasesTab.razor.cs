using Microsoft.AspNetCore.Components;
using System.Collections.Generic;

namespace Menominee.Client.Components.RepairOrders
{
    public partial class RepairOrderPurchasesTab : ComponentBase
    {
        [Parameter]
        public List<Purchase> Purchases { get; set; } = null;

    }
}
