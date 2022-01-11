using MenomineePlayWASM.Shared.Dtos.Inventory;
using Microsoft.AspNetCore.Components;

namespace MenomineePlayWASM.Client.Components.Inventory
{
    public partial class InventoryItemForm
    {
        [Parameter]
        public InventoryItemToWrite Item { get; set; }

        [Parameter]
        public string Title { get; set; } = "Edit Item";

        [Parameter]
        public EventCallback OnValidSubmit { get; set; }

        [Parameter]
        public EventCallback OnDiscard { get; set; }

    }
}
