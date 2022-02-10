using CustomerVehicleManagement.Shared.Models.RepairOrders.Services;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;

namespace Menominee.Client.Components.RepairOrders
{
    public partial class RepairOrderServicesTab
    {
        [Parameter]
        public IList<RepairOrderServiceToWrite> Services { get; set; }

        private bool CanAddPart { get; set; } = true;
        private bool CanAddLabor { get; set; } = true;

        private void OnAddPart()
        {
        }

        private void OnAddLabor()
        {
        }
    }
}
