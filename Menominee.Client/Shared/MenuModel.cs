using System.Collections.Generic;

namespace Menominee.Client.Shared
{
    public class MenuModel
    {
        public string Text { get; set; }
        public string Url { get; set; }
        public string Icon { get; set; }
        public List<MenuModel> Items { get; set; }
    }
}
