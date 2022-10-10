using Blazored.Modal;
using Blazored.Modal.Services;
using CustomerVehicleManagement.Shared.Models.Payables.Invoices.Taxes;
using Menominee.Client.Components.Shared;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using Telerik.Blazor.Components;

namespace Menominee.Client.Components.Payables
{
    public partial class VendorInvoiceTaxes : ComponentBase
    {
        [Parameter]
        public IList<VendorInvoiceTaxToWrite> Taxes { get; set; }
        public TelerikGrid<VendorInvoiceTaxToWrite> Grid { get; set; }

        [CascadingParameter]
        IModalService ModalService { get; set; }

        [Parameter]
        public Action OnCalculateTotals { get; set; }

        private long SelectedId = 0;

        private void OnEdit(GridRowClickEventArgs args)
        {
            SelectedId = (args.Item as VendorInvoiceTaxToWrite).Id;

            if (SelectedId == 0)
            {
                var parameters = new ModalParameters();
                parameters.Add(nameof(ModalMessage.Message), "Please select a tax to edit.");
                ModalService.Show<ModalMessage>("Edit Tax", parameters);
                return;
            }

            // open dialog with FormMode == Edit

            // TEMPORARILY Edit an existing tax TO TEST
            var tax = Taxes.FirstOrDefault(tax => tax.Id == SelectedId);

            tax.Amount = 3.6;
        }

        private void OnNew()
        {
            // open dialog with FormMode == Add
        }

        private void OnDelete()
        {
            // display confirmation dialog, mark for deletion
        }

        private void OnRowSelected(GridRowClickEventArgs args)
        {
            SelectedId = (args.Item as VendorInvoiceTaxToWrite).Id;
        }
    }
}
