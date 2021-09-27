using CustomerVehicleManagement.Shared.Models;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace Menominee.OrganizationDtos.Components
{
    public partial class AddressAdd : ComponentBase
    {
        public AddressAddDto Address { get; set; }

        protected override Task OnInitializedAsync()
        {

            return base.OnInitializedAsync();
        }
    }
}
