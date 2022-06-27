using Menominee.Client.Shared;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;

namespace Menominee.Client.Components.RepairOrders
{
    public partial class RepairOrdersMenu
    {
        [Inject]
        public NavigationManager navigationManager { get; set; }

        //[CascadingParameter(Name = "RepairOrderEditMenuVisible")] 
        [Parameter]
        public bool RepairOrderEditMenuVisible { get; set; } = false;

        //[Parameter] 
        //public Action<bool> OnSwapMenu { get; set; }

        //[CascadingParameter(Name = "MainLayout")]
        //MainLayout MainLayout { get; set; }

        //private void SwapMenu(bool showEditMenu)
        //{
        //    //RepairOrderEditMenuVisible = showEditMenu;
        //    //OnSwapMenu?.Invoke(showEditMenu);
        //    MainLayout?.ToggleRepairOrderEditMenuDisplay(showEditMenu);
        //}

        private static string ModuleUrl = "/repairorders";


        public void OnItemSelected(ModuleMenuItem selectedItem)
        {
        }

        protected override void OnParametersSet()
        {
            menuItems.Clear();

            //RepairOrderEditMenuVisible = mainLayout.RepairOrderEditMenuVisible;
            //if (MainLayout.RepairOrderEditMenuVisible)
            if (RepairOrderEditMenuVisible)
            {
                menuItems.Add(new ModuleMenuItem
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
                });

                menuItems.Add(new ModuleMenuItem
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
                });

                menuItems.Add(new ModuleMenuItem
                {
                    Text = "Inspection Log",
                    Id = (int)RepairOrderEditMenuId.InspectionLog,
                    Url = ""
                });

                menuItems.Add(new ModuleMenuItem
                {
                    Text = "Caller ID",
                    Id = (int)RepairOrderEditMenuId.CallerId,
                    Url = ""
                });
            }
            else
            {
                menuItems.Add(new ModuleMenuItem
                {
                    Text = "Repair Orders",
                    Id = (int)RepairOrdersMenuId.RepairOrders,
                    SubItems = new List<ModuleMenuItem>
                    {
                        new ModuleMenuItem { Text="Today's Repair Orders", Url=$"{ModuleUrl}/worklog", Id=(int)RepairOrdersMenuId.TodaysROs },
                        new ModuleMenuItem { Text="Waiting On Parts", Url=$"{ModuleUrl}/worklog", Id=(int)RepairOrdersMenuId.WaitingOnParts },
                        new ModuleMenuItem { Text="Ready For Pickup", Url=$"{ModuleUrl}/worklog", Id=(int)RepairOrdersMenuId.ReadyForPickup },
                        new ModuleMenuItem { Text="All Repair Orders", Url=$"{ModuleUrl}/worklog", Id=(int)RepairOrdersMenuId.AllROs }
                    },
                    Url = ""
                });

                menuItems.Add(new ModuleMenuItem
                {
                    Text = "Invoices",
                    Id = (int)RepairOrdersMenuId.Invoices,
                    SubItems = new List<ModuleMenuItem>
                    {
                        new ModuleMenuItem { Text="Unpaid Invoices", Url=$"{ModuleUrl}/worklog", Id=(int)RepairOrdersMenuId.UnpaidInvoices },
                        new ModuleMenuItem { Text="Today's Invoices", Url=$"{ModuleUrl}/worklog", Id=(int)RepairOrdersMenuId.TodaysInvoices },
                        new ModuleMenuItem { Text="All Invoices", Url=$"{ModuleUrl}/worklog", Id=(int)RepairOrdersMenuId.AllInvoices }
                    },
                    Url = ""
                });
            }
        }

        private List<ModuleMenuItem> menuItems = new List<ModuleMenuItem>
        {
            new ModuleMenuItem
            {
                Text = "x",
                Id = (int) RepairOrdersMenuId.Invoices,
                Url = ""
            }
        };

        //{
        //    new ModuleMenuItem
        //    {
        //        Text = "Repair Orders",
        //        Id = (int) RepairOrdersMenuId.RepairOrders,
        //        SubItems = new List<ModuleMenuItem>
        //        {
        //            new ModuleMenuItem { Text="Today's Repair Orders", Url=$"{ModuleUrl}/worklog", Id=(int)RepairOrdersMenuId.TodaysROs },
        //            new ModuleMenuItem { Text="Waiting On Parts", Url=$"{ModuleUrl}/worklog", Id=(int)RepairOrdersMenuId.WaitingOnParts },
        //            new ModuleMenuItem { Text="Ready For Pickup", Url=$"{ModuleUrl}/worklog", Id=(int)RepairOrdersMenuId.ReadyForPickup },
        //            new ModuleMenuItem { Text="All Repair Orders", Url=$"{ModuleUrl}/worklog", Id=(int)RepairOrdersMenuId.AllROs }
        //        },
        //        Url = ""
        //    },
        //    new ModuleMenuItem
        //    {
        //        Text = "Invoices",
        //        Id = (int) RepairOrdersMenuId.Invoices,
        //        SubItems = new List<ModuleMenuItem>
        //        {
        //            new ModuleMenuItem { Text="Unpaid Invoices", Url=$"{ModuleUrl}/worklog", Id=(int)RepairOrdersMenuId.UnpaidInvoices },
        //            new ModuleMenuItem { Text="Today's Invoices", Url=$"{ModuleUrl}/worklog", Id=(int)RepairOrdersMenuId.TodaysInvoices },
        //            new ModuleMenuItem { Text="All Invoices", Url=$"{ModuleUrl}/worklog", Id=(int)RepairOrdersMenuId.AllInvoices }
        //        },
        //        Url = ""
        //    }
        // };
    }
}