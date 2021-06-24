using CustomerVehicleManagement.Shared.Models;
using Menominee.Client.Services;
using Microsoft.AspNetCore.Components;
using SharedKernel.Enums;
using SharedKernel.Static;
using SharedKernel.ValueObjects;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Menominee.Client.Components
{
    public partial class PersonDetail : ComponentBase
    {
        [Inject]
        public IPersonDataService PersonDataService { get; set; }

        [Parameter]
        public int Id { get; set; }
        public PersonUpdateDto PersonUpdateDto { get; set; }
        public PersonReadDto Person { get; set; } = new PersonReadDto();
        public List<State> StatesList { get; set; } = States.ToList();

        // Screen state
        protected string Message = string.Empty;
        protected string StatusClass = string.Empty;
        protected bool Saved;

        protected override async Task OnParametersSetAsync()
        {
            //Saved = false;

            if (Id > 0)
                Person = await PersonDataService.GetPersonDetails(Id);

            PersonUpdateDto = new();
        }

        protected async Task HandleValidSubmit()
        {
            //Saved = false;

            //if (Person.Id == 0) // new
            //{
            //    var addedPerson = await PersonDataService.AddPerson(Person);
            //    if (addedPerson != null)
            //    {
            //        StatusClass = "alert-success";
            //        Message = "New person added successfully.";
            //        Saved = true;
            //    }
            //    else
            //    {
            //        StatusClass = "alert-danger";
            //        Message = "Something went wrong adding the new person. Error logged.";
            //        Saved = false;
            //    }
            //}
            ////else // existing
            ////{
            //await PersonDataService.UpdatePerson(Person);
            //StatusClass = "alert-success";
            //Message = "Person updated successfully.";
            //Saved = true;
            //}
        }

        protected void HandleInvalidSubmit()
        {
            StatusClass = "alert-danger";
            Message = "Please resolve validation errors.";
        }

        public void Close()
        {
            Saved = true;
            Message = string.Empty;
            StatusClass = string.Empty;
            Id = -1;
            StateHasChanged();
        }
    }
}
