using Microsoft.AspNetCore.Components;
using System.Collections.Generic;

namespace Menominee.OrganizationDataContracts.Shared
{
    public partial class NavMenu : ComponentBase
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
                    Text = "Home",
                    Url = "/",
                    Icon = "home"
                },
                new MenuModel()
                {
                    Text = "Organizations",
                    Url = "organizations",
                    Icon = "building-blocks"
                },
            };
        }
        public class MenuModel
        {
            public string Text { get; set; }
            public string Url { get; set; }
            public string Icon { get; set; }
            public List<MenuModel> Items { get; set; }
        }
    }
}
