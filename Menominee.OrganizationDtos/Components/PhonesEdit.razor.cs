using CustomerVehicleManagement.Shared.Models;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;

namespace Menominee.OrganizationDtos.Components
{
    public partial class PhonesEdit : ComponentBase
    {
        [Parameter]
        public IEnumerable<PhoneUpdateDto> Phones { get; set; }
        protected void Add()
        {
            Console.WriteLine("Add called from PhonesEdit component");
        }

    }
}
