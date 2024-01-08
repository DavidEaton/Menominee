using CSharpFunctionalExtensions;
using Menominee.Client.Services.Payables.PaymentMethods;
using Menominee.Domain.Enums;
using Menominee.Shared.Models.Payables.Invoices.Payments;
using Microsoft.AspNetCore.Components;
using Telerik.Blazor;
using Telerik.Blazor.Components;

namespace Menominee.Client.Components.Settings.Pages
{
    public partial class VendorPaymentMethodsPage : ComponentBase
    {
        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Inject]
        public IVendorInvoicePaymentMethodDataService PaymentMethodDataService { get; set; }

        [Parameter]
        public long PayMethodToSelect { get; set; } = 0;

        [CascadingParameter]
        public DialogFactory Dialogs { get; set; }

        [Inject]
        ILogger<VendorPaymentMethodsPage> Logger { get; set; }

        public IReadOnlyList<VendorInvoicePaymentMethodToReadInList> PayMethods = new List<VendorInvoicePaymentMethodToReadInList>();
        public IEnumerable<VendorInvoicePaymentMethodToReadInList> SelectedList { get; set; } = Enumerable.Empty<VendorInvoicePaymentMethodToReadInList>();
        public VendorInvoicePaymentMethodToReadInList SelectedItem { get; set; }

        private bool ShowInactive { get; set; } = false;

        public TelerikGrid<VendorInvoicePaymentMethodToReadInList> Grid { get; set; }

        private bool CanEdit { get; set; } = false;
        private bool CanDelete { get; set; } = false;

        private FormMode PayMethodFormMode = FormMode.Unknown;
        public VendorInvoicePaymentMethodRequest PayMethod { get; set; } = null;
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
            await LoadPaymentMethodsAsync();
        }

        private async Task LoadPaymentMethodsAsync()
        {
            await PaymentMethodDataService.GetAllAsync()
            .Match(
                success => PayMethods = success,
                failure => Logger.LogError(failure));

            if (PayMethods?.Count > 0)
            {
                if (PayMethodToSelect > 0)
                    SelectedItem = PayMethods.Where(x => x.Id == PayMethodToSelect).FirstOrDefault();

                if (PayMethodToSelect == 0 || SelectedItem == null)
                    SelectedItem = PayMethods.FirstOrDefault();

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
                var result = await PaymentMethodDataService.GetAsync(SelectedId);

                if (result.IsFailure)
                {
                    Logger.LogError(result.Error);
                    return;
                }

                PayMethod = new VendorInvoicePaymentMethodRequest
                {
                    Name = result.Value.Name,
                    IsActive = result.Value.IsActive,
                    PaymentType = result.Value.PaymentType,
                    ReconcilingVendor = result.Value.ReconcilingVendor
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
                await PaymentMethodDataService.DeleteAsync(SelectedId);
            }

            await LoadPaymentMethodsAsync();
            Grid.Rebind();
        }

        protected async Task HandleAddSubmitAsync()
        {
            SelectedId = (await PaymentMethodDataService.AddAsync(PayMethod)).Value.Id;
            await EndAddEditAsync();
            Grid.Rebind();
        }

        protected async Task HandleEditSubmitAsync()
        {
            await PaymentMethodDataService.UpdateAsync(PayMethod);
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

            await PaymentMethodDataService.GetAllAsync()
            .Match(
                success => PayMethods = success,
                failure => Logger.LogError(failure));

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
