﻿using CustomerVehicleManagement.Shared.Helpers.Inventory;
using CustomerVehicleManagement.Shared.Models.Inventory;
using Menominee.Client.Services.Inventory;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace Menominee.Client.Pages.Inventory
{
    public partial class InventoryLaborEdit : ComponentBase
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
                var readDto = await DataService.GetItemAsync(ItemId);
                if (readDto != null)
                {
                    Item = InventoryItemHelper.CreateWriteDtoFromReadDto(readDto);
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
                var item = await DataService.AddItemAsync(Item);
                ItemId = Item.Id;
            }
            else
            {
                await DataService.UpdateItemAsync(Item, ItemId);
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