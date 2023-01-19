using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Navigations;
using System.Collections.Generic;

namespace Menominee.Client.Components.Receivables
{
    public partial class ReceivablesMenu
    {
        [Inject]
        public NavigationManager navigationManager { get; set; }

        private static string ModuleUrl = "/receivables";
        public int menuWidth { get; set; } = 526;

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
                Text = "Accounts",
                Id = "-1",
                Items = new List<MenuItem>
                {
                    new MenuItem { Text="Accounts List", Url=$"{ModuleUrl}/accounts/list", Id=((int)ReceivablesMenuId.AccountsList).ToString() },
                    new MenuItem { Text="Credit Hold List", Url=$"{ModuleUrl}/accounts/creditholdlist", Id=((int)ReceivablesMenuId.CreditHoldList).ToString() }
                },
                Url = ""
            },
            new MenuItem
            {
                Text = "Transactions",
                Id = "-1",
                Items = new List<MenuItem>
                {
                    new MenuItem { Text="New Transaction", Url=$"{ModuleUrl}/", Id=((int)ReceivablesMenuId.NewTransaction).ToString() },
                    new MenuItem { Text="Batch Files", Url=$"{ModuleUrl}/", Id=((int)ReceivablesMenuId.BatchFiles).ToString() },
                    new MenuItem { Text="Apply Open Credits", Url=$"{ModuleUrl}/", Id=((int)ReceivablesMenuId.ApplyOpenCredits).ToString() },
                    new MenuItem { Text="Clear Paid Transactions", Url=$"{ModuleUrl}/", Id=((int)ReceivablesMenuId.ClearPaidTransactions).ToString() },
                    new MenuItem { Text="Unapply Transaction", Url=$"{ModuleUrl}/", Id=((int)ReceivablesMenuId.UnapplyTransaction).ToString() }
                },
                Url = ""
            },
            new MenuItem
            {
                Text = "Service Charges",
                Id = "-1",
                Items = new List<MenuItem>
                {
                    new MenuItem { Text="Create & Post", Url=$"{ModuleUrl}/", Id=((int)ReceivablesMenuId.CreateAndPost).ToString() },
                    new MenuItem { Text="Waive Charges", Url=$"{ModuleUrl}/", Id=((int)ReceivablesMenuId.WaiveCharges).ToString() }
                },
                Url = ""
            },
            new MenuItem
            {
                Text = "Reports",
                Id = ((int)ReceivablesMenuId.Reports).ToString(),
                Url = ""
            },
            new MenuItem
            {
                Text = "Setup",
                Id = ((int)ReceivablesMenuId.Setup).ToString(),
                Url = ""
            }
         };
#pragma warning restore BL0005
    }
}