using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace Menominee.OrganizationDataContracts.Components
{
    public partial class EditFormButtons : ComponentBase
    {
        [Parameter]
        public EventCallback CloseEventCallback { get; set; }

        private async Task CloseAsync()
        {
            await CloseEventCallback.InvokeAsync();
        }
    }
}
