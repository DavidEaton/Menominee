using Menominee.Shared.Models.RepairOrders;
using Microsoft.AspNetCore.Components;

namespace Menominee.Client.Components.RepairOrders
{
    public partial class RepairOrderCustomerVehicleTab
    {
        [Parameter]
        public RepairOrderToWrite RepairOrder { get; set; }
    }
}
