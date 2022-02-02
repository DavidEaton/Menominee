using Microsoft.AspNetCore.Components;

namespace Menominee.Client.Components.Shared
{
    public partial class ContentHeader
    {
        [Parameter]
        public string Header { get; set; }
        [Parameter]
        public int HeaderSize { get; set; } = 5;

    }
}
