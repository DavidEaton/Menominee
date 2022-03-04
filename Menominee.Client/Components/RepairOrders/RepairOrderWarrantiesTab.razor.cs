using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Linq;
using Telerik.Blazor.Components;

namespace Menominee.Client.Components.RepairOrders
{
    public partial class RepairOrderWarrantiesTab : ComponentBase
    {
        [Parameter]
        public List<WarrantyListItem> Warranties { get; set; }

        private bool CanEdit { get; set; } = false;
        private bool CanCopy { get; set; } = false;
        private bool CanClear { get; set; } = false;

        // FIX ME - replace Warranty with DTO
        public IEnumerable<WarrantyListItem> SelectedWarranties { get; set; } = Enumerable.Empty<WarrantyListItem>();
        public WarrantyListItem SelectedWarranty { get; set; }
        public WarrantyListItem WarrantyToModify { get; set; } = null;

        public long SelectedId
        {
            get => selectedId;
            set
            {
                selectedId = value;
                CanEdit = selectedId != 0;
                CanCopy = selectedId != 0 && Warranties.Count > 0;
                CanClear = selectedId != 0;
            }
        }

        private long itemToSelect { get; set; } = 0;
        private long selectedId = 0;
        private int selectedItemIndex = 0;

        protected override void OnInitialized()
        {
            if (Warranties.Count > 0)
            {
                if (itemToSelect == 0)
                {
                    SelectedWarranty = Warranties.FirstOrDefault();
                }
                else
                {
                    SelectedWarranty = Warranties.Where(x => x.Id == itemToSelect).FirstOrDefault();
                }
                selectedItemIndex = Warranties.IndexOf(SelectedWarranty);
                SelectedId = SelectedWarranty.Id;
                SelectedWarranties = new List<WarrantyListItem> { SelectedWarranty };
            }
        }

        protected void OnSelect(IEnumerable<WarrantyListItem> warranty)
        {
            //SelectedItem = ros.FirstOrDefault();
            //SelectedList = new List<RepairOrderToReadInList> { SelectedItem };
        }

        private void OnRowSelected(GridRowClickEventArgs args)
        {
            SelectedWarranty = args.Item as WarrantyListItem;
            SelectedId = SelectedWarranty.Id;
            selectedItemIndex = Warranties.IndexOf(SelectedWarranty);
            SelectedWarranties = new List<WarrantyListItem> { SelectedWarranty };
        }

        private void OnEdit()
        {

        }

        private void OnCopy()
        {

        }

        private void OnClear()
        {

        }
    }
}
