using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Linq;
using Telerik.Blazor.Components;

namespace Menominee.Client.Components.RepairOrders
{
    public partial class RepairOrderSerialNumbersTab : ComponentBase
    {
        [Parameter]
        public List<SerialNumberListItem> SerialNumberList { get; set; }

        [Parameter]
        public EventCallback Updated { get; set; }
        private bool EditDialogVisible { get; set; } = false;
        private bool CanEdit { get; set; } = false;
        public SerialNumberListItem SerialNumberToEdit { get; set; }

        public void Save()
        {
            EditDialogVisible = false;

            var mmops = SerialNumberList?.Count;

            // Update SerialNumberList with model changes
            Updated.InvokeAsync();
        }

        private void OnRowSelected(GridRowClickEventArgs args)
        {
            SerialNumberToEdit = args.Item as SerialNumberListItem;
        }
    }
}
