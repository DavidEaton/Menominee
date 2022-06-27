using Menominee.Client.Shared;
using Microsoft.AspNetCore.Components;
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

        public void OnItemSelected(ModuleMenuItem selectedItem)
        {
        }

        private List<ModuleMenuItem> menuItems = new List<ModuleMenuItem>
        {
            new ModuleMenuItem
            {
                Text = "Catalogs",
                Id = (int)RepairOrderEditMenuId.Catalogs,
                SubItems = new List<ModuleMenuItem>
                {
                    new ModuleMenuItem { Text="MVConnect", Url=$"{ModuleUrl}", Id=(int)RepairOrderEditMenuId.MVConnect },
                    new ModuleMenuItem { Text="Nexpart", Url=$"{ModuleUrl}", Id=(int)RepairOrderEditMenuId.Nexpart },
                    new ModuleMenuItem { Text="NAPA", Url=$"{ModuleUrl}", Id=(int)RepairOrderEditMenuId.NAPA },
                    new ModuleMenuItem { Text="IAP", Url=$"{ModuleUrl}", Id=(int)RepairOrderEditMenuId.IAP }
                },
                Url = ""
            },
            new ModuleMenuItem
            {
                Text = "Actions",
                Id = (int)RepairOrderEditMenuId.Actions,
                SubItems = new List<ModuleMenuItem>
                {
                    new ModuleMenuItem { Text="Authorize", Url=$"{ModuleUrl}", Id=(int)RepairOrderEditMenuId.Authorize },
                    new ModuleMenuItem { Text="Take Deposit", Url=$"{ModuleUrl}", Id=(int)RepairOrderEditMenuId.TakeDeposit },
                    new ModuleMenuItem { Text="Print Work Order", Url=$"{ModuleUrl}", Id=(int)RepairOrderEditMenuId.PrintWorkOrder },
                    new ModuleMenuItem { Text="Email Repair Order", Url=$"{ModuleUrl}", Id=(int)RepairOrderEditMenuId.EmailRepairOrder },
                    new ModuleMenuItem { Text="Text Messaging", Url=$"{ModuleUrl}", Id=(int)RepairOrderEditMenuId.TextMessaging }
                },
                Url = ""
            },
            new ModuleMenuItem
            {
                Text = "Inspection Log",
                Id = (int)RepairOrderEditMenuId.InspectionLog,
                Url = ""
            },
            new ModuleMenuItem
            {
                Text = "Caller ID",
                Id = (int)RepairOrderEditMenuId.CallerId,
                Url = ""
            }
         };
    }
}