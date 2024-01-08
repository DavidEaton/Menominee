using Menominee.Shared.Models.Persons;
using Microsoft.AspNetCore.Components;

namespace Menominee.Client.Components.Persons
{
    public partial class PersonEditor : ComponentBase
    {
        [Parameter] public PersonToWrite? Person { get; set; } = new PersonToWrite();

        private void TrimPersonNotes()
        {
            Person.Notes = Person.Notes?.Trim();
        }

        [Parameter] public bool IncludeNotes { get; set; } = true;
    }
}
