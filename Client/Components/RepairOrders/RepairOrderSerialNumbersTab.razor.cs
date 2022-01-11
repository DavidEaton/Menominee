using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telerik.Blazor.Components;

namespace MenomineePlayWASM.Client.Components.RepairOrders
{
    public partial class RepairOrderSerialNumbersTab : ComponentBase
    {
        [Parameter]
        public List<SerialNumber> SerialNumbers { get; set; } = null;

        private bool CanEdit { get; set; } = false;
        private bool CanCopy { get; set; } = false;
        private bool CanClear { get; set; } = false;

        // FIX ME - replace SerialNumber with DTO
        public IEnumerable<SerialNumber> SelectedSerialNumbers { get; set; } = Enumerable.Empty<SerialNumber>();
        public SerialNumber SelectedSerialNumber { get; set; }
        public SerialNumber SerialNumberToModify { get; set; } = null;

        public long SelectedId
        {
            get => selectedId;
            set
            {
                selectedId = value;
                CanEdit = selectedId != 0;
                CanCopy = selectedId != 0 && SerialNumbers.Count > 0;
                CanClear = selectedId != 0;
            }
        }

        private long itemToSelect { get; set; } = 0;
        private long selectedId = 0;
        private int selectedItemIndex = 0;

        protected override void OnInitialized()
        {
            if (SerialNumbers.Count > 0)
            {
                if (itemToSelect == 0)
                {
                    SelectedSerialNumber = SerialNumbers.FirstOrDefault();
                }
                else
                {
                    SelectedSerialNumber = SerialNumbers.Where(x => x.Id == itemToSelect).FirstOrDefault();
                }
                selectedItemIndex = SerialNumbers.IndexOf(SelectedSerialNumber);
                SelectedId = SelectedSerialNumber.Id;
                SelectedSerialNumbers = new List<SerialNumber> { SelectedSerialNumber };
            }
        }

        protected void OnSelect(IEnumerable<SerialNumber> serialNumber)
        {
            //SelectedItem = ros.FirstOrDefault();
            //SelectedList = new List<RepairOrderToReadInList> { SelectedItem };
        }

        private void OnRowSelected(GridRowClickEventArgs args)
        {
            SelectedSerialNumber = args.Item as SerialNumber;
            SelectedId = SelectedSerialNumber.Id;
            selectedItemIndex = SerialNumbers.IndexOf(SelectedSerialNumber);
            SelectedSerialNumbers = new List<SerialNumber> { SelectedSerialNumber };
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
