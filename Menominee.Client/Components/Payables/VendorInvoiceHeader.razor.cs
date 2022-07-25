using CustomerVehicleManagement.Shared.Models.Payables.Vendors;
using CustomerVehicleManagement.Shared.Models.Payables.Invoices;
using Menominee.Client.Services.Payables.Vendors;
using Microsoft.AspNetCore.Components;
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

        public IReadOnlyList<VendorToReadInList> Vendors;

        private bool parametersSet = false;
        private long vendorId = 0;

        protected override async Task OnInitializedAsync()
        {
            Vendors = (await VendorDataService.GetAllVendorsAsync())
                                              .Where(vendor => vendor.IsActive == true)
                                              .OrderBy(vendor => vendor.VendorCode)
                                              .ToList();
        }

        protected override async Task OnParametersSetAsync()
        {
            if (parametersSet)
                return;
            parametersSet = true;

            if (Invoice?.Vendor?.Id != null)
            {
                vendorId = Invoice.Vendor.Id;
            }
        }

        private async Task OnVendorChangeAsync()
        {
            if (vendorId > 0 && Invoice.Vendor?.Id != vendorId)
            {
                Invoice.Vendor = VendorHelper.ConvertReadToWriteDto(await VendorDataService.GetVendorAsync(vendorId));
            }

            //if (Item?.Manufacturer is not null)
            //{
            //    long savedProductCodeId = productCodeId;
            //    ProductCodes = (await productCodeDataService.GetAllProductCodesAsync(manufacturerId)).ToList();
            //    if (savedProductCodeId > 0 && Item.ProductCode?.Id == 0 && ProductCodes.Any(pc => pc.Id == savedProductCodeId) == true)
            //        Item.ProductCode = ProductCodeHelper.ConvertReadToWriteDto(await productCodeDataService.GetProductCodeAsync(savedProductCodeId));
            //    productCodeId = savedProductCodeId;
            //}
            //else
            //{
            //    productCodeId = 0;
            //    ProductCodes = new List<ProductCodeToReadInList>();
            //    Item.ProductCode = new();
            //}

            //await OnProductCodeChangeAsync();
        }


    }
}
