using CustomerVehicleManagement.Shared.Models.Inventory;
using Menominee.Common.Enums;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telerik.Blazor.Components;

namespace Menominee.Client.Components.Inventory
{
    public partial class InventoryPackageEditor
    {
        [Parameter]
        public InventoryItemToWrite Item { get; set; }

        [Parameter]
        public string Title { get; set; } = "Edit Package";

        [Parameter]
        public EventCallback OnValidSubmit { get; set; }

        [Parameter]
        public EventCallback OnDiscard { get; set; }

        private bool parametersSet = false;
        private FormMode ItemFormMode = FormMode.Unknown;
        private FormMode PlaceholderFormMode = FormMode.Unknown;

        private long ItemId { get; set; } = 0;
        private long PlaceholderId { get; set; } = 0;

        private bool CanDeleteItem { get => ItemId > 0; }
        private bool CanDeletePlaceholder { get => PlaceholderId > 0; }

        private TelerikGrid<InventoryPackageItemToWrite> ItemsGrid { get; set; }
        private TelerikGrid<InventoryPackagePlaceholderToWrite> PlaceholdersGrid { get; set; }
        private IEnumerable<InventoryPackageItemToWrite> SelectedItems { get; set; } = Enumerable.Empty<InventoryPackageItemToWrite>();
        private InventoryPackageItemToWrite SelectedItem { get; set; }
        private IEnumerable<InventoryPackagePlaceholderToWrite> SelectedPlaceholders { get; set; } = Enumerable.Empty<InventoryPackagePlaceholderToWrite>();
        private InventoryPackagePlaceholderToWrite SelectedPlaceholder { get; set; }
        //private InventoryPackageItemToWrite ItemToAdd { get; set; }
        private InventoryPackagePlaceholderToWrite PlaceholderToAdd { get; set; }
        public InventoryItemToReadInList SelectedInventoryItem { get; set; }

        //protected override async Task OnInitializedAsync()
        //{
        //}

        protected override void OnParametersSet()
        {
            if (parametersSet)
                return;

            parametersSet = true;

            if (Item?.Package == null)
            {
                Item.Package = new();
                Item.ItemType = InventoryItemType.Package;

                Title = "Add Package";
            }
        }

        private void OnAddItem()
        {
            //ItemToAdd = new();
            ItemFormMode = FormMode.Add;
        }

        private void OnAddPlaceholder()
        {
            PlaceholderToAdd = new();
            PlaceholderFormMode = FormMode.Add;
        }

        private void OnDeleteItem()
        {
            //if (ItemId > 0)
            //    await DataService.Delete(ItemId);
        }

        private void OnDeletePlaceholder()
        {
            //if (PlaceholderId > 0)
            //    await DataService.Delete(PlaceholderId);
        }

        protected void OnItemSelect(IEnumerable<InventoryPackageItemToWrite> items)
        {
            SelectedItem = items.FirstOrDefault();
            SelectedItems = new List<InventoryPackageItemToWrite> { SelectedItem };
        }

        private void OnItemRowSelected(GridRowClickEventArgs args)
        {
            ItemId = (args.Item as InventoryPackageItemToWrite).Id;
        }

        protected void OnPlaceholderSelect(IEnumerable<InventoryPackagePlaceholderToWrite> placeholders)
        {
            SelectedPlaceholder = placeholders.FirstOrDefault();
            SelectedPlaceholders = new List<InventoryPackagePlaceholderToWrite> { SelectedPlaceholder };
        }

        private void OnPlaceholderRowSelected(GridRowClickEventArgs args)
        {
            PlaceholderId = (args.Item as InventoryPackagePlaceholderToWrite).Id;
        }

        protected void SubmitItemHandler()
        {
            //ItemId = (await CreditCardDataService.AddCreditCardAsync(CreditCard)).Id;
            InventoryPackageItemToWrite ItemToAdd = new();
            ItemToAdd.Item = new();
            ItemToAdd.Item.Id = SelectedInventoryItem.Id;
            ItemToAdd.Item.ManufacturerId = SelectedInventoryItem.ManufacturerId;
            ItemToAdd.Item.ItemNumber = SelectedInventoryItem.ItemNumber;
            ItemToAdd.Item.Description = SelectedInventoryItem.Description;
            ItemToAdd.Quantity = 1;
            ItemToAdd.PartAmountIsAdditional = false;
            ItemToAdd.LaborAmountIsAdditional = false;
            ItemToAdd.ExciseFeeIsAdditional = false;
            Item.Package.Items.Add(ItemToAdd);
            EndAddItem();
            ItemsGrid.Rebind();
        }

        protected void EndAddItem()
        {
            ItemFormMode = FormMode.Unknown;
            //CreditCards = (await CreditCardDataService.GetAllCreditCardsAsync()).ToList();
            //SelectedCreditCard = CreditCards.Where(x => x.Id == Id).FirstOrDefault();
            //SelectedCreditCards = new List<CreditCardToReadInList> { SelectedCreditCard };
            //ItemToAdd = null;
        }

        protected void SubmitPlaceholderHandler()
        {
            Item.Package.Placeholders.Add(PlaceholderToAdd);
            EndAddPlaceholder();
            PlaceholdersGrid.Rebind();
        }

        protected void EndAddPlaceholder()
        {
            PlaceholderFormMode = FormMode.Unknown;
            //CreditCards = (await CreditCardDataService.GetAllCreditCardsAsync()).ToList();
            //SelectedCreditCard = CreditCards.Where(x => x.Id == Id).FirstOrDefault();
            //SelectedCreditCards = new List<CreditCardToReadInList> { SelectedCreditCard };
            PlaceholderToAdd = null;
        }

    }
}
