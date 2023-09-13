using Menominee.Shared.Models.Payables.Invoices.Payments;
using Menominee.Shared.Models.Payables.Vendors;
using Menominee.Client.Services.Payables.Vendors;
using Menominee.Client.Shared;
using Menominee.Common.Enums;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Menominee.Client.Components.Settings
{
    public partial class VendorInvoicePaymentMethodEditor
    {
        [Inject]
        public IVendorDataService? VendorDataService { get; set; }

        [Inject]
        IJSRuntime? JsInterop { get; set; }

        [Parameter]
        public VendorInvoicePaymentMethodToWrite? PaymentMethod { get; set; }

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

        private string? Title { get; set; }
        private FormMode formMode;
        private IReadOnlyList<VendorToRead>? Vendors = null;
        private long? vendorId = null;
        private bool parametersSet = false;
        private IList<VendorPaymentType> PaymentTypes { get; set; } = new List<VendorPaymentType>();

        protected override async Task OnInitializedAsync()
        {
            Vendors = (await VendorDataService.GetAllVendorsAsync())
                                                          .Where(vendor => vendor.IsActive == true 
                                                                        && vendor.VendorRole == VendorRole.PaymentReconciler)
                                                          .OrderBy(vendor => vendor.VendorCode)
                                                          .ToList();

            foreach (VendorInvoicePaymentMethodType payType in Enum.GetValues(typeof(VendorInvoicePaymentMethodType)))
            {
                PaymentTypes.Add(new VendorPaymentType { Text = EnumExtensions.GetDisplayName(payType), Value = payType });
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
                PaymentMethod!.ReconcilingVendor = await VendorDataService.GetVendorAsync(vendorId ?? 0);
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
