using CustomerVehicleManagement.Shared.Models;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;

namespace Menominee.OrganizationDtos.Components
{
    public partial class EmailsEdit : ComponentBase
    {
        [Parameter]
        public IEnumerable<EmailUpdateDto> Emails { get; set; }
        protected void Add()
        {
            Console.WriteLine("Add called from EmailsEdit component");
        }

    }
}
