using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telerik.Blazor.Components;

namespace MenomineePlayWASM.Client.Components.RepairOrders
{
    public partial class RepairOrderWarrantiesTab : ComponentBase
    {
        [Parameter]
        public List<Warranty> Warranties { get; set; }

        private bool CanEdit { get; set; } = false;
        private bool CanCopy { get; set; } = false;
        private bool CanClear { get; set; } = false;

        // FIX ME - replace Warranty with DTO
        public IEnumerable<Warranty> SelectedWarranties { get; set; } = Enumerable.Empty<Warranty>();
        public Warranty SelectedWarranty { get; set; }
        public Warranty WarrantyToModify { get; set; } = null;

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

        protected override void OnInitializedAsync()
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
                SelectedWarranties = new List<Warranty> { SelectedWarranty };
            }
        }

        protected void OnSelect(IEnumerable<Warranty> warranty)
        {
            //SelectedItem = ros.FirstOrDefault();
            //SelectedList = new List<RepairOrderToReadInList> { SelectedItem };
        }

        private void OnRowSelected(GridRowClickEventArgs args)
        {
            SelectedWarranty = args.Item as Warranty;
            SelectedId = SelectedWarranty.Id;
            selectedItemIndex = Warranties.IndexOf(SelectedWarranty);
            SelectedWarranties = new List<Warranty> { SelectedWarranty };
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
