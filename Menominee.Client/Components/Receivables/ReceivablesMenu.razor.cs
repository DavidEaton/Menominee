using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Navigations;

namespace Menominee.Client.Components.Receivables;

public partial class ReceivablesMenu
{
    [Inject]
    public NavigationManager? NavigationManager { get; set; }

    private static readonly string ModuleUrl = "/receivables";
    private static int MenuWidth { get; set; } = 432;

    public void OnItemSelected(MenuItem selectedItem)
    {
        //if (int.Parse(selectedItem.Id) >= 0 && selectedItem.Url.Length > 0)
        //{
        //    navigationManager.NavigateTo(selectedItem.Url);
        //}
    }

    private readonly List<MenuItem> menuItems = new()
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
        }
     };
#pragma warning restore BL0005
}