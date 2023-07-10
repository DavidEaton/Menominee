using Menominee.Shared.Models.Addresses;
using Menominee.Shared.Models.Payables.Invoices.Payments;
using Menominee.Shared.Models.Payables.Vendors;
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
        protected long PaymentMethodId { get; set; } = 0;
        protected bool AutoComplete { get; set; } = false;
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
            PaymentMethodId = Vendor?.DefaultPaymentMethod?.PaymentMethod.Id ?? 0;
            AutoComplete = Vendor?.DefaultPaymentMethod?.AutoCompleteDocuments ?? false;
        }

        private void OnPaymentMethodChange()
        {
            if (PaymentMethodId == 0)
                AutoComplete = false;
        }

        protected async Task OnSaveAsync()
        {
            if (!AddressIsValid(Vendor.Address))
                Vendor.Address = null;

            if (PaymentMethodId == 0)
                Vendor.DefaultPaymentMethod = null;
            else
            {
                var readDto = await PaymentMethodDataService.GetPaymentMethodAsync(PaymentMethodId);
                Vendor.DefaultPaymentMethod = new();
                Vendor.DefaultPaymentMethod.PaymentMethod = readDto;
                Vendor.DefaultPaymentMethod.AutoCompleteDocuments = AutoComplete;
            }

            await OnValidSubmit.InvokeAsync(this);
        }

        private bool AddressIsValid(AddressToWrite address)
        {
            if (address is null)
                return false;

            var validator = new AddressValidator();

            return validator.Validate(address).IsValid;
        }

        protected class PaymentTypeModel
        {
            public long Id { get; set; }
            public string DisplayText { get; set; }
        }
    }
}
