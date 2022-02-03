using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace Menominee.Client.Pages.Inventory
{
    public partial class InventoryItemEdit : ComponentBase
    {
        [Inject]
        private NavigationManager navigationManager { get; set; }

        // FIX ME - Add code back in when the DataService & Dtos are implemented
        //[Inject]
        //public IInventoryItemDataService DataService { get; set; }

        [Parameter]
        public long ItemId { get; set; }

        //private InventoryItemToWrite Item;

        //protected override async Task OnInitializedAsync()
        //{
        //    if (ItemId == 0)
        //    {
        //        Item = new();
        //    }
        //    else
        //    {
        //        var readDto = await DataService.GetItem(ItemId);
        //        Item = new InventoryItemToWrite()
        //        {
        //            Id = readDto.Id,
        //            PartNumber = readDto.PartNumber,
        //            Description = readDto.Description,
        //            //PartType = readDto.PartType.ToString(),
        //            Retail = readDto.Retail,
        //            Cost = readDto.Cost,
        //            Core = readDto.Core,
        //            Labor = readDto.Labor,
        //            OnHand = readDto.OnHand
        //        };
        //    }
        //}

        //private async Task Save()
        //{
        //    if (ItemId == 0)
        //    {
        //        var item = await DataService.AddItem(Item);
        //        ItemId = Item.Id;
        //    }
        //    else
        //    {
        //        await DataService.UpdateItem(Item, ItemId);
        //    }

        //    EndEdit();
        //}

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
