using CSharpFunctionalExtensions;
using CustomerVehicleManagement.Shared.Models.Payables.Invoices;
using CustomerVehicleManagement.Shared.Models.Payables.Vendors;
using Menominee.Client.Services.Payables.Vendors;
using Menominee.Client.Shared.Models;
using Menominee.Common.Enums;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Menominee.Client.Components.Payables
{
    public partial class VendorInvoiceHeader : ComponentBase
    {
        [Inject]
        public IVendorDataService VendorDataService { get; set; }

        [Parameter]
        public VendorInvoiceToWrite Invoice { get; set; }

        [CascadingParameter]
        public FormMode FormMode { get; set; }

        private bool CanEdit { get; set; } = false;

        public IReadOnlyList<VendorToRead> Vendors;
        private IList<VendorInvoiceStatusEnumModel> VendorInvoiceStatusEnumData { get; set; } = new List<VendorInvoiceStatusEnumModel>();

        private long vendorId = 0;
        private string invoiceStatus = "";

        protected override async Task OnInitializedAsync()
        {
            foreach (VendorInvoiceStatus status in Enum.GetValues(typeof(VendorInvoiceStatus)))
                VendorInvoiceStatusEnumData.Add(new VendorInvoiceStatusEnumModel { DisplayText = status.ToString(), Value = status });

            Vendors = (await VendorDataService.GetAllVendorsAsync())
                                              .Where(vendor => vendor.IsActive == true)
                                              .OrderBy(vendor => vendor.VendorCode)
                                              .ToList();
        }

        protected override void OnParametersSet()
        {
            CanEdit = FormMode == FormMode.Add || FormMode == FormMode.Edit;

            if (Invoice?.Vendor?.Id != null)
            {
                vendorId = Invoice.Vendor.Id;
            }

            if (Invoice != null)
            {
                invoiceStatus = Invoice.Status.GetDisplayName();
            }
        }

        private void OnVendorChange()
        {
            if (vendorId > 0 && Invoice.Vendor?.Id != vendorId)
                Invoice.Vendor = Vendors.FirstOrDefault(x => x.Id == vendorId);
        }
    }
}
