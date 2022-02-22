using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Navigations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telerik.Blazor.Components;

namespace Menominee.Client.Shared
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

        private ModuleId SelectedModule { get; set; } = ModuleId.Home;

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
            GotoModule(ModuleId.Home);
        }

        public void ShowDispatch()
        {
            GotoModule(ModuleId.Dispatch);
        }

        public void ShowRepairOrders()
        {
            GotoModule(ModuleId.RepairOrders);
        }

        public void ShowInspections()
        {
            GotoModule(ModuleId.Inspections);
        }

        public void ShowSchedule()
        {
            GotoModule(ModuleId.Schedule);
        }

        public void ShowPartOrders()
        {
            GotoModule(ModuleId.PartOrders);
        }

        public void ShowInventory()
        {
            GotoModule(ModuleId.Inventory);
        }

        public void ShowReports()
        {
            GotoModule(ModuleId.Reports);
        }

        public void ShowCustomers()
        {
            GotoModule(ModuleId.Customers);
        }

        public void ShowReceivables()
        {
            GotoModule(ModuleId.Receivables);
        }

        public void ShowPayables()
        {
            GotoModule(ModuleId.Payables);
        }

        public void ShowEmployees()
        {
            GotoModule(ModuleId.Employees);
        }

        public void ShowSettings()
        {
            GotoModule(ModuleId.Settings);
        }

        //public void ShowSchedule2()
        //{
        //    GotoModule("telerikschedule");
        //}

        public void GotoModule(ModuleId moduleId)
        {
            if (SidebarExpanded)
            {
                Toggle();
            }

            SelectedModule = moduleId;

            string url = string.Empty;

            switch (moduleId) 
            {
                case ModuleId.Home:
                    url = string.Empty;
                    break;
                case ModuleId.Dispatch:
                    url = "dispatch";
                    break;
                case ModuleId.RepairOrders:
                    url = "repairorders/worklog";
                    break;
                case ModuleId.Inspections:
                    url = "inspections";
                    break;
                case ModuleId.Schedule:
                    url = "schedule";
                    break;
                case ModuleId.PartOrders:
                    url = "partsorders";
                    break;
                case ModuleId.Inventory:
                    url = "inventory";
                    break;
                case ModuleId.Reports:
                    url = "reports";
                    break;
                case ModuleId.Customers:
                    url = "customers";
                    break;
                case ModuleId.Receivables:
                    url = "receivables";
                    break;
                case ModuleId.Payables:
                    url = "payables";
                    break;
                case ModuleId.Employees:
                    url = "employees";
                    break;
                case ModuleId.Settings:
                    url = "settings";
                    break;
                default:
                    url = string.Empty;
                    break;
            };

            navigationManager.NavigateTo($"/{url}");
        }

        private string MenuItemClass(ModuleId moduleId)
        {
            string _class = "sidebar-item";
            if (moduleId == ModuleId.Dispatch && SelectedModule == ModuleId.Dispatch)
                _class += " mi-dispatch";
            else if (moduleId == ModuleId.RepairOrders && SelectedModule == ModuleId.RepairOrders)
                _class += " mi-repairorders";
            else if (moduleId == ModuleId.Inspections && SelectedModule == ModuleId.Inspections)
                _class += " mi-inspections";
            else if (moduleId == ModuleId.Schedule && SelectedModule == ModuleId.Schedule)
                _class += " mi-schedule";
            else if (moduleId == ModuleId.PartOrders && SelectedModule == ModuleId.PartOrders)
                _class += " mi-partorders";
            else if (moduleId == ModuleId.Inventory && SelectedModule == ModuleId.Inventory)
                _class += " mi-inventory";
            else if (moduleId == ModuleId.Reports && SelectedModule == ModuleId.Reports)
                _class += " mi-reports";
            else if (moduleId == ModuleId.Customers && SelectedModule == ModuleId.Customers)
                _class += " mi-customers";
            else if (moduleId == ModuleId.Receivables && SelectedModule == ModuleId.Receivables)
                _class += " mi-receivables";
            else if (moduleId == ModuleId.Payables && SelectedModule == ModuleId.Payables)
                _class += " mi-payables";
            else if (moduleId == ModuleId.Employees && SelectedModule == ModuleId.Employees)
                _class += " mi-employees";
            else if (moduleId == ModuleId.Settings && SelectedModule == ModuleId.Settings)
                _class += " mi-settings";
            return _class;
        }
    }
}
