using CustomerVehicleManagement.Shared.Models.Payables.Invoices.Payments;
using CustomerVehicleManagement.Shared.Models.Payables.Vendors;
using Menominee.Client.Services.Payables.Vendors;
using Menominee.Client.Shared;
using Menominee.Common.Enums;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Syncfusion.Blazor.Navigations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Menominee.Client.Components.Payables.VendorInvoiceItemEditor;

namespace Menominee.Client.Components.Payables
{
    public partial class VendorInvoicePaymentMethodEditor
    {
        [Inject]
        public IVendorDataService vendorDataService { get; set; }

        [Inject]
        IJSRuntime JsInterop { get; set; }

        [Parameter]
        public VendorInvoicePaymentMethodToWrite PaymentMethod { get; set; }

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

        private string Title { get; set; }
        private FormMode formMode;
        private IReadOnlyList<VendorToRead> Vendors = null;
        private long? vendorId = null;
        private bool parametersSet = false;
        private IList<VendorPaymentType> PaymentTypes { get; set; } = new List<VendorPaymentType>();

        protected override async Task OnInitializedAsync()
        {
            Vendors = (await vendorDataService.GetAllVendorsAsync())
                                                          .Where(vendor => vendor.IsActive == true)
                                                          //&& vendor.VendorRole == VendorRole.PaymentReconciler)
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

            //if (PaymentMethod?.ReconcilingVendor != null)
            //{
            //    vendorId = PaymentMethod.ReconcilingVendor.Id;
            //}
            //vendorId = PaymentMethod?.VendorId;

            await OnVendorChangeAsync();
        }

        private async Task OnVendorChangeAsync()
        {
            if (vendorId == null || vendorId == 0)
            {
                PaymentMethod.ReconcilingVendor = null;
            }
            //else if (vendorId > 0 && PaymentMethod.ReconcilingVendor?.Id != vendorId)
            else if (PaymentMethod?.ReconcilingVendor.Id != vendorId)
            {
                PaymentMethod.ReconcilingVendor = await vendorDataService.GetVendorAsync(vendorId ?? 0);
            }
            //PaymentMethod.VendorId = vendorId;
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
