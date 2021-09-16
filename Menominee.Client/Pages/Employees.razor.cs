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
    public partial class Employees : ComponentBase
    {
        [Inject]
        public IEmployeeDataService EmployeeDataService { get; set; }

        [Inject]
        public ILogger<Employees> Logger { get; set; }

        public IReadOnlyList<EmployeeReadDto> EmployeesList;
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
