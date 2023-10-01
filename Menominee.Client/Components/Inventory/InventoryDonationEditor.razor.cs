using Menominee.Common.Enums;
using Microsoft.AspNetCore.Components;

namespace Menominee.Client.Components.Inventory
{
    public partial class InventoryDonationEditor : InventoryEditorBase
    {
        [Inject]
        public ILogger<InventoryDonationEditor> Logger { get; set; } = null!;

        [Parameter]
        public EventCallback OnValidSubmit { get; set; }

        [Parameter]
        public EventCallback OnDiscard { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await LoadProductCodesByManufacturer();
        }

        protected override async Task OnParametersSetAsync()
        {
            await OnParametersSetCommonAsync(InventoryItemType.Donation, "Add Donation", "Edit Donation");

            if (Item.Id == 0)//if (Item.Donation == null)
            {
                ResetItemProductCode();
                await LoadItemManufacturerByMiscellaneousStaticManufacturerCode();
                //Item.Donation = new();
            }
        }
    }
}

