using Microsoft.AspNetCore.Components;

namespace Menominee.Client.Components
{
    public partial class EditFormButtons : ComponentBase
    {
        [Parameter]
        public EventCallback Close { get; set; }

        [Parameter]
        public EventCallback Save { get; set; }

        [Parameter]
        public bool EnableSave { get; set; }
    }
}
