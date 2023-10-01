using CSharpFunctionalExtensions;
using Menominee.Client.Services;
using Menominee.Shared.Models.Employees;
using Microsoft.AspNetCore.Components;

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
            await EmployeeDataService.GetAllAsync()
                .Match(
                    success => EmployeesList = success,
                    failure => Logger.LogError(failure)
            );
        }

        private void SetSelectedId(long id)
        {
            SelectedId = id;
        }
    }
}
