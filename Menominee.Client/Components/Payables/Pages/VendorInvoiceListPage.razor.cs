using CSharpFunctionalExtensions;
using CustomerVehicleManagement.Shared.Models.Payables.Invoices;
using CustomerVehicleManagement.Shared.Models.Payables.Vendors;
using Menominee.Client.Services.Payables.Invoices;
using Menominee.Client.Services.Payables.Vendors;
using Menominee.Client.Shared.Models;
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

        public ResourceParameters ResourceParameters { get; set; } = new ResourceParameters { Status = VendorInvoiceStatus.Open };

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

        private bool CanEdit { get; set; } = false;
        private bool CanDelete { get; set; } = false;

        protected override async Task OnInitializedAsync()
        {
            await GetVendorsAsync();
            ConfigureVendorInvoiceStatuses();
            await GetInvoiceList();
            SelectInvoices();
        }

        private void ConfigureVendorInvoiceStatuses()
        {
            foreach (VendorInvoiceStatus status in Enum.GetValues(typeof(VendorInvoiceStatus)))
                if (status != VendorInvoiceStatus.Unknown && status != VendorInvoiceStatus.ReturnSent)
                    VendorInvoiceStatusEnumData.Add(new VendorInvoiceStatusEnumModel { DisplayText = status.ToString(), Value = status });
        }

        private async Task GetVendorsAsync()
        {
            Vendors = (await VendorDataService.GetAllVendorsAsync())
                                  .Where(vendor => vendor.IsActive == true)
                                  .OrderBy(vendor => vendor.VendorCode)
                                  .ToList();
        }
        private async Task OnVendorFilterChangeHandlerAsync(object vendorId)
        {
            ResourceParameters.VendorId = (long?)vendorId;
            await GetInvoiceList();
            SelectInvoices();
        }

        private async Task OnStatusFilterChangeHandlerAsync(object status)
        {
            ResourceParameters.Status = (VendorInvoiceStatus?)status;
            Console.WriteLine($"status: {(VendorInvoiceStatus?)status}");
            await GetInvoiceList();
            SelectInvoices();
        }

        private async Task GetInvoiceList()
        {
            InvoiceList = await VendorInvoiceDataService.GetInvoices(ResourceParameters);
        }

        private void SelectInvoices()
        {
            if (InvoiceList?.Count > 0)
            {
                if (ItemToSelect > 0)
                    SelectedInvoice = InvoiceList.Where(x => x.Id == ItemToSelect).FirstOrDefault();

                if (ItemToSelect == 0 || SelectedInvoice == null)
                    SelectedInvoice = InvoiceList.FirstOrDefault();

                SelectedId = SelectedInvoice.Id;
                SelectedInvoices = new List<VendorInvoiceToReadInList> { SelectedInvoice };
            }

            if (InvoiceList?.Count == 0)
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
    }
}
