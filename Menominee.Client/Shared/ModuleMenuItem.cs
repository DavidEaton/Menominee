using System.Collections.Generic;

namespace Menominee.Client.Shared
{
    public class ModuleMenuItem
    {
        public string Text { get; set; }
        public string Url { get; set; }
        public int Id { get; set; }
        public bool Expanded { get; set; }
        public int Level { get; set; }
        public bool Separator { get; set; }
        public List<ModuleMenuItem> SubItems { get; set; }
    }
}
