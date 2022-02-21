using CustomerVehicleManagement.Shared.Models.RepairOrders;
using Microsoft.AspNetCore.Components;

namespace Menominee.Client.Components.RepairOrders
{
    public partial class RepairOrderTotals
    {
        [Parameter]
        public EventCallback OnDiscard { get; set; }
        [Parameter]
        public EventCallback OnSave { get; set; }

        [CascadingParameter]
        public RepairOrderToWrite RepairOrder { get; set; }

        private double SubTotal()
        {
            return RepairOrder.PartsTotal + RepairOrder.LaborTotal + RepairOrder.ShopSuppliesTotal;
        }

    }
}
