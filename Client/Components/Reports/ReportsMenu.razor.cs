using MenomineePlayWASM.Client.Components.Shared;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MenomineePlayWASM.Client.Components.Reports
{
    public partial class ReportsMenu
    {
        [Inject]
        private NavigationManager navigationManager { get; set; }

        public List<ModuleMenuItem> MenuItems { get; set; }

        protected override void OnInitialized()
        {
            MenuItems = new List<ModuleMenuItem>()
        {
            new ModuleMenuItem()
            {
                Text = "Sales",
                SubItems = new List<ModuleMenuItem>()
                {
                    new ModuleMenuItem()
                    {
                        Text = "Report 1"
                    },
                    new ModuleMenuItem()
                    {
                        Text = "Report 2"
                    },
                    new ModuleMenuItem()
                    {
                        Text = "Report 3"
                    },
                }
            },
            new ModuleMenuItem()
            {
                Text = "Customers",
                SubItems = new List<ModuleMenuItem>()
                {
                    new ModuleMenuItem()
                    {
                        Text = "Report 1"
                    },
                    new ModuleMenuItem()
                    {
                        Text = "Report 2"
                    },
                    new ModuleMenuItem()
                    {
                        Text = "Report 3"
                    },
                }
            },
            new ModuleMenuItem()
            {
                Text = "Another Thing 1",
            },
            new ModuleMenuItem()
            {
                Text = "Another Thing 2",
            },
            new ModuleMenuItem()
            {
                Text = "Another Thing 3",
            },
            new ModuleMenuItem()
            {
                Text = "Another Thing 4",
            },
            new ModuleMenuItem()
            {
                Text = "Another Thing 5",
            }
        };

            base.OnInitialized();
        }

        public void OnItemSelected(ModuleMenuItem item)
        {

        }
    }
}
