using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Navigations;
using System.Collections.Generic;

namespace Menominee.Client.Components.Payables
{
    public partial class PayablesMenu
    {
        [Inject]
        public NavigationManager navigationManager { get; set; }

        private static string ModuleUrl = "/payables";
        public int menuWidth { get; set; } = 470;

        public void OnItemSelected(MenuItem selectedItem)
        {
            //if (Int32.Parse(selectedItem.Id) >= 0 && selectedItem.Url.Length > 0)
            //{
            //    navigationManager.NavigateTo(selectedItem.Url);
            //}
        }

        private List<MenuItem> menuItems = new()
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
                Url = $"{ModuleUrl}/returns/listing"
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
                Id = "-1",//((int)PayablesMenuId.Reports).ToString(),
                Items = new List<MenuItem>
                {
                    new MenuItem { Text= "Vendor List", Url="", Id=((int)PayablesMenuId.VendorListReport).ToString() },
                    new MenuItem { Separator= true, Url="" },
                    new MenuItem { Text= "Vendor Detail", Url="", Id=((int)PayablesMenuId.VendorDetailReport).ToString() },
                    new MenuItem { Text= "Vendor Summary", Url="", Id=((int)PayablesMenuId.VendorSummaryReport).ToString() },
                    new MenuItem { Text= "Invoice Summary", Url="", Id=((int)PayablesMenuId.InvoiceSummaryReport).ToString() }
                },
                Url = ""
            },
            new MenuItem
            {
                Text = "Settings",
                Id = "-1",
                Items = new List<MenuItem>
                {
                    new MenuItem { Text= "Payment Methods", Url=$"{ModuleUrl}/paymentmethods/listing", Id=((int)PayablesMenuId.PaymentMethods).ToString() }
                },
                Url = ""
            }
         };
#pragma warning restore BL0005
    }
}
