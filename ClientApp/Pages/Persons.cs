using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using ClientApp.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SharedKernel;
using CustomerVehicleManagement.Domain.Entities;

namespace ClientApp.Pages
{
    public partial class Persons : ComponentBase
    {
        public IEnumerable<Person> PersonsList;

        [Inject]
        public IPersonDataService PersonsDataService { get; set; }
        
        [Inject]
        public ILogger<Persons> Logger { get; set; }

        public int SelectedId { get; set; }
        public Tenant Tenant { get; set; }

        //[Parameter]
        //public RenderFragment ChildContent { get; set; }
        //protected AddPersonDialog AddPersonDialog { get; set; }
        protected override async Task OnInitializedAsync()
        {
            PersonsList = (await PersonsDataService.GetAllPersons()).ToList();
            PersonsList = FormatPersonData(PersonsList);
            Logger.LogInformation("Persons.OnInitializedAsync()");



        }

        private IEnumerable<Person> FormatPersonData(IEnumerable<Person> persons)
        {
            foreach (var person in persons)
            {
                //if (person.PhonePrimary != null)
                //    person.PhonePrimary = Regex.Replace(person.PhonePrimary, @"(\d{3})(\d{3})(\d{4})", "($1) $2-$3");
                //if (person.PhoneSecondary != null)
                //    person.PhoneSecondary = Regex.Replace(person.PhoneSecondary, @"(\d{3})(\d{3})(\d{4})", "($1) $2-$3");
            }

            return persons;
        }

        protected void QuickAddPerson()
        {
            //AddPersonDialog.Show();
        }

        public async void AddPersonDialog_OnDialogClose()
        {
            PersonsList = (await PersonsDataService.GetAllPersons()).ToList();
            StateHasChanged();
        }
        private void SetSelectedId(int Id)
        {
            SelectedId = Id;
        }
    }
}
