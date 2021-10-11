using CustomerVehicleManagement.Shared.Models;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace Menominee.OrganizationDataContracts.Components
{
    public partial class OrganizationsIndexRow : ComponentBase
    {
        [Parameter]
        public OrganizationToReadInList Organization { get; set; }

        [Parameter]
        public EventCallback<long> OnEdit { get; set; }

        protected async Task EditAsync(long id)
        {
            await OnEdit.InvokeAsync(id);
        }
    }
}
