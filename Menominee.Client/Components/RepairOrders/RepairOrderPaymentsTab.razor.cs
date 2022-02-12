using CustomerVehicleManagement.Shared.Models.RepairOrders.Payments;
using Menominee.Common.Enums;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Linq;
using Telerik.Blazor.Components;

namespace Menominee.Client.Components.RepairOrders
{
    public partial class RepairOrderPaymentsTab : ComponentBase
    {
        [Parameter]
        public IList<RepairOrderPaymentToWrite> Payments { get; set; }

        public TelerikGrid<RepairOrderPaymentToWrite> PaymentsGrid { get; set; }

        private bool CanEdit { get; set; } = false;
        private bool CanDelete { get; set; } = false;
        //private bool CanAdd { get; set; } = false;

        public IEnumerable<RepairOrderPaymentToWrite> SelectedPayments { get; set; } = Enumerable.Empty<RepairOrderPaymentToWrite>();
        public RepairOrderPaymentToWrite SelectedPayment { get; set; }

        public long SelectedId
        {
            get => selectedId;
            set
            {
                selectedId = value;
                CanEdit = selectedId != 0;
                CanDelete = selectedId != 0 && Payments.Count > 0;
                //CanAdd = true;
            }
        }

        private long itemToSelect { get; set; } = 0;
        private long selectedId = 0;
        private int selectedItemIndex = 0;
        private long nextId = 0;
        private bool editDialogVisible = false;
        private bool shouldRender = true;
        private FormMode PaymentFormMode = FormMode.Unknown;

        protected override void OnInitialized()
        {
            if (Payments.Count > 0)
            {
                //if (itemToSelect == 0)
                //{
                //    SelectedPayment = Payments.FirstOrDefault();
                //}
                //else
                //{
                //    SelectedPayment = Payments.Where(x => x.Id == itemToSelect).FirstOrDefault();
                //}
                //selectedItemIndex = Payments.IndexOf(SelectedPayment);
                //SelectedId = SelectedPayment.Id;
                //SelectedPayments = new List<RepairOrderPaymentToWrite> { SelectedPayment };
            }
        }

        protected override bool ShouldRender()
        {
            return shouldRender;
        }

        protected void OnSelect(IEnumerable<RepairOrderPaymentToWrite> payment)
        {
            //SelectedItem = ros.FirstOrDefault();
            //SelectedList = new List<RepairOrderToReadInList> { SelectedItem };
        }

        private void OnRowSelected(GridRowClickEventArgs args)
        {
            //SelectedPayment = args.Item as RepairOrderPaymentToWrite;
            //SelectedId = SelectedPayment.Id;
            //selectedItemIndex = Payments.IndexOf(SelectedPayment);
            //SelectedPayments = new List<RepairOrderPaymentToWrite> { SelectedPayment };
        }

        private bool EditDialogVisible
        {
            get => editDialogVisible;
            set
            {
                if (value == true)
                {
                    if (PaymentFormMode == FormMode.Add)
                    {
                        //PaymentToModify = new RepairOrderPaymentToWrite()
                        //{
                        //    Id = --nextId
                        //};
                    }
                    if (PaymentFormMode == FormMode.Edit || PaymentFormMode == FormMode.View)
                    {
                        //PaymentToModify = new RepairOrderPaymentToWrite();
                        //CopyPayment(SelectedPayment, PaymentToModify);
                    }
                }
                else
                {
                    //if (PaymentToModify != null)
                    //    PaymentToModify = null;
                    PaymentFormMode = FormMode.Unknown;
                }

                editDialogVisible = value;
            }
        }

        //private void OnEdit()
        void OnRowDoubleClickHandler(GridRowClickEventArgs args)
        {
            //PaymentToModify = args.Item as RepairOrderPaymentToWrite;
            SelectedPayment = args.Item as RepairOrderPaymentToWrite;
            PaymentFormMode = FormMode.Edit;
            EditDialogVisible = true;
        }

        private void OnAdd()
        {
            SelectedPayment = new();

            PaymentFormMode = FormMode.Add;
            EditDialogVisible = true;
        }

        private void OnDelete()
        {
        }

        private void OnSaveEdit()
        {
            if (PaymentFormMode != FormMode.Add && PaymentFormMode != FormMode.Edit)
                return;

            if (PaymentFormMode == FormMode.Add)
            {
                Payments.Add(SelectedPayment);
                //selectedItemIndex = Payments.IndexOf(PaymentToModify);
                //SelectedPayment = Payments[selectedItemIndex];
                //SelectedPayments = new List<RepairOrderPaymentToWrite> { SelectedPayment };
            }
            else if (PaymentFormMode == FormMode.Edit)
            {
                //CopyPayment(PaymentToModify, Payments[selectedItemIndex]);
            }
            //SelectedId = SelectedPayment.Id;
            EditDialogVisible = false;
            shouldRender = true;
            StateHasChanged();
            PaymentsGrid?.Rebind();
        }

        private void OnCancelEdit()
        {
            PaymentFormMode = FormMode.Unknown;
            EditDialogVisible = false;
        }
    }
}
