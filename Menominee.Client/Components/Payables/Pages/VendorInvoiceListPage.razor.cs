using CustomerVehicleManagement.Shared.Models.Payables.Invoices;
using CustomerVehicleManagement.Shared.Models.Payables.Vendors;
using Menominee.Client.Services.Payables.Invoices;
using Menominee.Client.Services.Payables.Vendors;
using Menominee.Common.Enums;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telerik.Blazor.Components;

namespace Menominee.Client.Components.Payables.Pages
{
    public partial class VendorInvoiceListPage : ComponentBase
    {
        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Inject]
        public IVendorInvoiceDataService VendorInvoiceDataService { get; set; }

        [Inject]
        public IVendorDataService VendorDataService { get; set; }

        //[Inject]
        //public ILogger<VendorInvoiceList> Logger { get; set; }

        [Parameter]
        public long ItemToSelect { get; set; } = 0;

        public IReadOnlyList<VendorInvoiceToReadInList> InvoiceList;
        public IEnumerable<VendorInvoiceToReadInList> SelectedInvoices { get; set; } = Enumerable.Empty<VendorInvoiceToReadInList>();
        public VendorInvoiceToReadInList SelectedInvoice { get; set; }

        public IReadOnlyList<VendorToRead> Vendors;
        private IList<VendorInvoiceStatusEnumModel> VendorInvoiceStatusEnumData { get; set; } = new List<VendorInvoiceStatusEnumModel>();

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
        private long vendorId = 0;
        private VendorInvoiceStatus invoiceStatus = VendorInvoiceStatus.Open;

        private bool CanEdit { get; set; } = false;
        private bool CanDelete { get; set; } = false;

        protected override async Task OnInitializedAsync()
        {
            Vendors = (await VendorDataService.GetAllVendorsAsync())
                                              .Where(vendor => vendor.IsActive == true)
                                              .OrderBy(vendor => vendor.VendorCode)
                                              .ToList();

            foreach (VendorInvoiceStatus status in Enum.GetValues(typeof(VendorInvoiceStatus)))
                if (status != VendorInvoiceStatus.Unknown && status != VendorInvoiceStatus.ReturnSent)
                    VendorInvoiceStatusEnumData.Add(new VendorInvoiceStatusEnumModel { DisplayText = status.ToString(), Value = status });

            await GetFilteredInvoiceListAsync();
        }

        private async Task OnFilterInvoicesChangeAsync()
        {
            await GetFilteredInvoiceListAsync();
        }

        private async Task GetFilteredInvoiceListAsync()
        {
            // TODO: Move the filtering into the API?
            // TODO: Must be a more elegant way to build the Where clause but haven't researched it yet.
            if (vendorId == 0)
            {
                if (invoiceStatus == VendorInvoiceStatus.Unknown)
                    InvoiceList = (await VendorInvoiceDataService.GetAllInvoices()).ToList();
                else
                    InvoiceList = (await VendorInvoiceDataService.GetAllInvoices()).Where(x => x.Status == invoiceStatus.ToString()).ToList();
            }
            else
            {
                if (invoiceStatus == VendorInvoiceStatus.Unknown)
                    InvoiceList = (await VendorInvoiceDataService.GetAllInvoices()).Where(x => x.VendorId == vendorId).ToList();
                else
                    InvoiceList = (await VendorInvoiceDataService.GetAllInvoices()).Where(x => x.VendorId == vendorId
                                                                                            && x.Status == invoiceStatus.ToString()).ToList();
            }

            if (InvoiceList.Count > 0)
            {
                if (ItemToSelect == 0)
                    SelectedInvoice = InvoiceList.FirstOrDefault();
                else
                    SelectedInvoice = InvoiceList.Where(x => x.Id == ItemToSelect).FirstOrDefault();
                SelectedId = SelectedInvoice.Id;
                SelectedInvoices = new List<VendorInvoiceToReadInList> { SelectedInvoice };
            }
            else
            {
                SelectedInvoice = null;
                SelectedId = 0;
                SelectedInvoices = Enumerable.Empty<VendorInvoiceToReadInList>();
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

        // TODO: This was copied from VendorInvoiceHeader.razor.cs and needs to be moved into separate class
        internal class VendorInvoiceStatusEnumModel
        {
            public VendorInvoiceStatus Value { get; set; }
            public string DisplayText { get; set; }
        }
    }
}
