using Menominee.Shared.Models.RepairOrders;
using Menominee.Shared.Models.RepairOrders.Warranties;
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

        private bool DialogVisible { get; set; } = false;

        private bool CanEdit { get; set; } = false;
        private bool CanCopy { get; set; } = false;
        private bool CanClear { get; set; } = false;

        public IList<WarrantyListItem> WarrantyList { get; set; } = new List<WarrantyListItem>();

        public WarrantyListItem Warranty { get; set; }

        public void Save()
        {
            UpdateMissingWarrantyCount();
            DialogVisible = false;
            Updated.InvokeAsync(WarrantiesMissingCount);
        }

        private void OnRowSelected(GridRowClickEventArgs args)
        {
            Warranty = args.Item as WarrantyListItem;
        }

        protected override void OnParametersSet()
        {
            WarrantyList = RepairOrderHelper.BuildWarrantyList(RepairOrder.Services);
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
