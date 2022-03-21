using CustomerVehicleManagement.Shared.Helpers;
using CustomerVehicleManagement.Shared.Models.RepairOrders;
using Menominee.Client.Components.RepairOrders.Models;
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

        private bool EditDialogVisible { get; set; } = false;

        private bool CanEdit { get; set; } = false;

        public IList<SerialNumberListItem> SerialNumberList { get; set; } = new List<SerialNumberListItem>();
        public SerialNumberListItem SerialNumber { get; set; }

        public void Save()
        {
            UpdateMissingSerialNumberCount();
            EditDialogVisible = false;
        }

        private void OnRowSelected(GridRowClickEventArgs args)
        {
            SerialNumber = args.Item as SerialNumberListItem;
        }

        protected override void OnParametersSet()
        {
            BuildSerialNumberList();
        }

        private void BuildSerialNumberList()
        {
            SerialNumberList = new List<SerialNumberListItem>();
            foreach (var service in RepairOrder.Services)
            {
                foreach (var item in service.Items)
                {
                    foreach (var serialNumber in item.SerialNumbers)
                    {
                        SerialNumberList.Add(new SerialNumberListItem()
                        {
                            Description = item.Description,
                            PartNumber = item.PartNumber,
                            RepairOrderItemId = serialNumber.RepairOrderItemId,
                            SerialNumberType = serialNumber
                        });
                    }
                }
            }
        }

        private void UpdateMissingSerialNumberCount()
        {
            SerialNumbersMissingCount = RepairOrderHelper.MissingSerialNumberCount(RepairOrder.Services);
        }
    }
}
