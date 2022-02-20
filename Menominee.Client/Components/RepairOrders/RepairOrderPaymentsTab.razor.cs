using CustomerVehicleManagement.Shared.Models.RepairOrders.Payments;
using Menominee.Common.Enums;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telerik.Blazor;
using Telerik.Blazor.Components;

namespace Menominee.Client.Components.RepairOrders
{
    public partial class RepairOrderPaymentsTab : ComponentBase
    {
        [Parameter]
        public IList<RepairOrderPaymentToWrite> Payments { get; set; }

        [CascadingParameter]
        public DialogFactory Dialogs { get; set; }

        public TelerikGrid<RepairOrderPaymentToWrite> PaymentsGrid { get; set; }

        private bool CanEdit { get; set; } = false;
        private bool CanDelete { get; set; } = false;
        //private bool CanAdd { get; set; } = false;

        public IEnumerable<RepairOrderPaymentToWrite> SelectedPayments { get; set; } = Enumerable.Empty<RepairOrderPaymentToWrite>();
        public RepairOrderPaymentToWrite SelectedPayment { get; set; }

        //public long SelectedId
        //{
        //    get => selectedId;
        //    set
        //    {
        //        selectedId = value;
        //        CanEdit = selectedId != 0;
        //        CanDelete = selectedId != 0 && Payments.Count > 0;
        //        //CanAdd = true;
        //    }
        //}

        //private long itemToSelect { get; set; } = 0;
        //private long selectedId = 0;
        //private int selectedItemIndex = 0;
        //private long nextId = 0;
        private bool editDialogVisible = false;
        //private bool shouldRender = true;
        private FormMode PaymentFormMode = FormMode.Unknown;

        protected override void OnInitialized()
        {
            if (Payments.Count > 0)
            {
                SelectPayment(Payments.FirstOrDefault());
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
            else
            {
                SelectedPayment = null;
            }
        }

        private void SelectPayment(RepairOrderPaymentToWrite payment)
        {
            SelectedPayment = payment;
            if (payment != null)
            {
                SelectedPayments = new List<RepairOrderPaymentToWrite> { payment };
            }
            else if (Payments.Count > 0)
            {
                SelectedPayment = Payments.FirstOrDefault();
                SelectedPayments = new List<RepairOrderPaymentToWrite> { SelectedPayment };
            }
            else
            {
                SelectedPayments = new List<RepairOrderPaymentToWrite>();
            }

            CanEdit = CanDelete = (SelectedPayment != null);     // will need other conditions checked before setting to true
        }

        protected void OnSelect(IEnumerable<RepairOrderPaymentToWrite> payment)
        {
            //SelectedItem = ros.FirstOrDefault();
            //SelectedList = new List<RepairOrderToReadInList> { SelectedItem };
        }

        private void OnRowSelected(GridRowClickEventArgs args)
        {
            SelectPayment(args.Item as RepairOrderPaymentToWrite);
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

        void OnRowDoubleClickHandler(GridRowClickEventArgs args)
        {
            //PaymentToModify = args.Item as RepairOrderPaymentToWrite;
            //SelectedPayment = args.Item as RepairOrderPaymentToWrite;
            SelectPayment(args.Item as RepairOrderPaymentToWrite);
            OnEdit();
        }

        private void OnEdit()
        {
            if (SelectedPayment != null)
            {
                PaymentFormMode = FormMode.Edit;
                EditDialogVisible = true;
            }
        }

        private void OnAdd()
        {
            SelectedPayment = new();

            PaymentFormMode = FormMode.Add;
            EditDialogVisible = true;
        }

        private async Task OnDelete()
        {
            //SelectedPayment = args.Item as RepairOrderPaymentToWrite;
            if (SelectedPayment != null && await ShowPaymentDeleteConfirm(SelectedPayment.PaymentMethod.ToString()))
            {
                Payments.Remove(SelectedPayment);
                StateHasChanged();
                PaymentsGrid?.Rebind();
            }
        }

        public async Task<bool> ShowPaymentDeleteConfirm(string payType)
        {
            return await Dialogs.ConfirmAsync($"Are you sure you want to delete the {payType} payment?", "Delete Payment");
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
                SelectPayment(SelectedPayment);
            }
            else if (PaymentFormMode == FormMode.Edit)
            {
                //CopyPayment(PaymentToModify, Payments[selectedItemIndex]);
            }
            //SelectedId = SelectedPayment.Id;
            EditDialogVisible = false;
            //shouldRender = true;
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
