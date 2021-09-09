using CustomerVehicleManagement.Shared.Models;
using Menominee.UiExperiments.Services;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Grids;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Menominee.UiExperiments.Pages
{
    public partial class Persons
    {
        [Inject]
        public IPersonDataService PersonsDataService { get; set; }
        public IReadOnlyList<PersonReadDto> PersonsList;
        public long SelectedId { get; set; }

        protected override async Task OnInitializedAsync()
        {
            PersonsList = (await PersonsDataService.GetAllPersons()).ToList();
        }

        public void RowSelectedHandler(RowSelectEventArgs<PersonReadDto> args)
        {
            SelectedId = args.Data.Id;
        }

    }
}
