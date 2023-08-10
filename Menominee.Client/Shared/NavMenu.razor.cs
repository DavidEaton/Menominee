using Menominee.Client.Shared.Models;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;

namespace Menominee.Client.Shared
{
    public partial class NavMenu : ComponentBase
    {
        public List<MenuModel> NotAuthorizedMenuData { get; set; }
        public List<MenuModel> AdminMenuData { get; set; }

        protected override void OnInitialized()
        {
            GetNotAuthorizedMenuData();
            GetAuthorizedMenuData();
        }

        public void GetNotAuthorizedMenuData()
        {
            NotAuthorizedMenuData = new List<MenuModel>()
            {
                new MenuModel()
                {
                    Text = "Home",
                    Url = "/",
                    Icon = "home"
                },
                new MenuModel()
                {
                    Text = "Products",
                    Url = "products",
                    Icon = "list-unordered"
                }
            };
        }
        public void GetAuthorizedMenuData()
        {

            // Get the currently logged-in user ShopRole
            // Build list of authorized links

            AdminMenuData = new List<MenuModel>()
            {
                new MenuModel()
                {
                    Text = "Home",
                    Url = "/",
                    Icon = "home"
                },
                new MenuModel()
                {
                    Text = "Customers",
                    Url = "customers",
                    Icon = "user"
                },
                new MenuModel()
                {
                    Text = "Persons",
                    Url = "persons",
                    Icon = "user"
                },
                new MenuModel()
                {
                    Text = "Businesses",
                    Url = "businesses",
                    Icon = "building-blocks"
                },
                new MenuModel()
                {
                    Text = "Users",
                    Url = "users",
                    Icon = "lock"
                },
                new MenuModel()
                {
                    Text = "Employees",
                    Url = "employees",
                    Icon = "anchor"
                },
                new MenuModel()
                {
                    Text = "Invoices",
                    Url = "/payables/invoices/listing",
                    Icon = "list"
                },
            };
        }
    }
}
