using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Navigations;

namespace Menominee.Client.Components.Payables;

public partial class PayablesMenu
{
    [Inject]
    public NavigationManager? NavigationManager { get; set; }

    private static readonly string ModuleUrl = "/payables";
    public int MenuWidth { get; set; } = 468;

    private readonly List<MenuItem> menuItems = new()
    {
#pragma warning disable BL0005
        new MenuItem
        {
            Text = "Invoices",
            Id = ((int)PayablesMenuId.Invoices).ToString(),
            Url = $"{ModuleUrl}/invoices/listing"
        },
        new MenuItem
        {
            Text = "Credit Returns",
            Id = ((int)PayablesMenuId.CreditReturns).ToString(),
            Url = ""
        },
        new MenuItem
        {
            Text = "Statements",
            Id = ((int)PayablesMenuId.Statements).ToString(),
            Url = ""
        },
        new MenuItem
        {
            Text = "Vendors",
            Id = ((int)PayablesMenuId.Vendors).ToString(),
            Url = $"{ModuleUrl}/vendors/listing"
        },
        new MenuItem
        {
            Text = "Reports",
            Id = "-1",
            Items = new List<MenuItem>
            {
                new MenuItem { Text= "Vendor List", Url="reportviewer/vendorlisting.trdp/true", Id=((int)PayablesMenuId.VendorListReport).ToString() },
                new MenuItem { Separator= true, Url="" },
                new MenuItem { Text= "Vendor Detail", Url="", Id=((int)PayablesMenuId.VendorDetailReport).ToString() },
                new MenuItem { Text= "Vendor Summary", Url="", Id=((int)PayablesMenuId.VendorSummaryReport).ToString() },
                new MenuItem { Text= "Invoice Summary", Url="", Id=((int)PayablesMenuId.InvoiceSummaryReport).ToString() }
            },
            Url = ""
        }
     };
#pragma warning restore BL0005

    public void OnItemSelected(MenuItem selectedItem)
    {
        if (int.Parse(selectedItem.Id) == (int)PayablesMenuId.InvoiceSummaryReport)
        {
            NavigationManager?.NavigateTo("reportviewer/vendorInvoiceSummary.trdp/true");
        }
    }
}
