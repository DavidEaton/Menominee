using CustomerVehicleManagement.Shared.Models.RepairOrders;
using CustomerVehicleManagement.Shared.Models.RepairOrders.SerialNumbers;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using Telerik.Blazor.Components;

namespace Menominee.Client.Components.RepairOrders
{
    public partial class RepairOrderSerialNumbersTab : ComponentBase
    {
        [Parameter]
        public RepairOrderToWrite RepairOrder { get; set; }

        [Parameter]
        public EventCallback<int> Updated { get; set; }

        public int SerialNumbersMissingCount { get; set; }

        private bool DialogVisible { get; set; } = false;

        private bool CanEdit { get; set; } = false;

        public IList<SerialNumberListItem> SerialNumberList { get; set; } = new List<SerialNumberListItem>();
        public SerialNumberListItem SerialNumber { get; set; }

        public void Save()
        {
            UpdateMissingSerialNumberCount();
            DialogVisible = false;
            Updated.InvokeAsync(SerialNumbersMissingCount);
        }

        private void OnRowSelected(GridRowClickEventArgs args)
        {
            SerialNumber = args.Item as SerialNumberListItem;
        }

        protected override void OnParametersSet()
        {
            SerialNumberList = RepairOrderHelper.BuildSerialNumberList(RepairOrder.Services);
        }

        private void UpdateMissingSerialNumberCount()
        {
            SerialNumbersMissingCount = RepairOrderHelper.SerialNumbersRequiredMissingCount(RepairOrder.Services);
        }
    }
}
