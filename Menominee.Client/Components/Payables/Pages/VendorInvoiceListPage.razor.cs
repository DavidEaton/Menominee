using CSharpFunctionalExtensions;
using CustomerVehicleManagement.Shared.Models.Payables.Invoices;
using CustomerVehicleManagement.Shared.Models.Payables.Vendors;
using Menominee.Client.Services.Payables.Invoices;
using Menominee.Client.Services.Payables.Vendors;
using Menominee.Client.Shared.Models;
using Menominee.Common.Enums;
using Microsoft.AspNetCore.Components;
using Telerik.Blazor.Components;
using Telerik.Reporting;
using Telerik.Reporting.Processing;

namespace Menominee.Client.Components.Payables.Pages
{
    public partial class VendorInvoiceListPage : ComponentBase
    {
        [Inject]
        public NavigationManager? NavigationManager { get; set; }

        [Inject]
        public IVendorInvoiceDataService? VendorInvoiceDataService { get; set; }

        [Inject]
        public IVendorDataService? VendorDataService { get; set; }

        [Parameter]
        public long ItemToSelect { get; set; } = 0;

        private VendorInvoiceToRead? InvoiceToPrint { get; set; }
        private InvoiceTotals InvoiceTotals { get; set; } = new();
        private Dictionary<string, object> ReportParameters = new();

        public ResourceParameters ResourceParameters { get; set; } = new ResourceParameters { Status = VendorInvoiceStatus.Open };

        public IReadOnlyList<VendorInvoiceToReadInList>? InvoiceList;
        public IEnumerable<VendorInvoiceToReadInList> SelectedInvoices { get; set; } = Enumerable.Empty<VendorInvoiceToReadInList>();
        public VendorInvoiceToReadInList? SelectedInvoice { get; set; }

        public IReadOnlyList<VendorToRead>? Vendors;
        private IList<VendorInvoiceStatusEnumModel> VendorInvoiceStatusEnumData { get; set; } = new List<VendorInvoiceStatusEnumModel>();

        public long SelectedId
        {
            get => selectedId;
            set
            {
                selectedId = value;
                CanEdit = selectedId > 0;
                CanDelete = selectedId > 0;
                CanPrint = selectedId > 0;
            }
        }

        public TelerikGrid<VendorInvoiceToReadInList>? Grid { get; set; }

        private long selectedId;

        private bool CanEdit { get; set; } = false;
        private bool CanDelete { get; set; } = false;
        private bool CanPrint { get; set; } = false;
        private bool PrintingInvoice { get; set; } = false;

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
            if (VendorDataService is not null)
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
            if (VendorInvoiceDataService is not null)
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

                if (SelectedInvoice is not null)
                {
                    SelectedId = SelectedInvoice.Id;
                    SelectedInvoices = new List<VendorInvoiceToReadInList> { SelectedInvoice };
                }
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
            NavigationManager?.NavigateTo("payables/invoices/0");
        }

        private void OnEdit()
        {
            if (SelectedId > 0)
                NavigationManager?.NavigateTo($"payables/invoices/{SelectedId}");
        }

        private void OnDelete()
        {
        }

        private async Task OnPreview()
        {
            if (VendorInvoiceDataService is null)
                return;

            InvoiceToPrint = await VendorInvoiceDataService.GetInvoice(SelectedId);
            if (InvoiceToPrint is not null)
            {
                // TODO: Need to get the real shop name and number to display on the report
                var documentTypeString = InvoiceToPrint.DocumentType == VendorInvoiceDocumentType.Invoice ? "Vendor Invoice" : InvoiceToPrint.DocumentType.GetDisplayName();
                var nullDate = new DateTime(1899, 1, 1);
                InvoiceTotals.Calculate(InvoiceToPrint);
                InvoiceToPrint.Total = InvoiceTotals.Total;

                ReportParameters.Clear();
                ReportParameters.Add("VendorInvoiceId", SelectedId);
                ReportParameters.Add("ShopName", "Al's AutoCare (need real name & shop #)");
                ReportParameters.Add("ShopNumber", 1);
                ReportParameters.Add("DocumentTypeString", documentTypeString);
                ReportParameters.Add("VendorCode", InvoiceToPrint.Vendor.VendorCode);
                ReportParameters.Add("VendorName", InvoiceToPrint.Vendor.Name);
                ReportParameters.Add("VendorStreet", InvoiceToPrint.Vendor.Address.AddressLine);
                ReportParameters.Add("VendorCityStateZip", $"{InvoiceToPrint.Vendor.Address.City}, {InvoiceToPrint.Vendor.Address.State} {InvoiceToPrint.Vendor.Address.PostalCode}");
                ReportParameters.Add("InvoiceNumber", InvoiceToPrint.InvoiceNumber);
                ReportParameters.Add("DateCreated", InvoiceToPrint.Date != null ? InvoiceToPrint.Date : nullDate);
                ReportParameters.Add("DatePosted", InvoiceToPrint.DatePosted != null ? InvoiceToPrint.DatePosted : nullDate);
                ReportParameters.Add("StatusString", InvoiceToPrint.Status.GetDisplayName());
                ReportParameters.Add("Total", InvoiceTotals.Total);
                ReportParameters.Add("Balance", InvoiceTotals.Total - InvoiceTotals.Payments);
                PrintingInvoice = true;
            }
        }

        private async Task OnPrint()
        {
            await OnPreview();

            // TODO: MEN-522 - attempt this again once Telerik completes their changes
            //if (VendorInvoiceDataService is null)
            //    return;

            //InvoiceToPrint = await VendorInvoiceDataService.GetInvoice(SelectedId);
            //if (InvoiceToPrint is not null)
            //{
            //    // TODO: Need to get the real shop name and number to display on the report
            //    var documentTypeString = InvoiceToPrint.DocumentType == VendorInvoiceDocumentType.Invoice ? "Vendor Invoice" : InvoiceToPrint.DocumentType.GetDisplayName();
            //    var nullDate = new DateTime(1899, 1, 1);
            //    InvoiceTotals.Calculate(InvoiceToPrint);
            //    InvoiceToPrint.Total = InvoiceTotals.Total;

            //    //// Obtain the settings of the default printer
            //    //var printerSettings = new System.Drawing.Printing.PrinterSettings();
            //    //printerSettings.PrinterName

            //    //// The standard print controller comes with no UI
            //    //var standardPrintController = new System.Drawing.Printing.StandardPrintController();

            //    //// Print the report using the custom print controller
            //    //var reportProcessor = new Telerik.Reporting.Processing.ReportProcessor
            //    //{
            //    //    PrintController = standardPrintController
            //    //};

            //    //var reportSource = new Telerik.Reporting.TypeReportSource
            //    //{
            //    //    // reportName is the Assembly Qualified Name of the report
            //    //    TypeName = "VendorInvoice.trdp"
            //    //};

            //    //reportSource.Parameters.Clear();
            //    //reportSource.Parameters.Add("VendorInvoiceId", SelectedId);
            //    //reportSource.Parameters.Add("ShopName", "Al's AutoCare (need real name & shop #)");
            //    //reportSource.Parameters.Add("ShopNumber", 1);
            //    //reportSource.Parameters.Add("DocumentTypeString", documentTypeString);
            //    //reportSource.Parameters.Add("VendorCode", InvoiceToPrint.Vendor.VendorCode);
            //    //reportSource.Parameters.Add("VendorName", InvoiceToPrint.Vendor.Name);
            //    //reportSource.Parameters.Add("VendorStreet", InvoiceToPrint.Vendor.Address.AddressLine);
            //    //reportSource.Parameters.Add("VendorCityStateZip", $"{InvoiceToPrint.Vendor.Address.City}, {InvoiceToPrint.Vendor.Address.State} {InvoiceToPrint.Vendor.Address.PostalCode}");
            //    //reportSource.Parameters.Add("InvoiceNumber", InvoiceToPrint.InvoiceNumber);
            //    //reportSource.Parameters.Add("DateCreated", InvoiceToPrint.Date != null ? InvoiceToPrint.Date : nullDate);
            //    //reportSource.Parameters.Add("DatePosted", InvoiceToPrint.DatePosted != null ? InvoiceToPrint.DatePosted : nullDate);
            //    //reportSource.Parameters.Add("StatusString", InvoiceToPrint.Status.GetDisplayName());
            //    //reportSource.Parameters.Add("Total", InvoiceTotals.Total);
            //    //reportSource.Parameters.Add("Balance", InvoiceTotals.Total - InvoiceTotals.Payments);

            //    //reportProcessor.PrintReport(reportSource, printerSettings);

            //    var reportProcessor = new ReportProcessor();
            //    var deviceInfo = new System.Collections.Hashtable();
            //    System.Drawing.Printing.PrinterSettings? printerSettings = null;
            //    try
            //    {
            //        printerSettings = new System.Drawing.Printing.PrinterSettings();
            //    }
            //    catch (Exception ex)
            //    {
            //        Console.WriteLine("Create PrinterSettings Error: " + ex.ToString());
            //    }
            //    //var reportSource = new TypeReportSource
            //    //{
            //    //    TypeName = typeof(VendorInvoiceReport).AssemblyQualifiedName// "CustomerVehicleManagement.Api.Reports.VendorInvoice"
            //    //};
            //    var reportSource = new UriReportSource
            //    {
            //        Uri = "https://localhost:54382/api/reports/VendorInvoice.trdp"
            //    };

            //    Console.WriteLine("ReportSource = " + reportSource.ToString());

            //    reportSource.Parameters.Clear();
            //    reportSource.Parameters.Add("VendorInvoiceId", SelectedId);
            //    reportSource.Parameters.Add("ShopName", "Al's AutoCare (need real name & shop #)");
            //    reportSource.Parameters.Add("ShopNumber", 1);
            //    reportSource.Parameters.Add("DocumentTypeString", documentTypeString);
            //    reportSource.Parameters.Add("VendorCode", InvoiceToPrint.Vendor.VendorCode);
            //    reportSource.Parameters.Add("VendorName", InvoiceToPrint.Vendor.Name);
            //    reportSource.Parameters.Add("VendorStreet", InvoiceToPrint.Vendor.Address.AddressLine);
            //    reportSource.Parameters.Add("VendorCityStateZip", $"{InvoiceToPrint.Vendor.Address.City}, {InvoiceToPrint.Vendor.Address.State} {InvoiceToPrint.Vendor.Address.PostalCode}");
            //    reportSource.Parameters.Add("InvoiceNumber", InvoiceToPrint.InvoiceNumber);
            //    reportSource.Parameters.Add("DateCreated", InvoiceToPrint.Date != null ? InvoiceToPrint.Date : nullDate);
            //    reportSource.Parameters.Add("DatePosted", InvoiceToPrint.DatePosted != null ? InvoiceToPrint.DatePosted : nullDate);
            //    reportSource.Parameters.Add("StatusString", InvoiceToPrint.Status.GetDisplayName());
            //    reportSource.Parameters.Add("Total", InvoiceTotals.Total);
            //    reportSource.Parameters.Add("Balance", InvoiceTotals.Total - InvoiceTotals.Payments);

            //    try
            //    {
            //        RenderingResult result = reportProcessor.RenderReport("PDF", reportSource, deviceInfo);
            //    }
            //    catch (Exception ex)
            //    {
            //        Console.WriteLine("Rendor Error: " + ex.ToString());
            //    }
            //    try
            //    {
            //        reportProcessor.PrintReport(reportSource, printerSettings);
            //    }
            //    catch (Exception ex)
            //    {
            //        Console.WriteLine("Print Error: " + ex.ToString());
            //    }

            //    /*if (!result.HasErrors)
            //    {
            //        string fileName = result.DocumentName + "." + result.Extension;
            //        string path = Path.GetTempPath();
            //        string filePath = Path.Combine(path, fileName);

            //        using (System.IO.FileStream fs = new System.IO.FileStream(filePath, FileMode.Create))
            //        {
            //            fs.Write(result.DocumentBytes, 0, result.DocumentBytes.Length);
            //        }
            //    }*/
            //}
        }

        private void HideReport()
        {
            PrintingInvoice = false;
        }

        protected void OnSelect(IEnumerable<VendorInvoiceToReadInList> invoices)
        {
            SelectedInvoice = invoices.FirstOrDefault();
            if (SelectedInvoice is not null)
                SelectedInvoices = new List<VendorInvoiceToReadInList> { SelectedInvoice };
        }

        private void OnRowSelected(GridRowClickEventArgs args)
        {
            SelectedId = ((VendorInvoiceToReadInList)args.Item).Id;
        }

        private void OnDone()
        {
            NavigationManager?.NavigateTo("/payables/");
        }
    }
}
