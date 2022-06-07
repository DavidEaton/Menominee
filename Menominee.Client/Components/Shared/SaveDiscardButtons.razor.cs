using Microsoft.AspNetCore.Components;

namespace Menominee.Client.Components.Shared
{
    public partial class SaveDiscardButtons
    {
        [Parameter]
        public EventCallback OnSave { get; set; }
        [Parameter]
        public EventCallback OnDiscard { get; set; }
        [Parameter]
        public bool ShowDivider { get; set; } = true;
    }
}
