using CustomerVehicleManagement.Shared.Models.RepairOrders;
using CustomerVehicleManagement.Shared.Models.RepairOrders.Warranties;
using Menominee.Common.Enums;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using Telerik.Blazor.Components;

namespace Menominee.Client.Components.RepairOrders
{
    public partial class RepairOrderWarrantiesTab : ComponentBase
    {
        [Parameter]
        public RepairOrderToWrite RepairOrder { get; set; }

        [Parameter]
        public EventCallback<int> Updated { get; set; }

        public int WarrantiesMissingCount { get; set; }

        private bool EditDialogVisible { get; set; } = false;

        private bool CanEdit { get; set; } = false;
        private bool CanCopy { get; set; } = false;
        private bool CanClear { get; set; } = false;

        public IList<WarrantyListItem> WarrantyList { get; set; } = new List<WarrantyListItem>();

        public WarrantyListItem Warranty { get; set; }

        public void Save()
        {
            UpdateMissingWarrantyCount();
            EditDialogVisible = false;
            Updated.InvokeAsync();
        }

        private void OnRowSelected(GridRowClickEventArgs args)
        {
            Warranty = args.Item as WarrantyListItem;
        }

        protected override void OnParametersSet()
        {
            BuildWarrantyList();
        }

        private void BuildWarrantyList()
        {
            WarrantyList = new List<WarrantyListItem>();
            foreach (var service in RepairOrder.Services)
            {
                foreach (var item in service.Items)
                {
                    foreach (var warranty in item.Warranties)
                    {
                        WarrantyList.Add(new WarrantyListItem()
                        {
                            Type = (WarrantyType)warranty.Type,
                            Description = item.Description,
                            PartNumber = item.PartNumber,
                            RepairOrderItemId = warranty.RepairOrderItemId,
                            WarrantyType = warranty
                        });
                    }
                }
            }
        }

        private void UpdateMissingWarrantyCount()
        {
            WarrantiesMissingCount = RepairOrderHelper.WarrantyRequiredMissingCount(RepairOrder.Services);
        }

        private void OnEdit()
        {

        }

        private void OnCopy()
        {

        }

        private void OnClear()
        {

        }
    }
}
