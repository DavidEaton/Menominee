using CSharpFunctionalExtensions;
using FluentValidation;
using Menominee.Client.Services.Payables.Vendors;
using Menominee.Client.Shared;
using Menominee.Domain.Enums;
using Menominee.Shared.Models.Payables.Invoices.Payments;
using Menominee.Shared.Models.Payables.Vendors;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Menominee.Client.Components.Settings
{
    public partial class VendorInvoicePaymentMethodEditor
    {
        [Inject]
        public IVendorDataService? VendorDataService { get; set; }
        [Inject]
        private IValidator<VendorInvoicePaymentMethodRequest>? RequestValidator { get; set; }

        [Inject]
        IJSRuntime? JsInterop { get; set; }

        [Parameter]
        public VendorInvoicePaymentMethodRequest? PaymentMethod { get; set; }

        [Parameter]
        public EventCallback OnSave { get; set; }

        [Parameter]
        public EventCallback OnCancel { get; set; }

        [Parameter]
        public FormMode Mode
        {
            get => formMode;
            set
            {
                formMode = value;
                Title = FormTitle.BuildTitle(formMode, "Payment Method");
            }
        }

        [Inject]
        ILogger<VendorInvoicePaymentMethodEditor> Logger { get; set; }

        private string? Title { get; set; }
        private FormMode formMode;
        private IReadOnlyList<VendorToRead>? Vendors = null;
        private long? vendorId = null;
        private bool parametersSet = false;
        private IList<VendorPaymentType> PaymentTypes { get; set; } = new List<VendorPaymentType>();

        protected override async Task OnInitializedAsync()
        {
            await GetVendorsAsync();

            foreach (VendorInvoicePaymentMethodType payType in Enum.GetValues(typeof(VendorInvoicePaymentMethodType)))
            {
                PaymentTypes.Add(new VendorPaymentType { Text = EnumExtensions.GetDisplayName(payType), Value = payType });
            }
        }

        private async Task GetVendorsAsync()
        {
            if (VendorDataService is not null)
            {
                await VendorDataService.GetAllAsync()
                .Match(
                    success => Vendors = success
                        .Where(vendor =>
                               vendor.IsActive == true
                            && vendor.VendorRole == VendorRole.PaymentReconciler)
                        .OrderBy(vendor => vendor.VendorCode)
                        .ToList(),

                    failure => Logger.LogError(failure));
            }
        }

        protected override async Task OnParametersSetAsync()
        {
            if (parametersSet)
                return;
            parametersSet = true;

            if (PaymentMethod?.ReconcilingVendor != null)
            {
                vendorId = PaymentMethod.ReconcilingVendor.Id;
            }

            await OnVendorChangeAsync();
        }

        private async Task OnVendorChangeAsync()
        {
            if (vendorId == null || vendorId == 0)
            {
                PaymentMethod.ReconcilingVendor = null;
            }
            else if (PaymentMethod?.ReconcilingVendor?.Id != vendorId)
            {
                var result = await VendorDataService.GetAsync(vendorId ?? 0);
                if (result.IsSuccess)
                {
                    PaymentMethod.ReconcilingVendor = result.Value;
                }
                if (result.IsFailure)
                {
                    Logger.LogError(result.Error);
                    PaymentMethod.ReconcilingVendor = new();
                }
            }
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);
            if (firstRender)
            {
                await Focus("description");
            }
        }

        public async Task Focus(string elementId)
        {
            await JsInterop.InvokeVoidAsync("jsfunction.focusElement", elementId);
        }


        public class VendorPaymentType
        {
            public string Text { get; set; }
            public VendorInvoicePaymentMethodType Value { get; set; }
        }
    }
}
