using Microsoft.AspNetCore.Components;
using System.Collections.Generic;

namespace Menominee.Client.Shared
{
    public partial class NavMenu : ComponentBase
    {
        public List<MenuModel> NotAuthorizedMenuData { get; set; }
        public List<MenuModel> AuthorizedMenuData { get; set; }

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
                    Url = "/",
                    Icon = "list-unordered"
                },
                new MenuModel()
                {
                    Text = "About",
                    Url = "/",
                    Icon = "info-circle"
                },
            };
        }
        public void GetAuthorizedMenuData()
        {
            AuthorizedMenuData = new List<MenuModel>()
            {
                new MenuModel()
                {
                    Text = "Home",
                    Url = "/",
                    Icon = "home"
                },
                new MenuModel()
                {
                    Text = "Persons",
                    Url = "persons",
                    Icon = "user"
                },
                new MenuModel()
                {
                    Text = "Organizations",
                    Url = "organizations",
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
                    Text = "Technicians",
                    Url = "technicians",
                    Icon = "wrench"
                },
                new MenuModel()
                {
                    Text = "Employees",
                    Url = "employees",
                    Icon = "anchor"
                }
            };
        }
    }
}
