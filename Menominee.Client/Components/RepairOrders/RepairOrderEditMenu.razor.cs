using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Navigations;
using System.Collections.Generic;

//-------------------------------------------------------------------------------
// THIS COMPONENT IS NO LONGER IN USE - WILL DELETE WHEN CONFIRMED TO BE OBSOLETE
//-------------------------------------------------------------------------------

namespace Menominee.Client.Components.RepairOrders
{
    public partial class RepairOrderEditMenu
    {
        [Inject]
        public NavigationManager navigationManager { get; set; }

        private static string ModuleUrl = "/repairorders";

        public void OnItemSelected(MenuItem selectedItem)
        {
        }

        private List<MenuItem> menuItems = new()
        {
#pragma warning disable BL0005
            new MenuItem
            {
                Text = "Catalogs",
                Id = ((int)RepairOrderEditMenuId.Catalogs).ToString(),
                Items = new List<MenuItem>
                {
                    new MenuItem { Text="MVConnect", Url=$"{ModuleUrl}", Id=((int)RepairOrderEditMenuId.MVConnect).ToString() },
                    new MenuItem { Text="Nexpart", Url=$"{ModuleUrl}", Id=((int)RepairOrderEditMenuId.Nexpart).ToString() },
                    new MenuItem { Text="NAPA", Url=$"{ModuleUrl}", Id=((int)RepairOrderEditMenuId.NAPA).ToString() },
                    new MenuItem { Text="IAP", Url=$"{ModuleUrl}", Id=((int)RepairOrderEditMenuId.IAP).ToString() }
                },
                Url = ""
            },
            new MenuItem
            {
                Text = "Actions",
                Id = ((int)RepairOrderEditMenuId.Actions).ToString(),
                Items = new List<MenuItem>
                {
                    new MenuItem { Text="Authorize", Url=$"{ModuleUrl}", Id=((int)RepairOrderEditMenuId.Authorize).ToString() },
                    new MenuItem { Text="Take Deposit", Url=$"{ModuleUrl}", Id=((int)RepairOrderEditMenuId.TakeDeposit).ToString() },
                    new MenuItem { Text="Print Work Order", Url=$"{ModuleUrl}", Id=((int)RepairOrderEditMenuId.PrintWorkOrder).ToString() },
                    new MenuItem { Text="Email Repair Order", Url=$"{ModuleUrl}", Id=((int)RepairOrderEditMenuId.EmailRepairOrder).ToString() },
                    new MenuItem { Text="Text Messaging", Url=$"{ModuleUrl}", Id=((int)RepairOrderEditMenuId.TextMessaging).ToString() }
                },
                Url = ""
            },
            new MenuItem
            {
                Text = "Inspection Log",
                Id = ((int)RepairOrderEditMenuId.InspectionLog).ToString(),
                Url = ""
            },
            new MenuItem
            {
                Text = "Caller ID",
                Id = ((int)RepairOrderEditMenuId.CallerId).ToString(),
                Url = ""
            }
         };
#pragma warning restore BL0005
    }
}