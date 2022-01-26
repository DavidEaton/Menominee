using MenomineePlayWASM.Shared.Dtos.Payables.Invoices;
using MenomineePlayWASM.Shared.Services.Payables.Invoices;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telerik.Blazor.Components;

namespace MenomineePlayWASM.Client.Pages.Payables
{
    public partial class VendorInvoiceList : ComponentBase
    {
        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Inject]
        public IVendorInvoiceDataService VendorInvoiceDataService { get; set; }

        //[Inject]
        //public ILogger<VendorInvoiceList> Logger { get; set; }

        [Parameter]
        public long ItemToSelect { get; set; } = 0;

        public IReadOnlyList<VendorInvoiceToReadInList> InvoiceList;
        public IEnumerable<VendorInvoiceToReadInList> SelectedInvoices { get; set; } = Enumerable.Empty<VendorInvoiceToReadInList>();
        public VendorInvoiceToReadInList SelectedInvoice { get; set; }

        public long SelectedId
        {
            get => selectedId;
            set
            {
                selectedId = value;
                CanEdit = selectedId > 0;
                CanDelete = selectedId > 0;
            }
        }

        //[CascadingParameter]
        //IModalService ModalService { get; set; }

        public TelerikGrid<VendorInvoiceToReadInList> Grid { get; set; }

        private long selectedId;

        private bool CanEdit { get; set; } = false;
        private bool CanDelete { get; set; } = false;

        protected override async Task OnInitializedAsync()
        {
            InvoiceList = (await VendorInvoiceDataService.GetAllInvoices()).ToList();

            if (InvoiceList.Count > 0)
            {
                if (ItemToSelect == 0)
                {
                    SelectedInvoice = InvoiceList.FirstOrDefault();
                }
                else
                {
                    SelectedInvoice = InvoiceList.Where(x => x.Id == ItemToSelect).FirstOrDefault();
                }
                SelectedId = SelectedInvoice.Id;
                SelectedInvoices = new List<VendorInvoiceToReadInList> { SelectedInvoice };
            }
        }

        private void OnAdd()
        {
            NavigationManager.NavigateTo("payables/invoices/0");
        }

        private void OnEdit()
        {
            //if (SelectedId == 0)
            //{
            //    var parameters = new ModalParameters();
            //    parameters.Add(nameof(ModalMessage.Message), "Please select an invoice to edit.");
            //    ModalService.Show<ModalMessage>("Edit Invoice", parameters);
            //    return;
            //}

            NavigationManager.NavigateTo($"payables/invoices/{SelectedId}");
        }

        private void OnDelete()
        {
        }

        protected void OnSelect(IEnumerable<VendorInvoiceToReadInList> invoices)
        {
            SelectedInvoice = invoices.FirstOrDefault();
            SelectedInvoices = new List<VendorInvoiceToReadInList> { SelectedInvoice };
        }

        private void OnRowSelected(GridRowClickEventArgs args)
        {
            //SelectedInvoice = args.Item as VendorInvoiceToReadInList;
            //SelectedInvoices = new List<VendorInvoiceToReadInList> { SelectedInvoice };

            SelectedId = (args.Item as VendorInvoiceToReadInList).Id;
        }

        private void OnDone()
        {
            NavigationManager.NavigateTo("/payables/");
        }
    }
}
