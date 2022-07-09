using Menominee.Client.Shared;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telerik.Blazor.Components;

namespace Menominee.Client.Shared
{
    public partial class MainLayout
    {
        [Inject]
        public NavigationManager NavigationManager { get; set; }

        public bool DrawerExpanded { get; set; } = true;
        DrawerItem SelectedItem { get; set; } 
        TelerikDrawer<DrawerItem> DrawerRef { get; set; }
        public bool RepairOrderEditMenuVisible { get; set; } = false;

        private bool displayIsLarge = false;

        // in this sample we hardcode the existing pages, in your case you can
        // create the list based on your business logic (e.g., based on user roles/access)
        List<DrawerItem> NavigablePages { get; set; } = new List<DrawerItem>
        {
            new DrawerItem { ItemId = ModuleId.MainMenu,     Text = "Main Menu",     ElementId = String.Empty,            Url = String.Empty },
            new DrawerItem { ItemId = ModuleId.Home,         Text = "Home",          ElementId = "menuitem-home",         Url = "/",       Icon = "home" },
            new DrawerItem { ItemId = ModuleId.Dispatch,     Text = "Dispatch",      ElementId = "menuitem-dispatch",     Url = "dispatch", Icon = "support_agent" },
            new DrawerItem { ItemId = ModuleId.RepairOrders, Text = "Repair Orders", ElementId = "menuitem-repairorders", Url = "repairorders/worklog", Icon = "car_repair" },
            new DrawerItem { ItemId = ModuleId.Inspections,  Text = "Inspections",   ElementId = "menuitem-inspections",  Url = "inspections", Icon = "content_paste_search" },
            new DrawerItem { ItemId = ModuleId.Schedule,     Text = "Schedule",      ElementId = "menuitem-schedule",     Url = "schedule", Icon = "date_range" },
            new DrawerItem { ItemId = ModuleId.PartOrders,   Text = "Parts Orders",  ElementId = "menuitem-partorders",   Url = "partsorders", Icon = "shopping_cart" },
            new DrawerItem { ItemId = ModuleId.Inventory,    Text = "Inventory",     ElementId = "menuitem-inventory",    Url = "inventory", Icon = "warehouse" },
            new DrawerItem { ItemId = ModuleId.Reports,      Text = "Reports",       ElementId = "menuitem-reports",      Url = "reports", Icon = "bar_chart" },
            new DrawerItem { ItemId = ModuleId.Customers,    Text = "Customers",     ElementId = "menuitem-customers",    Url = "customers", Icon = "people" },
            new DrawerItem { ItemId = ModuleId.Receivables,  Text = "Receivables",   ElementId = "menuitem-receivables",  Url = "receivables", Icon = "savings" },
            new DrawerItem { ItemId = ModuleId.Payables,     Text = "Payables",      ElementId = "menuitem-payables",     Url = "payables", Icon = "payments" },
            new DrawerItem { ItemId = ModuleId.Employees,    Text = "Employees",     ElementId = "menuitem-employees",    Url = "employees", Icon = "assignment_ind" },
            new DrawerItem { ItemId = ModuleId.Settings,     Text = "Settings",      ElementId = "menuitem-settings",     Url = "settings", Icon = "settings" }
        };

        protected override void OnInitialized()
        {
            // pre-select the page the user lands on as the user clicks items,
            // the DOM changes only in the Body and so the selected item stays active
            var currPage = NavigationManager.Uri;
            DrawerItem ActivePage = NavigablePages.Where(p => p.Url.ToLowerInvariant() == GetCurrentPage().ToLowerInvariant()).FirstOrDefault();
            if (ActivePage != null)
            {
                SelectedItem = ActivePage;
            }

            base.OnInitialized();
        }

        public async Task ToggleDrawerAsync()
        {
            await DrawerRef.ToggleAsync();
        }

        public void ToggleDrawer()
        {
            DrawerExpanded = !DrawerExpanded;
        }

        private string GetSMSVisibleBreakpoint()
        {
            //Console.WriteLine(DrawerExpanded ? "(min-width: 630px)" : "(min-width: 500px)");
            return DrawerExpanded ? "(min-width: 630px)" : "(min-width: 500px)";
        }

        public void MediaQueryChange(bool matchesMediaQuery)
        {
            displayIsLarge = matchesMediaQuery;
            //Console.WriteLine(matchesMediaQuery);
        }

        public string GetCurrentPage()
        {
            string uriWithoutQueryString = NavigationManager.Uri.Split("?")[0];
            string currPage = uriWithoutQueryString.Substring(Math.Min(NavigationManager.Uri.Length, NavigationManager.BaseUri.Length));
            return string.IsNullOrWhiteSpace(currPage) ? "/" : currPage;
        }

        private async Task SelectAndNavigateAsync(DrawerItem item)
        {
            if (item != null)
            {
                if (item.ItemId == ModuleId.MainMenu)
                    await DrawerRef.ToggleAsync();
                else
                {
                    ToggleRepairOrderEditMenuDisplay(false);
                    SelectedItem = item;
                    await DrawerRef.CollapseAsync();
                    NavigationManager.NavigateTo(SelectedItem.Url);
                }
            }
        }

        public string GetSelectedItemClass(DrawerItem item)
        {
            if (SelectedItem == null || item.ItemId == ModuleId.MainMenu)
                return string.Empty;
            return SelectedItem.Text.ToLowerInvariant().Equals(item.Text.ToLowerInvariant()) ? "k-selected" : "";
        }
        
        private string MenuItemClass(ModuleId moduleId)
        {
            string _class = "k-icon material-icons ";
            if (SelectedItem?.ItemId == moduleId)
            {
                if (moduleId == ModuleId.Home)
                    _class += "mi-home";
                else if (moduleId == ModuleId.Dispatch)
                    _class += "mi-dispatch";
                else if (moduleId == ModuleId.RepairOrders)
                    _class += "mi-repairorders";
                else if (moduleId == ModuleId.Inspections)
                    _class += "mi-inspections";
                else if (moduleId == ModuleId.Schedule)
                    _class += "mi-schedule";
                else if (moduleId == ModuleId.PartOrders)
                    _class += "mi-partorders";
                else if (moduleId == ModuleId.Inventory)
                    _class += "mi-inventory";
                else if (moduleId == ModuleId.Reports)
                    _class += "mi-reports";
                else if (moduleId == ModuleId.Customers)
                    _class += "mi-customers";
                else if (moduleId == ModuleId.Receivables)
                    _class += "mi-receivables";
                else if (moduleId == ModuleId.Payables)
                    _class += "mi-payables";
                else if (moduleId == ModuleId.Employees)
                    _class += "mi-employees";
                else if (moduleId == ModuleId.Settings)
                    _class += "mi-settings";
            }
            return _class;
        }

        public string GetIconName(DrawerItem item)
        {
            string iconName;

            if (item.ItemId == ModuleId.MainMenu)
            {
                if (DrawerExpanded)
                    iconName = "navigate_before";
                else
                    iconName = "navigate_next";
            }
            else
                iconName = item.Icon;

            return iconName;
        }

        public void ToggleRepairOrderEditMenuDisplay(bool display)
        {
            RepairOrderEditMenuVisible = display;
            StateHasChanged();
        }

        //public class DrawerItem
        //{
        //    public string Text { get; set; }
        //    public string Url { get; set; }
        //    public string Icon { get; set; }
        //    public string IconClass { get; set; }
        //    public string ElementId { get; set; }
        //    public bool IsSeparator { get; set; }
        //    public ModuleId ItemId { get; set; }
        //}
    }
}
