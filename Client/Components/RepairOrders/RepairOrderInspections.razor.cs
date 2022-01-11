using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telerik.Blazor.Components;

namespace MenomineePlayWASM.Client.Components.RepairOrders
{
    public partial class RepairOrderInspections
    {
        [Parameter]
        public bool IsCurrent { get; set; }

        [Parameter]
        public List<Inspection> Inspections { get; set; }

        private bool CanEdit { get; set; } = false;
        private bool CanDelete { get; set; } = false;
        private bool CanPrint { get; set; } = false;

        // FIX ME - replace Inspection with DTO
        public IEnumerable<Inspection> SelectedInspections { get; set; } = Enumerable.Empty<Inspection>();
        public Inspection SelectedInspection { get; set; }
        public Inspection InspectionToModify { get; set; } = null;

        public long SelectedId
        {
            get => selectedId;
            set
            {
                selectedId = value;
                CanEdit = selectedId != 0;      // && completed
                CanDelete = selectedId != 0;    // && !started
                CanPrint = selectedId != 0;     // && completed
            }
        }

        private long itemToSelect { get; set; } = 0;
        private long selectedId = 0;
        //private long nextId = 0;
        private int selectedItemIndex = 0;

        protected override void OnInitialized()
        {
            if (Inspections.Count > 0)
            {
                if (itemToSelect == 0)
                {
                    SelectedInspection = Inspections.FirstOrDefault();
                }
                else
                {
                    SelectedInspection = Inspections.Where(x => x.Id == itemToSelect).FirstOrDefault();
                }
                selectedItemIndex = Inspections.IndexOf(SelectedInspection);
                SelectedId = SelectedInspection.Id;
                SelectedInspections = new List<Inspection> { SelectedInspection };
            }
        }

        protected void OnSelect(IEnumerable<Inspection> inspection)
        {
            //SelectedItem = ros.FirstOrDefault();
            //SelectedList = new List<RepairOrderToReadInList> { SelectedItem };
        }

        private void OnRowSelected(GridRowClickEventArgs args)
        {
            SelectedInspection = args.Item as Inspection;
            SelectedId = SelectedInspection.Id;
            selectedItemIndex = Inspections.IndexOf(SelectedInspection);
            SelectedInspections = new List<Inspection> { SelectedInspection };
        }

        private void OnEdit()
        {
            
        }


        private void OnAdd()
        {

        }

        private void OnDelete()
        {

        }

        private void OnPrint()
        {

        }

    }
}
