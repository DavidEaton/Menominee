using CustomerVehicleManagement.Shared.Models;
using Menominee.Client.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Menominee.Client.Pages
{
    public partial class EmployeesDavid : ComponentBase
    {
        [Inject]
        public IEmployeeDataService EmployeeDataService { get; set; }

        [Inject]
        public ILogger<EmployeesDavid> Logger { get; set; }

        public IReadOnlyList<EmployeeToRead> EmployeesList;
        public long SelectedId { get; set; }

        protected override async Task OnInitializedAsync()
        {
            EmployeesList = (await EmployeeDataService.GetAllEmployees()).ToList();
        }

        private void SetSelectedId(long id)
        {
            SelectedId = id;
        }
    }
}
