using Menominee.Client.Shared;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Navigations;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Menominee.Client.Components.Shared
{
    public partial class ModuleMenu
    {
        [Parameter]
        public string ModuleName { get; set; }

        [Parameter]
        public List<MenuItem> MenuItems { get; set; }

        [Parameter]
        public EventCallback<string> OnItemSelected { get; set; }

        [Parameter]
        public string ModuleIconCss { get; set; } = "m-empty-icon";

        [Parameter]
        public ModuleId ModuleId { get; set; }

        private async Task OnMenuItemSelected(MenuEventArgs<MenuItem> args)
        {
            await OnItemSelected.InvokeAsync(args.Item.Id);
        }

        private string ModuleHeaderClass()
        {
            string _class = "sb-header e-view";
            //if (ModuleId == ModuleId.Dispatch)
            //    _class += " mi-dispatch";
            //else if (ModuleId == ModuleId.RepairOrders)
            //    _class += " mi-repairorders";
            //else if (ModuleId == ModuleId.Inspections)
            //    _class += " mi-inspections";
            //else if (ModuleId == ModuleId.Schedule)
            //    _class += " mi-schedule";
            //else if (ModuleId == ModuleId.PartOrders)
            //    _class += " mi-partorders";
            //else if (ModuleId == ModuleId.Inventory)
            //    _class += " mi-inventory";
            //else if (ModuleId == ModuleId.Reports)
            //    _class += " mi-reports";
            //else if (ModuleId == ModuleId.Customers)
            //    _class += " mi-customers";
            //else if (ModuleId == ModuleId.Receivables)
            //    _class += " mi-receivables";
            //else if (ModuleId == ModuleId.Payables)
            //    _class += " mi-payables";
            //else if (ModuleId == ModuleId.Employees)
            //    _class += " mi-employees";
            //else if (ModuleId == ModuleId.Settings)
            //    _class += " mi-settings";
            return _class;
        }

    }
}
