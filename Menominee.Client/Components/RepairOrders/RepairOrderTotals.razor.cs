using Microsoft.AspNetCore.Components;

namespace Menominee.Client.Components.RepairOrders
{
    public partial class RepairOrderTotals
    {
        [Parameter]
        public EventCallback OnDiscard { get; set; }

    }
}
