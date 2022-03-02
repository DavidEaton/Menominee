using CustomerVehicleManagement.Shared.Models;
using Menominee.Common.Enums;
using Microsoft.AspNetCore.Components;

namespace Menominee.Client.Components.Persons
{
    public partial class PersonNameEditor : ComponentBase
    {
        [Parameter]
        public PersonNameToWrite PersonName { get; set; }

        //[Parameter]
        //public FormMode FormMode { get; set; }

        //private PersonNameToWrite personNameOriginal;

        //protected override void OnAfterRender(bool firstRender)
        //{
        //    if (firstRender)
        //    {
        //        personNameOriginal = new PersonNameToWrite
        //        {
        //            FirstName = PersonName.FirstName,
        //            MiddleName = PersonName.MiddleName,
        //            LastName = PersonName.LastName
        //        };
        //    }
        //}

        //public void Reset()
        //{
        //    PersonName.FirstName = personNameOriginal.FirstName;
        //    PersonName.MiddleName = personNameOriginal.MiddleName;
        //    PersonName.LastName = personNameOriginal.LastName;
        //}
    }
}
