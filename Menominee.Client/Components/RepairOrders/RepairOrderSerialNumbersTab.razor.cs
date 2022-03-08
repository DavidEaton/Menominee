using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using Telerik.Blazor.Components;

namespace Menominee.Client.Components.RepairOrders
{
    public partial class RepairOrderSerialNumbersTab : ComponentBase
    {
        [CascadingParameter]
        public List<SerialNumberListItem> SerialNumberList { get; set; }

        [Parameter]
        public EventCallback Updated { get; set; }
        private bool CanEdit { get; set; } = false;
        private bool CanCopy { get; set; } = false;
        private bool CanClear { get; set; } = false;
        private bool EditDialogVisible { get; set; } = false;

        // FIX ME - resolve Id==0 issue with new records with detail records
        public IEnumerable<SerialNumberListItem> SelectedSerialNumbers { get; set; } = Enumerable.Empty<SerialNumberListItem>();
        public SerialNumberListItem SelectedSerialNumber { get; set; }
        public SerialNumberListItem SerialNumberToModify { get; set; } = null;

        public long SelectedId
        {
            get => selectedId;
            set
            {
                selectedId = value;
                CanEdit = selectedId != 0;
                CanCopy = selectedId != 0 && SerialNumberList.Count > 0;
                CanClear = selectedId != 0;
            }
        }

        private long itemToSelect { get; set; } = 0;
        private long selectedId = 0;
        //private int selectedItemIndex = 0;

        protected override void OnInitialized()
        {
            if (SerialNumberList.Count > 0)
            {
                if (itemToSelect == 0)
                {
                    SelectedSerialNumber = SerialNumberList.FirstOrDefault();
                }
                else
                {
                    SelectedSerialNumber = SerialNumberList.Where(x => x.Id == itemToSelect).FirstOrDefault();
                }
                //selectedItemIndex = SerialNumbers.IndexOf(SelectedSerialNumber);
                SelectedId = SelectedSerialNumber.Id;
                SelectedSerialNumbers = new List<SerialNumberListItem> { SelectedSerialNumber };
            }
        }

        public void Save()
        {
            EditDialogVisible = false;
            // Update SerialNumberList with model changes
            Updated.InvokeAsync();
        }

        protected void OnSelect(IEnumerable<SerialNumberListItem> serialNumber)
        {
            //SelectedItem = ros.FirstOrDefault();
            //SelectedList = new List<RepairOrderToReadInList> { SelectedItem };
        }

        private void OnRowSelected(GridRowClickEventArgs args)
        {
            SelectedSerialNumber = args.Item as SerialNumberListItem;
        }

        private void OnCopy()
        {
        }

        private static void CopySerialNumber(SerialNumberListItem src, SerialNumberListItem dst)
        {
            dst.Id = src.Id;
            dst.ItemId = src.ItemId;
            dst.PartNumber = src.PartNumber;
            dst.Description = src.Description;
            dst.SerialNum = src.SerialNum;
        }
    }
}
