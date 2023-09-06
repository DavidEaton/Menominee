using Menominee.Shared.Models.RepairOrders;
using Menominee.Shared.Models.RepairOrders.SerialNumbers;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using Telerik.Blazor.Components;

namespace Menominee.Client.Components.RepairOrders
{
    public partial class RepairOrderSerialNumbersTab : ComponentBase
    {
        [Parameter]
        public RepairOrderToWrite RepairOrderToEdit { get; set; }

        [Parameter]
        public EventCallback<RepairOrderToWrite> RepairOrderToEditChanged { get; set; }

        [Parameter]
        public EventCallback<int> Updated { get; set; }

        public int SerialNumbersMissingCount { get; set; }

        private bool DialogVisible { get; set; } = false;

        private bool CanEdit { get; set; } = false;

        public List<SerialNumberListItem> SerialNumbers { get; set; } = new();

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
            SerialNumbers = RepairOrderHelper.BuildSerialNumberList(RepairOrderToEdit.Services);
        }

        private void UpdateMissingSerialNumberCount()
        {
            SerialNumbersMissingCount = RepairOrderHelper.SerialNumberRequiredMissingCount(RepairOrderToEdit.Services);
        }
    }
}
