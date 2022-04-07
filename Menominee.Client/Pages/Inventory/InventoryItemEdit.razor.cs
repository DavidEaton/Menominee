using CustomerVehicleManagement.Shared.Models.Inventory;
using Menominee.Client.Services.Inventory;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace Menominee.Client.Pages.Inventory
{
    public partial class InventoryItemEdit : ComponentBase
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
            {
                Item = new();
            }
            else
            {
                var readDto = await DataService.GetItem(ItemId);
                if (readDto != null)
                {
                    Item = new InventoryItemToWrite()
                    {
                        Id = readDto.Id,
                        //Manufacturer = readDto.Manufacturer,
                        ManufacturerId = readDto.ManufacturerId,
                        ItemNumber = readDto.ItemNumber,
                        Description = readDto.Description,
                        //ProductCode = readDto.ProductCode,
                        ProductCodeId = readDto.ProductCodeId,
                        ItemType = readDto.ItemType,
                        DetailId = readDto.DetailId
                        //SuggestedPrice = readDto.SuggestedPrice,
                        //Cost = readDto.Cost,
                        //Labor = readDto.Labor,
                        //QuantityOnHand = readDto.QuantityOnHand
                    };
                }
                else
                {
                    // FIX ME - what's the best way to handle this?
                    ItemId = 0;
                    Item = new();
                }
            }
        }

        private async Task Save()
        {
            if (ItemId == 0)
            {
                var item = await DataService.AddItem(Item);
                ItemId = Item.Id;
            }
            else
            {
                await DataService.UpdateItem(Item, ItemId);
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
