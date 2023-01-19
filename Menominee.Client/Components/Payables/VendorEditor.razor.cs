using CustomerVehicleManagement.Shared.Models.Payables.Invoices.Payments;
using CustomerVehicleManagement.Shared.Models.Payables.Vendors;
using Menominee.Client.Services.Payables.PaymentMethods;
using Menominee.Client.Shared;
using Menominee.Client.Shared.Models;
using Menominee.Common.Enums;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace Menominee.Client.Components.Payables
{
    public partial class VendorEditor : ComponentBase
    {
        [Inject]
        public IVendorInvoicePaymentMethodDataService PaymentMethodDataService { get; set; }

        [Parameter]
        public VendorToWrite Vendor { get; set; }

        [Parameter]
        public EventCallback OnValidSubmit { get; set; }

        [Parameter]
        public EventCallback OnDiscard { get; set; }

        [Parameter]
        public FormMode FormMode { get; set; }

        private string Title { get; set; }
        private IList<VendorTypeEnumModel> VendorTypeEnumData { get; set; } = new List<VendorTypeEnumModel>();
        private ObservableCollection<PaymentTypeModel> PaymentMethodList { get; set; } = new ObservableCollection<PaymentTypeModel>();
        protected long paymentMethodId { get; set; } = 0;
        protected bool autoComplete { get; set; } = false;
        private bool parametersSet = false;

        protected override async Task OnInitializedAsync()
        {
            foreach (VendorRole type in Enum.GetValues(typeof(VendorRole)))
                VendorTypeEnumData.Add(new VendorTypeEnumModel { DisplayText = EnumExtensions.GetDisplayName(type), Value = type });

            IReadOnlyList<VendorInvoicePaymentMethodToReadInList> paymentMethodList = (await PaymentMethodDataService.GetAllPaymentMethodsAsync()).ToList();
            PaymentMethodList.Add(new PaymentTypeModel { Id = 0, DisplayText = "None" });
            foreach (var method in paymentMethodList)
            {
                PaymentMethodList.Add(new PaymentTypeModel { Id = method.Id, DisplayText = method.Name });
            }
        }

        protected override void OnParametersSet()
        {
            if (parametersSet)
                return;
            parametersSet = true;

            Title = FormTitle.BuildTitle(FormMode, "Vendor");

            if (Vendor?.Address is null)
                Vendor.Address = new();
            paymentMethodId = Vendor?.DefaultPaymentMethod?.PaymentMethod.Id ?? 0;
            autoComplete = Vendor?.DefaultPaymentMethod?.AutoCompleteDocuments ?? false;
        }

        private void OnPaymentMethodChange()
        {
            if (paymentMethodId == 0)
                autoComplete = false;
        }

        protected async Task OnSaveAsync()
        {
            if (paymentMethodId == 0)
                Vendor.DefaultPaymentMethod = null;
            else
            {
                var readDto = await PaymentMethodDataService.GetPaymentMethodAsync(paymentMethodId);
                Vendor.DefaultPaymentMethod = new();
                Vendor.DefaultPaymentMethod.PaymentMethod = readDto;
                Vendor.DefaultPaymentMethod.AutoCompleteDocuments = autoComplete;
            }

            await OnValidSubmit.InvokeAsync(this);
        }

        protected class PaymentTypeModel
        {
            public long Id { get; set; }
            public string DisplayText { get; set; }
        }
    }
}
