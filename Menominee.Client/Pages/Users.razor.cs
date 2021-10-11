using CustomerVehicleManagement.Shared.Models;
using Menominee.Client.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Menominee.Client.Pages
{
    public partial class Users : ComponentBase
    {
        [Inject]
        public IUserDataService UserDataService { get; set; }

        [Inject]
        public ILogger<Users> Logger { get; set; }

        public IReadOnlyList<UserToReadInList> UsersList;
        protected string SelectedId;

        protected override async Task OnInitializedAsync()
        {
            UsersList = (await UserDataService.GetAllUsers()).ToList();
        }
        private void SetSelectedId(string id)
        {
            SelectedId = id;
        }
    }
}
