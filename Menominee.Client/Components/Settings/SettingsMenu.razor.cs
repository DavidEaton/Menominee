using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Navigations;
using System.Collections.Generic;

namespace Menominee.Client.Components.Settings
{
    public partial class SettingsMenu
    {
        [Inject]
        private NavigationManager navigationManager { get; set; }

        public void OnItemSelected(string selectedItem)
        {
            if (selectedItem.Length > 0)
            {
                string url = "/settings";

                switch (selectedItem)
                {
                    case "companyInfo":
                        navigationManager.NavigateTo($"{url}/companyinfo");
                        break;
                    case "customSettings":
                        navigationManager.NavigateTo($"{url}/customsettings");
                        break;
                    case "shopInfo":
                        navigationManager.NavigateTo($"{url}/shopinfo");
                        break;
                    case "salesDepartments":
                        navigationManager.NavigateTo($"{url}/salesdepartments");
                        break;
                    case "orderingAndCatalogs":
                        navigationManager.NavigateTo($"{url}/orderingcatalogs");
                        break;
                    case "userInfo":
                        navigationManager.NavigateTo($"{url}/userinfo");
                        break;
                    default:
                        navigationManager.NavigateTo(url);
                        break;
                }
            }
        }

        private List<MenuItem> menuItems = new List<MenuItem>
        {
            new MenuItem
            {
                Text = "General",
                //IconCss = "em-icons e-file",
                Items = new List<MenuItem>
                {
                    new MenuItem { Text= "Company Information", HtmlAttributes=SubItemHtmlAttribute, Id="companyInfo"/*, ChildContent=ChildContent*/},
                    new MenuItem { Text= "Custom Settings", HtmlAttributes=SubItemHtmlAttribute, Id="customSettings"}
                },
                HtmlAttributes=ItemHtmlAttribute
            },
            new
            MenuItem {
                Text = "Shop",
                //IconCss = "em-icons e-edit",
                Items = new List<MenuItem>
                {
                    new MenuItem { Text= "Shop Information", HtmlAttributes=SubItemHtmlAttribute, Id="shopInfo" },
                    new MenuItem { Text= "Sales Departments", HtmlAttributes=SubItemHtmlAttribute, Id="salesDepartments" },
                    new MenuItem { Separator= true, HtmlAttributes=SubItemHtmlAttribute },
                    new MenuItem { Text= "Ordering & Catalogs", HtmlAttributes=SubItemHtmlAttribute, Id="orderingAndCatalogs" }
                },
                HtmlAttributes=ItemHtmlAttribute
            },
            new MenuItem
            {
                Text = "User",
                Items = new List<MenuItem>
                {
                    new MenuItem { Text= "User Information", HtmlAttributes=SubItemHtmlAttribute, Id="userInfo" }
                },
                HtmlAttributes=ItemHtmlAttribute
            }
         };

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
