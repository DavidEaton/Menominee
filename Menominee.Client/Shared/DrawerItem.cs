namespace Menominee.Client.Shared
{
    public class DrawerItem
    {
        public string Text { get; set; }
        public string Url { get; set; }
        public string Icon { get; set; }
        public string IconClass { get; set; }
        public string ElementId { get; set; }
        public bool IsSeparator { get; set; }
        public ModuleId ItemId { get; set; }
    }
}
