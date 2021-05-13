using Menominee.Client.Services;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace Menominee.Client.Pages
{
    public partial class PersonsTotal : ComponentBase
    {
        [Inject]
        public IPersonDataService PersonsDataService { get; set; }
        public int Total { get; set; }
        protected override async Task OnInitializedAsync()
        {
            Total = await PersonsDataService.GetPersonsTotal();
        }

    }
}
