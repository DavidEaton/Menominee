using CustomerVehicleManagement.Shared.Models;
using Menominee.Client.Components;
using Menominee.Client.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using SharedKernel.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Menominee.Client.Pages
{
    public partial class Persons : ComponentBase
    {
        [Inject]
        public IPersonDataService PersonDataService { get; set; }

        [Inject]
        public ILogger<Persons> Logger { get; set; }

        public IReadOnlyList<PersonToReadInList> PersonsList;
        public long SelectedId { get; set; }
        public Tenant Tenant { get; set; }

        protected AddPersonDialog AddPersonDialog { get; set; }
        //protected PersonDetail PersonDetail { get; set; }
        protected override async Task OnInitializedAsync()
        {
            PersonsList = (await PersonDataService.GetAllPersons()).ToList();
        }

        protected void AddPerson()
        {
            //PersonDetail?.Close();
            AddPersonDialog.Show();
        }

        public async void AddPersonDialog_OnDialogClose()
        {
            PersonsList = (await PersonDataService.GetAllPersons()).ToList();
            StateHasChanged();
        }
        private void SetSelectedId(long id)
        {
            SelectedId = id;
        }
    }

}
