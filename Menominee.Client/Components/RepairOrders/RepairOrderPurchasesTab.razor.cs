using CustomerVehicleManagement.Shared.Models.RepairOrders;
using CustomerVehicleManagement.Shared.Models.RepairOrders.Purchases;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using Telerik.Blazor.Components;

namespace Menominee.Client.Components.RepairOrders
{
    public partial class RepairOrderPurchasesTab : ComponentBase
    {
        [Parameter]
        public RepairOrderToWrite RepairOrder { get; set; }

        [Parameter]
        public EventCallback<int> Updated { get; set; }

        public int PurchasesMissingCount { get; set; }

        private bool DialogVisible { get; set; } = false;

        private bool CanEdit { get; set; } = false;

        public List<PurchaseListItem> Purchases { get; set; } = new();

        public PurchaseListItem Purchase { get; set; }

        public void Save()
        {
            UpdateMissingSerialNumberCount();
            DialogVisible = false;
            Updated.InvokeAsync(PurchasesMissingCount);
        }

        private void OnRowSelected(GridRowClickEventArgs args)
        {
            Purchase = args.Item as PurchaseListItem;
        }

        protected override void OnParametersSet()
        {
            Purchases = RepairOrderHelper.BuildPurchaseList(RepairOrder.Services);
        }

        private void UpdateMissingSerialNumberCount()
        {
            PurchasesMissingCount = RepairOrderHelper.PurchaseRequiredMissingCount(RepairOrder.Services);
        }
    }
}
