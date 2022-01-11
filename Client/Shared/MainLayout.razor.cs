using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Navigations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telerik.Blazor.Components;

namespace MenomineePlayWASM.Client.Shared
{
    public partial class MainLayout
    {
        SfSidebar Sidebar;
        public bool SidebarExpanded = false;

        [Inject]
        private NavigationManager navigationManager { get; set; }

        //TelerikDrawer<DrawerItem> DrawerRef { get; set; }
        //DrawerItem SelectedItem { get; set; }
        //IEnumerable<DrawerItem> DrawerItems { get; set; } =
        //    new List<DrawerItem>
        //    {
        //        new DrawerItem { Text = "Home", Icon = "", Url = "/" },
        //        new DrawerItem { Text = "Dispatch Board", Icon = "", Url = "/" },
        //        new DrawerItem { Text = "Tickets", Icon = "", Url = "/" },
        //        new DrawerItem { Text = "Appointments", Icon = "", Url = "schedule" },
        //        new DrawerItem { Text = "Appointments 2", Icon = "", Url = "telerikschedule" },
        //        new DrawerItem { Text = "Reports", Icon = "", Url = "/" },
        //        new DrawerItem { Text = "Inventory", Icon = "", Url = "inventory" },
        //        new DrawerItem { Text = "Customers", Icon = "", Url = "customers" },
        //        new DrawerItem { Text = "Receivables", Icon = "", Url = "receivables" },
        //        new DrawerItem { Text = "Payables", Icon = "", Url = "payables" },
        //        new DrawerItem { Text = "Settings", Icon = "", Url = "settings" }
        //    };

        //public class DrawerItem
        //{
        //    public string Text { get; set; }
        //    public string Icon { get; set; }
        //    public string Url { get; set; }
        //}


        Dictionary<string, object> HtmlAttribute = new Dictionary<string, object>()
        {
            {"class", "dockSidebar" }
        };

        public void Toggle()
        {
            SidebarExpanded = !SidebarExpanded;
        }

        public void ShowHome()
        {
            GotoModule("");
        }

        public void ShowRepairOrders()
        {
            GotoModule("repairorders");
        }

        public void ShowPartOrders()
        {
            GotoModule("partorders");
        }

        public void ShowInventory()
        {
            GotoModule("inventory");
        }

        public void ShowCustomers()
        {
            GotoModule("customers");
        }

        public void ShowSchedule()
        {
            GotoModule("schedule");
        }

        public void ShowSchedule2()
        {
            GotoModule("telerikschedule");
        }

        public void ShowReports()
        {
            GotoModule("reports");
        }

        public void ShowSettings()
        {
            GotoModule("settings");
        }

        public void ShowPayables()
        {
            GotoModule("payables");
        }

        public void ShowReceivables()
        {
            GotoModule("receivables");
        }

        private void GotoModule(string url)
        {
            if (SidebarExpanded)
            {
                Toggle();
            }

            navigationManager.NavigateTo($"/{url}");
        }
    }
}
