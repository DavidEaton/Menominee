using Menominee.Client.Shared;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;

namespace Menominee.Client.Components.Payables
{
    public partial class PayablesMenu
    {
        [Inject]
        public NavigationManager navigationManager { get; set; }

        private static string ModuleUrl = "/payables";

        public void OnItemSelected(ModuleMenuItem selectedItem)
        {
            if (selectedItem.Id >= 0)
            {
                //string url = "/payables/" + selectedItem.Url;
                //navigationManager.NavigateTo(url);
                //string url = "/payables";
                //navigationManager.NavigateTo($"{url}/{selectedItem.Url}");
                //navigationManager.NavigateTo(selectedItem.Url);

                //switch (selectedItem?.Id >= 0)
                //{
                //    case "invoices":
                //        navigationManager.NavigateTo($"{url}/invoices/listing");
                //        break;
                //    case "creditReturns":
                //        navigationManager.NavigateTo($"{url}/returns/listing");
                //        break;
                //    case "vendors":
                //        navigationManager.NavigateTo($"{url}/vendors/listing");
                //        break;
                //    default:
                //        navigationManager.NavigateTo(url);
                //        break;
                //}
            }
        }

        private List<ModuleMenuItem> menuItems = new List<ModuleMenuItem>
        {
            new ModuleMenuItem
            {
                Text = "Invoices",
                Id = (int)PayablesMenuId.Invoices,
                Url = $"{ModuleUrl}/invoices/listing"
            },
            new ModuleMenuItem
            {
                Text = "Credit Returns",
                Id = (int)PayablesMenuId.CreditReturns,
                Url = $"{ModuleUrl}/returns/listing"
            },
            new ModuleMenuItem
            {
                Text = "Statements",
                Id = (int)PayablesMenuId.Statements,
                Url = ""
            },
            new ModuleMenuItem
            {
                Text = "Vendors",
                Id = (int)PayablesMenuId.Vendors,
                Url = $"{ModuleUrl}/vendors/listing"
            },
            new ModuleMenuItem
            {
                Text = "Reports",
                SubItems = new List<ModuleMenuItem>
                {
                    new ModuleMenuItem { Text= "Vendor List", Url="", Id=(int)PayablesMenuId.VendorListReport },
                    new ModuleMenuItem { Separator= true, Url="" },
                    new ModuleMenuItem { Text= "Vendor Detail", Url="", Id=(int)PayablesMenuId.VendorDetailReport },
                    new ModuleMenuItem { Text= "Vendor Summary", Url="", Id=(int)PayablesMenuId.VendorSummaryReport },
                    new ModuleMenuItem { Text= "Invoice Summary", Url="", Id=(int)PayablesMenuId.InvoiceSummaryReport }
                },
                Url = "/"
            }
         };
    }
}
