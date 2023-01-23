using CustomerVehicleManagement.Shared.Models.Payables.Invoices;
using Menominee.Client.Services.Payables.Invoices;
using Menominee.Common.Enums;
using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;

namespace Menominee.Client.Components.Payables.Pages
{
    public partial class VendorInvoiceEditPage : ComponentBase
    {
        [Inject]
        private NavigationManager NavigationManager { get; set; }

        [Inject]
        private IVendorInvoiceDataService VendorInvoiceDataService { get; set; }

        [Parameter]
        public long Id { get; set; }

        private VendorInvoiceToWrite Invoice { get; set; }
        private FormMode FormMode { get; set; }

        protected override async Task OnParametersSetAsync()
        {
            if (Id == 0)
            {
                Invoice = new()
                {
                    Date = DateTime.Today,
                    Status = VendorInvoiceStatus.Open
                };

                FormMode = FormMode.Add;
            }
            else
            {
                var readDto = await VendorInvoiceDataService.GetInvoice(Id);
                if (readDto != null)
                {
                    Invoice = VendorInvoiceHelper.ConvertReadToWriteDto(readDto);
                    FormMode = (Invoice.Status == VendorInvoiceStatus.Open) ? FormMode.Edit : FormMode.View;
                }
                else
                {
                    // TODO: What to do?
                    FormMode = FormMode.View;
                }
            }
        }

        private async Task<bool> Save()
        {
            bool valid = Valid();
            if (valid)
            {
                if (Id == 0)
                {
                    var invoice = await VendorInvoiceDataService.AddInvoice(Invoice);
                    Id = invoice.Id;
                }
                else
                {
                    await VendorInvoiceDataService.UpdateInvoice(Invoice, Id);
                }
            }

            return valid;
        }

        private async Task SaveAndExit()
        {
            if (await Save())
                EndEdit();
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
            // TODO: Need to preserve the listing's filter settings upon return
            NavigationManager.NavigateTo($"payables/invoices/listing/{Id}");
        }
    }
}
