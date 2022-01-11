using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace Menominee.Client.Components
{
    public partial class EditFormButtons : ComponentBase
    {
        [Parameter]
        public EventCallback CloseEventCallback { get; set; }

        [Parameter]
        public EventCallback SaveEventCallback { get; set; }

        private async Task CloseAsync()
        {
            await CloseEventCallback.InvokeAsync();
        }
        private async Task SaveAsync()
        {
            await SaveEventCallback.InvokeAsync();
        }
    }
}
