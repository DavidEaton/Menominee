using Menominee.Common.Enums;
using Microsoft.AspNetCore.Components;

namespace Menominee.Client.Components.Inventory
{
    public partial class InventoryGiftCertificateEditor : InventoryEditorBase
    {

        [Parameter]
        public EventCallback OnValidSubmit { get; set; }

        [Parameter]
        public EventCallback OnDiscard { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await LoadSaleCodesByManufacturerAsync();
        }

        protected override async Task OnParametersSetAsync()
        {
            await OnParametersSetCommonAsync(InventoryItemType.GiftCertificate, "Add Gift Certificate", "Edit Gift Certificate");

            if (Item.Id == 0)//if (Item.GiftCertificate == null)
            {
                ResetItemProductCode();
                await LoadItemManufacturerByMiscellaneousStaticManufacturerCode();
                //Item.GiftCertificate = new();
            }
        }
    }
}
