using Menominee.Domain.Enums;
using Microsoft.AspNetCore.Components;

namespace Menominee.Client.Components.Inventory
{
    public partial class InventoryItemTypeSelectDialog
    {
        [Parameter]
        public bool DialogVisible { get; set; }

        [Parameter]
        public InventoryItemType SelectedItemType { get; set; }

        [Parameter]
        public EventCallback OnSelect { get; set; }

        [Parameter]
        public EventCallback OnCancel { get; set; }

        [Parameter]
        public EventCallback<InventoryItemType> SelectedItemTypeChanged { get; set; }

        protected override void OnInitialized()
        {
            foreach (InventoryItemType item in Enum.GetValues(typeof(InventoryItemType)))
            {
                itemTypeList.Add(new ListItem { Text = EnumExtensions.GetDisplayName(item), Value = item });
            }

            base.OnInitialized();
        }

        protected override void OnParametersSet()
        {
            SelectedItem = SelectedItemType;
        }

        protected async Task SelectType()
        {
            SelectedItemType = SelectedItem;
            await SelectedItemTypeChanged.InvokeAsync(SelectedItemType);
            await OnSelect.InvokeAsync();
        }

        public InventoryItemType SelectedItem { get; set; }
        private List<ListItem> itemTypeList = new();
        public class ListItem
        {
            public string Text { get; set; }
            public InventoryItemType Value { get; set; }
        }
    }
}
