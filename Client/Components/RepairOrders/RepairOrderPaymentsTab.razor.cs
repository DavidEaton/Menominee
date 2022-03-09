using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telerik.Blazor.Components;

namespace MenomineePlayWASM.Client.Components.RepairOrders
{
    public partial class RepairOrderPaymentsTab : ComponentBase
    {
        [Parameter]
        public List<Payment> Payments { get; set; }

        private bool CanEdit { get; set; } = false;
        private bool CanDelete { get; set; } = false;
        private bool CanAdd { get; set; } = false;

        // FIX ME - replace Payment with DTO
        public IEnumerable<Payment> SelectedPayments { get; set; } = Enumerable.Empty<Payment>();
        public Payment SelectedPayment { get; set; }
        public Payment PaymentToModify { get; set; } = null;

        public long SelectedId
        {
            get => selectedId;
            set
            {
                selectedId = value;
                CanEdit = selectedId != 0;
                CanDelete = selectedId != 0 && Payments.Count > 0;
                CanAdd = true;
            }
        }

        private long itemToSelect { get; set; } = 0;
        private long selectedId = 0;
        private int selectedItemIndex = 0;

        protected override void OnInitializedAsync()
        {
            if (Payments.Count > 0)
            {
                if (itemToSelect == 0)
                {
                    SelectedPayment = Payments.FirstOrDefault();
                }
                else
                {
                    SelectedPayment = Payments.Where(x => x.Id == itemToSelect).FirstOrDefault();
                }
                selectedItemIndex = Payments.IndexOf(SelectedPayment);
                SelectedId = SelectedPayment.Id;
                SelectedPayments = new List<Payment> { SelectedPayment };
            }
        }

        protected void OnSelect(IEnumerable<Payment> payment)
        {
            //SelectedItem = ros.FirstOrDefault();
            //SelectedList = new List<RepairOrderToReadInList> { SelectedItem };
        }

        private void OnRowSelected(GridRowClickEventArgs args)
        {
            SelectedPayment = args.Item as Payment;
            SelectedId = SelectedPayment.Id;
            selectedItemIndex = Payments.IndexOf(SelectedPayment);
            SelectedPayments = new List<Payment> { SelectedPayment };
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
    }
}
