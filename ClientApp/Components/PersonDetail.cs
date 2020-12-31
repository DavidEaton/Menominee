using Microsoft.AspNetCore.Components;
using ClientApp.Services;
using System.Threading.Tasks;
using Migrations.Core.Entities;
using Migrations.Core.ValueObjects;

namespace ClientApp.Components
{
    public partial class PersonDetail : ComponentBase
    {
        [Inject]
        public IPersonDataService PersonDataService { get; set; }

        [Parameter]
        public int Id { get; set; }
        public Person Person { get; set; }

        // Screen state
        protected string Message = string.Empty;
        protected string StatusClass = string.Empty;
        protected bool Saved;

        protected override async Task OnParametersSetAsync()
        {
            Saved = false;

            if (Id == 0)
                Person = new Person(new PersonName("", ""));
            else
                Person = await PersonDataService.GetPerson(Id);
        }

        protected async Task HandleValidSubmit()
        {
            Saved = false;

            if (Person.Id == 0) // new
            {
                var addedPerson = await PersonDataService.AddPerson(Person);
                if (addedPerson != null)
                {
                    StatusClass = "alert-success";
                    Message = "New person added successfully.";
                    Saved = true;
                }
                else
                {
                    StatusClass = "alert-danger";
                    Message = "Something went wrong adding the new person. Error logged.";
                    Saved = false;
                }
            }
            else // existing
            {
                await PersonDataService.UpdatePerson(Person);
                StatusClass = "alert-success";
                Message = "Person updated successfully.";
                Saved = true;
            }
        }

        protected void HandleInvalidSubmit()
        {
            StatusClass = "alert-danger";
            Message = "Please resolve validation errors.";
        }

        protected void Close()
        {
            Saved = true;
            Message = string.Empty;
            StatusClass = string.Empty;
            //Id = -1;
            //Person.Id = 0;
            Person = null;
            StateHasChanged();
        }
    }
}
