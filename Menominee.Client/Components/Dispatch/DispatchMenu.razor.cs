using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Navigations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Menominee.Client.Components.Dispatch
{
    public partial class DispatchMenu
    {
        [Inject]
        private NavigationManager navigationManager { get; set; }

        protected override void OnInitialized()
        {
#pragma warning disable BL0005            
            menuItems = new List<MenuItem>()
            {
                new MenuItem
                {
                    Text = "Technician",
                    Items = new List<MenuItem>
                    {
                        new MenuItem { Text= "All Technicians", HtmlAttributes=SubItemHtmlAttribute, Id="*" },
                        new MenuItem { Text= "101 - Bobby Brakedude", HtmlAttributes=SubItemHtmlAttribute, Id="101" },
                        new MenuItem { Text= "215 - Sammy Shocker", HtmlAttributes=SubItemHtmlAttribute, Id="215" },
                        new MenuItem { Text= "216 - Tony Tireman", HtmlAttributes=SubItemHtmlAttribute, Id="216" },
                        new MenuItem { Text= "385 - Ed Exhauster", HtmlAttributes=SubItemHtmlAttribute, Id="385" },
                        new MenuItem { Text= "447 - Alex Aligner", HtmlAttributes=SubItemHtmlAttribute, Id="447" }
                    },
                    HtmlAttributes=ItemHtmlAttribute
                },
                new MenuItem
                {
                    Text = "Advisor",
                    Items = new List<MenuItem>
                    {
                        new MenuItem { Text= "All Advisors", HtmlAttributes=SubItemHtmlAttribute, Id="*" },
                        new MenuItem { Text= "882 - Bill Bossman", HtmlAttributes=SubItemHtmlAttribute, Id="882" },
                        new MenuItem { Text= "937 - Levi Leaderman", HtmlAttributes=SubItemHtmlAttribute, Id="937" }
                    },
                    HtmlAttributes=ItemHtmlAttribute
                },
                new MenuItem
                {
                    Text = "Manager",
                    HtmlAttributes=ItemHtmlAttribute
                }
            };
#pragma warning restore BL0005            
        }

        public void OnItemSelected(string selectedItem)
        {
            if (selectedItem.Length > 0)
            {
                string url = "/dispatch";

                switch (selectedItem)
                {
                    case "placeholder":
                        navigationManager.NavigateTo($"{url}/placeholder");
                        break;
                    default:
                        navigationManager.NavigateTo(url);
                        break;
                }
            }
        }

        private List<MenuItem> menuItems = null;// new List<MenuItem>
        //{
        //    new MenuItem
        //    {
        //        Text = "Technician",
        //        Items = new List<MenuItem>
        //        {
        //            new MenuItem { Text= "All Technicians", HtmlAttributes=SubItemHtmlAttribute, Id="*" },
        //            new MenuItem { Text= "101 - Bobby Brakedude", HtmlAttributes=SubItemHtmlAttribute, Id="101" },
        //            new MenuItem { Text= "215 - Sammy Shocker", HtmlAttributes=SubItemHtmlAttribute, Id="215" },
        //            new MenuItem { Text= "216 - Tony Tireman", HtmlAttributes=SubItemHtmlAttribute, Id="216" },
        //            new MenuItem { Text= "385 - Ed Exhauster", HtmlAttributes=SubItemHtmlAttribute, Id="385" },
        //            new MenuItem { Text= "447 - Alex Aligner", HtmlAttributes=SubItemHtmlAttribute, Id="447" }
        //        },
        //        HtmlAttributes=ItemHtmlAttribute
        //    },
        //    new MenuItem
        //    {
        //        Text = "Advisor",
        //        Items = new List<MenuItem>
        //        {
        //            new MenuItem { Text= "All Advisors", HtmlAttributes=SubItemHtmlAttribute, Id="*" },
        //            new MenuItem { Text= "882 - Bill Bossman", HtmlAttributes=SubItemHtmlAttribute, Id="882" },
        //            new MenuItem { Text= "937 - Levi Leaderman", HtmlAttributes=SubItemHtmlAttribute, Id="937" }
        //        },
        //        HtmlAttributes=ItemHtmlAttribute
        //    },
        //    new MenuItem
        //    {
        //        Text = "Manager",
        //        HtmlAttributes=ItemHtmlAttribute
        //    }

        // };

        static Dictionary<string, object> SubItemHtmlAttribute = new Dictionary<string, object>()
        {
            {"class", "m-menu-sub-item" }
        };

        static Dictionary<string, object> ItemHtmlAttribute = new Dictionary<string, object>()
        {
            {"class", "m-menu-item" }
        };
    }
}
