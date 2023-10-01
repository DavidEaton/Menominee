using Menominee.Client.Services.Inventory;
using Menominee.Shared.Models.Inventory.InventoryItems;
using Microsoft.AspNetCore.Components;

namespace Menominee.Client.Components.Inventory.Pages
{
    public partial class InventoryGiftCertificateEditPage : ComponentBase
    {
        [Inject]
        private NavigationManager navigationManager { get; set; }

        [Inject]
        public IInventoryItemDataService DataService { get; set; }

        [Parameter]
        public long ItemId { get; set; }

        private InventoryItemToWrite Item;

        protected override async Task OnInitializedAsync()
        {
            if (ItemId == 0)
                Item = new();
            else
            {
                var result = await DataService.GetAsync(ItemId);
                if (result.IsSuccess)
                    Item = InventoryItemHelper.ConvertReadToWriteDto(result.Value);

                if (result.IsFailure)
                {
                    ItemId = default;
                    Item = new();
                }
            }
        }

        private async Task Save()
        {
            if (ItemId == 0)
            {
                var item = await DataService.AddAsync(Item);
                ItemId = Item.Id;
            }
            else
            {
                await DataService.UpdateAsync(Item);
            }

            EndEdit();
        }

        private void Discard()
        {
            EndEdit();
        }

        protected void EndEdit()
        {
            navigationManager.NavigateTo($"/inventory/items/listing/{ItemId}");
        }
    }
}
