using Menominee.Client.Shared;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;

namespace Menominee.Client.Components.Receivables
{
    public partial class ReceivablesMenu
    {
        [Inject]
        public NavigationManager navigationManager { get; set; }

        private static string ModuleUrl = "/receivables";

        public void OnItemSelected(ModuleMenuItem selectedItem)
        {
        }

        private List<ModuleMenuItem> menuItems = new List<ModuleMenuItem>
        {
            new ModuleMenuItem
            {
                Text = "Accounts",
                Id = (int)ReceivablesMenuId.Accounts,
                SubItems = new List<ModuleMenuItem>
                {
                    new ModuleMenuItem { Text="Accounts List", Url=$"{ModuleUrl}/accounts/list", Id=(int)ReceivablesMenuId.AccountsList },
                    new ModuleMenuItem { Text="Credit Hold List", Url=$"{ModuleUrl}/accounts/creditholdlist", Id=(int)ReceivablesMenuId.CreditHoldList }
                },
                Url = ""
            },
            new ModuleMenuItem
            {
                Text = "Transactions",
                Id = (int)ReceivablesMenuId.Transactions,
                SubItems = new List<ModuleMenuItem>
                {
                    new ModuleMenuItem { Text="New Transaction", Url=$"{ModuleUrl}/", Id=(int)ReceivablesMenuId.NewTransaction },
                    new ModuleMenuItem { Text="Batch Files", Url=$"{ModuleUrl}/", Id=(int)ReceivablesMenuId.BatchFiles },
                    new ModuleMenuItem { Text="Apply Open Credits", Url=$"{ModuleUrl}/", Id=(int)ReceivablesMenuId.ApplyOpenCredits },
                    new ModuleMenuItem { Text="Clear Paid Transactions", Url=$"{ModuleUrl}/", Id=(int)ReceivablesMenuId.ClearPaidTransactions },
                    new ModuleMenuItem { Text="Unapply Transaction", Url=$"{ModuleUrl}/", Id=(int)ReceivablesMenuId.UnapplyTransaction }
                },
                Url = ""
            },
            new ModuleMenuItem
            {
                Text = "Service Charges",
                Id = (int)ReceivablesMenuId.ServiceCharges,
                SubItems = new List<ModuleMenuItem>
                {
                    new ModuleMenuItem { Text="Create & Post", Url=$"{ModuleUrl}/", Id=(int)ReceivablesMenuId.CreateAndPost },
                    new ModuleMenuItem { Text="Waive Charges", Url=$"{ModuleUrl}/", Id=(int)ReceivablesMenuId.WaiveCharges }
                },
                Url = ""
            },
            new ModuleMenuItem
            {
                Text = "Reports",
                Id = (int)ReceivablesMenuId.Reports,
                Url = ""
            },
            new ModuleMenuItem
            {
                Text = "Setup",
                Id = (int)ReceivablesMenuId.Setup,
                Url = ""
            }
         };
    }
}