using Menominee.Shared.Models.Persons;
using Microsoft.AspNetCore.Components;

namespace Menominee.Client.Components.Persons
{
    public partial class PersonNameEditor : ComponentBase
    {
        [Parameter] public PersonNameToWrite Name { get; set; }
    }
}
