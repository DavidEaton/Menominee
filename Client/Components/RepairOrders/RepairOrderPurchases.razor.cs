using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telerik.Blazor.Components;

namespace MenomineePlayWASM.Client.Components.RepairOrders
{
    public partial class RepairOrderPurchases : ComponentBase
    {
        [Parameter]
        public List<Purchase> Purchases { get; set; }

        private bool CanEdit { get; set; } = false;
        private bool CanCopy { get; set; } = false;
        private bool CanClear { get; set; } = false;

        // FIX ME - replace Purchase with DTO
        public IEnumerable<Purchase> SelectedPurchases { get; set; } = Enumerable.Empty<Purchase>();
        public Purchase SelectedPurchase { get; set; }
        public Purchase PurchaseToModify { get; set; } = null;

        public long SelectedId
        {
            get => selectedId;
            set
            {
                selectedId = value;
                CanEdit = selectedId != 0;
                CanCopy = selectedId != 0 && Purchases.Count > 0;
                CanClear = selectedId != 0;     
            }
        }

        private long itemToSelect { get; set; } = 0;
        private long selectedId = 0;
        private int selectedItemIndex = 0;

        protected override void OnInitialized()
        {
            if (Purchases.Count > 0)
            {
                if (itemToSelect == 0)
                {
                    SelectedPurchase = Purchases.FirstOrDefault();
                }
                else
                {
                    SelectedPurchase = Purchases.Where(x => x.Id == itemToSelect).FirstOrDefault();
                }
                selectedItemIndex = Purchases.IndexOf(SelectedPurchase);
                SelectedId = SelectedPurchase.Id;
                SelectedPurchases = new List<Purchase> { SelectedPurchase };
            }
        }

        protected void OnSelect(IEnumerable<Purchase> purchase)
        {
            //SelectedItem = ros.FirstOrDefault();
            //SelectedList = new List<RepairOrderToReadInList> { SelectedItem };
        }

        private void OnRowSelected(GridRowClickEventArgs args)
        {
            SelectedPurchase = args.Item as Purchase;
            SelectedId = SelectedPurchase.Id;
            selectedItemIndex = Purchases.IndexOf(SelectedPurchase);
            SelectedPurchases = new List<Purchase> { SelectedPurchase };
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
