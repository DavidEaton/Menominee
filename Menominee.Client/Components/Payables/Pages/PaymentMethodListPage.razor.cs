using CustomerVehicleManagement.Domain.Entities.Payables;
using CustomerVehicleManagement.Shared.Models.Payables.Invoices.Payments;
using CustomerVehicleManagement.Shared.Models.Payables.Vendors;
using Menominee.Client.Services.Payables.PaymentMethods;
using Menominee.Common.Enums;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telerik.Blazor;
using Telerik.Blazor.Components;

namespace Menominee.Client.Components.Payables.Pages
{
    public partial class PaymentMethodListPage : ComponentBase
    {
        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Inject]
        public IVendorInvoicePaymentMethodDataService PaymentMethodDataService { get; set; }

        [Parameter]
        public long PayMethodToSelect { get; set; } = 0;

        [CascadingParameter]
        public DialogFactory Dialogs { get; set; }

        public IReadOnlyList<VendorInvoicePaymentMethodToReadInList> PayMethods = null;
        public IEnumerable<VendorInvoicePaymentMethodToReadInList> SelectedList { get; set; } = Enumerable.Empty<VendorInvoicePaymentMethodToReadInList>();
        public VendorInvoicePaymentMethodToReadInList SelectedItem { get; set; }

        private bool showInactive { get; set; } = false;

        public TelerikGrid<VendorInvoicePaymentMethodToReadInList> Grid { get; set; }

        private bool CanEdit { get; set; } = false;
        private bool CanDelete { get; set; } = false;

        private FormMode PayMethodFormMode = FormMode.Unknown;
        public VendorInvoicePaymentMethodToWrite PayMethod { get; set; } = null;
        private long selectedId;
        public long SelectedId
        {
            get => selectedId;
            set
            {
                selectedId = value;
                CanEdit = selectedId > 0;
                CanDelete = selectedId > 0;
            }
        }

        protected override async Task OnInitializedAsync()
        {
            PayMethods = (await PaymentMethodDataService.GetAllPaymentMethodsAsync()).ToList();

            if (PayMethods?.Count > 0)
            {
                if (PayMethodToSelect == 0)
                {
                    SelectedItem = PayMethods.FirstOrDefault();
                }
                else
                {
                    SelectedItem = PayMethods.Where(x => x.Id == PayMethodToSelect).FirstOrDefault();
                }
                SelectedId = SelectedItem.Id;
                SelectedList = new List<VendorInvoicePaymentMethodToReadInList> { SelectedItem };
            }
        }

        private void OnAdd()
        {
            PayMethodFormMode = FormMode.Add;
            PayMethod = new();
            PayMethod.IsActive = true;
        }

        private async Task OnEditAsync()
        {
            if (SelectedId > 0)
            {
                var readDto = await PaymentMethodDataService.GetPaymentMethodAsync(SelectedId);
                PayMethod = new VendorInvoicePaymentMethodToWrite
                {
                    Name = readDto.Name,
                    IsActive= readDto.IsActive,
                    //IsOnAccountPaymentType = readDto.IsOnAccountPaymentType,
                    PaymentType = readDto.PaymentType,
                    ReconcilingVendor = readDto.ReconcilingVendor
                };

                PayMethodFormMode = FormMode.Edit;
            }
        }

        private async Task OnDeleteAsync()
        {
            // TODO: Check to see if this has been used.  If so, mark as inactive, otherwise delete it
            if (SelectedItem != null
            && await Dialogs.ConfirmAsync($"Are you sure you want to delete the \"{SelectedItem.Name}\" payment method?", "Remove Payment Method?"))
            {
                await PaymentMethodDataService.DeletePaymentMethodAsync(SelectedId);
            }

            SelectedItem = PayMethods.FirstOrDefault();
            if (SelectedItem != null)
            {
                SelectedId = SelectedItem.Id;
                SelectedList = new List<VendorInvoicePaymentMethodToReadInList> { SelectedItem };
            }
            else
            {
                SelectedId = 0;
                SelectedList = Enumerable.Empty<VendorInvoicePaymentMethodToReadInList>();
            }
            Grid.Rebind();
        }

        protected async Task HandleAddSubmitAsync()
        {
            SelectedId = (await PaymentMethodDataService.AddPaymentMethodAsync(PayMethod)).Id;
            await EndAddEditAsync();
            Grid.Rebind();
        }

        protected async Task HandleEditSubmitAsync()
        {
            await PaymentMethodDataService.UpdatePaymentMethodAsync(PayMethod, SelectedId);
            await EndAddEditAsync();
        }

        protected async Task SubmitHandlerAsync()
        {
            if (PayMethodFormMode == FormMode.Add)
                await HandleAddSubmitAsync();
            else if (PayMethodFormMode == FormMode.Edit)
                await HandleEditSubmitAsync();
        }

        protected async Task EndAddEditAsync()
        {
            PayMethodFormMode = FormMode.Unknown;
            PayMethods = (await PaymentMethodDataService.GetAllPaymentMethodsAsync()).ToList();
            SelectedItem = PayMethods.Where(x => x.Id == SelectedId).FirstOrDefault();
            SelectedList = new List<VendorInvoicePaymentMethodToReadInList> { SelectedItem };
        }

        private void OnDone()
        {
            NavigationManager.NavigateTo("payables");
        }

        protected void OnSelect(IEnumerable<VendorInvoicePaymentMethodToReadInList> payMethods)
        {
            SelectedItem = payMethods.FirstOrDefault();
            SelectedList = new List<VendorInvoicePaymentMethodToReadInList> { SelectedItem };
        }

        private void OnRowSelected(GridRowClickEventArgs args)
        {
            SelectedId = (args.Item as VendorInvoicePaymentMethodToReadInList).Id;
        }

        private async Task RowDoubleClickAsync(GridRowClickEventArgs args)
        {
            SelectedId = (args.Item as VendorInvoicePaymentMethodToReadInList).Id;
            await OnEditAsync();
        }

    }
}
