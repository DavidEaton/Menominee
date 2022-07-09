using Menominee.Client.Shared;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Navigations;
using System;
using System.Collections.Generic;

namespace Menominee.Client.Components.RepairOrders
{
    public partial class RepairOrdersMenu
    {
        [Inject]
        public NavigationManager navigationManager { get; set; }

        [Parameter]
        public bool RepairOrderEditMenuVisible { get; set; } = false;

        private static string ModuleUrl = "/repairorders";
        public int menuWidth { get; set; } = 250;

        public void OnItemSelected(MenuItem selectedItem)
        {
        }

        protected override void OnParametersSet()
        {
            menuItems.Clear();

            if (RepairOrderEditMenuVisible)
            {
                menuWidth = 405;
                menuItems.Add(new MenuItem
                {
                    Text = "Catalogs",
                    Id = "-1",//((int)RepairOrderEditMenuId.Catalogs).ToString(),
                    Items = new List<MenuItem>
                    {
                        new MenuItem { Text="MVConnect", Url=$"{ModuleUrl}", Id=((int)RepairOrderEditMenuId.MVConnect).ToString() },
                        new MenuItem { Text="Nexpart", Url=$"{ModuleUrl}", Id=((int)RepairOrderEditMenuId.Nexpart).ToString() },
                        new MenuItem { Text="NAPA", Url=$"{ModuleUrl}", Id=((int)RepairOrderEditMenuId.NAPA).ToString() },
                        new MenuItem { Text="IAP", Url=$"{ModuleUrl}", Id=((int)RepairOrderEditMenuId.IAP).ToString() }
                    },
                    Url = ""
                });

                menuItems.Add(new MenuItem
                {
                    Text = "Actions",
                    Id = "-1",//((int)RepairOrderEditMenuId.Actions).ToString(),
                    Items = new List<MenuItem>
                    {
                        new MenuItem { Text="Authorize", Url=$"{ModuleUrl}", Id=((int)RepairOrderEditMenuId.Authorize).ToString() },
                        new MenuItem { Text="Take Deposit", Url=$"{ModuleUrl}", Id=((int)RepairOrderEditMenuId.TakeDeposit).ToString() },
                        new MenuItem { Text="Print Work Order", Url=$"{ModuleUrl}", Id=((int)RepairOrderEditMenuId.PrintWorkOrder).ToString() },
                        new MenuItem { Text="Email Repair Order", Url=$"{ModuleUrl}", Id=((int)RepairOrderEditMenuId.EmailRepairOrder).ToString() },
                        new MenuItem { Text="Text Messaging", Url=$"{ModuleUrl}", Id=((int)RepairOrderEditMenuId.TextMessaging).ToString() }
                    },
                    Url = ""
                });

                menuItems.Add(new MenuItem
                {
                    Text = "Inspection Log",
                    Id = ((int)RepairOrderEditMenuId.InspectionLog).ToString(),
                    Url = ""
                });

                menuItems.Add(new MenuItem
                {
                    Text = "Caller ID",
                    Id = ((int)RepairOrderEditMenuId.CallerId).ToString(),
                    Url = ""
                });
            }
            else
            {
                menuWidth = 250;
                menuItems.Add(new MenuItem
                {
                    Text = "Repair Orders",
                    Id = "-1",//((int)RepairOrdersMenuId.RepairOrders).ToString(),
                    Items = new List<MenuItem>
                    {
                        new MenuItem { Text="Today's Repair Orders", Url=$"{ModuleUrl}/worklog", Id=((int)RepairOrdersMenuId.TodaysROs).ToString() },
                        new MenuItem { Text="Waiting On Parts", Url=$"{ModuleUrl}/worklog", Id=((int)RepairOrdersMenuId.WaitingOnParts).ToString() },
                        new MenuItem { Text="Ready For Pickup", Url=$"{ModuleUrl}/worklog", Id=((int)RepairOrdersMenuId.ReadyForPickup).ToString() },
                        new MenuItem { Text="All Repair Orders", Url=$"{ModuleUrl}/worklog", Id=((int)RepairOrdersMenuId.AllROs).ToString() }
                    },
                    Url = ""
                });

                menuItems.Add(new MenuItem
                {
                    Text = "Invoices",
                    Id = "-1",//((int)RepairOrdersMenuId.Invoices).ToString(),
                    Items = new List<MenuItem>
                    {
                        new MenuItem { Text="Unpaid Invoices", Url=$"{ModuleUrl}/worklog", Id=((int)RepairOrdersMenuId.UnpaidInvoices).ToString() },
                        new MenuItem { Text="Today's Invoices", Url=$"{ModuleUrl}/worklog", Id=((int)RepairOrdersMenuId.TodaysInvoices).ToString() },
                        new MenuItem { Text="All Invoices", Url=$"{ModuleUrl}/worklog", Id=((int)RepairOrdersMenuId.AllInvoices).ToString() }
                    },
                    Url = ""
                });
            }
        }

        private List<MenuItem> menuItems = new List<MenuItem>();
        //{
        //    new MenuItem
        //    {
        //        Text = "x",
        //        Id = ((int) RepairOrdersMenuId.Invoices).ToString(),
        //        Url = ""
        //    }
        //};
    }
}