using CustomerVehicleManagement.Shared.Models.Payables.Invoices;
using Menominee.Client.Services.Payables.Invoices;
using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;

namespace Menominee.Client.Components.Payables.Pages
{
    public partial class VendorInvoiceEditPage : ComponentBase
    {
        [Inject]
        private NavigationManager navigationManager { get; set; }

        [Inject]
        private IVendorInvoiceDataService vendorInvoiceDataService { get; set; }

        [Parameter]
        public long Id { get; set; }

        private VendorInvoiceToWrite Invoice { get; set; }

        protected override async Task OnInitializedAsync()
        {
            if (Id == 0)
            {
                Invoice = new();
                Invoice.Date = DateTime.Today;
            }

            if (Id != 0)
            {
                var readDto = await vendorInvoiceDataService.GetInvoice(Id);
                Invoice = VendorInvoiceHelper.ConvertReadToWriteDto(readDto);
            }
        }

        private async Task Save()
        {
            if (Valid())
            {
                if (Id == 0)
                {
                    var invoice = await vendorInvoiceDataService.AddInvoice(Invoice);
                    Id = invoice.Id;
                    EndEdit();
                    return;
                }

                if (Id != 0)
                    await vendorInvoiceDataService.UpdateInvoice(Invoice, Id);

                EndEdit();
            }
        }

        private bool Valid()
        {
            // Child components will validate thier own data:
            // VendorInvoiceItems
            // VendorInvoicePayments
            // VendorInvoiceTaxes

            return Invoice?.Vendor is not null && Invoice.Date.HasValue;
        }

        private void Discard()
        {
            EndEdit();
        }

        protected void EndEdit()
        {
            navigationManager.NavigateTo($"payables/invoices/listing/{Id}");
        }
    }
}
