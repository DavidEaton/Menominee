using CustomerVehicleManagement.Shared.Models.Payables.Invoices;
using Menominee.Client.Services.Payables.Invoices;
using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;

namespace Menominee.Client.Components.Payables
{
    public partial class VendorInvoiceEdit : ComponentBase
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
            else
            {
                var readDto = await vendorInvoiceDataService.GetInvoice(Id);
                Invoice = VendorInvoiceDtoHelper.ReadDtoToWriteDto(readDto);
            }
        }

        private async Task Save()
        {
            if (Valid())
            {
                if (Invoice.LineItems != null)
                {
                    foreach (var item in Invoice.LineItems)
                    {
                        if (item.Id < 0)
                            item.Id = 0;
                    }
                }

                if (Invoice.Taxes != null)
                {
                    foreach (var tax in Invoice.Taxes)
                    {
                        if (tax.Id < 0)
                            tax.Id = 0;
                    }
                }

                if (Invoice.Payments != null)
                {
                    foreach (var payment in Invoice.Payments)
                    {
                        if (payment.Id < 0)
                            payment.Id = 0;
                    }
                }

                if (Id == 0)
                {
                    var invoice = await vendorInvoiceDataService.AddInvoice(Invoice);
                    Id = invoice.Id;
                }
                else
                {
                    await vendorInvoiceDataService.UpdateInvoice(Invoice, Id);
                }

                EndEdit();
            }
        }

        private bool Valid()
        {
            if (Invoice.VendorId > 0 && Invoice.Date.HasValue)
                return true;

            return false;
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
