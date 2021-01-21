using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Client.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SharedKernel;
using Client.Components;
using Client.Models;

namespace Client.Pages
{
    public partial class Persons : ComponentBase
    {
        public IEnumerable<PersonFlatDto> PersonsList;

        [Inject]
        public IPersonDataService PersonsDataService { get; set; }
        
        [Inject]
        public ILogger<Persons> Logger { get; set; }

        public int SelectedId { get; set; }
        public Tenant Tenant { get; set; }

        //[Parameter]
        //public RenderFragment ChildContent { get; set; }
        protected AddPersonDialog AddPersonDialog { get; set; }
        protected override async Task OnInitializedAsync()
        {
            Logger.LogInformation("Persons.OnInitializedAsync()");
            PersonsList = (await PersonsDataService.GetAllPersons()).ToList();
        }

        protected void QuickAddPerson()
        {
            AddPersonDialog.Show();
        }

        public async void AddPersonDialog_OnDialogClose()
        {
            PersonsList = (await PersonsDataService.GetAllPersons()).ToList();
            StateHasChanged();
        }
        private void SetSelectedId(PersonFlatDto selected)
        {
            SelectedId = selected.Id;
        }
    }
}
