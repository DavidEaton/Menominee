using Menominee.Client.Shared.Models;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;

namespace Menominee.Client.Shared
{
    public partial class Header : ComponentBase
    {
        public List<MenuModel> MenuData { get; set; }

        protected override void OnInitialized()
        {
            GenerateMenuData();
        }

        public void GenerateMenuData()
        {
            MenuData = new List<MenuModel>()
            {
                new MenuModel()
                {
                    Text = "Point of Sale",
                    Url = "/",
                    Icon = "cart"
                },
                new MenuModel()
                {
                    Text = "Sales",
                    Url = "/",
                    Icon = "graph"
                },
            };
        }
    }
}
