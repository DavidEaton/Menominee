using CustomerVehicleManagement.Shared.Models;
using Menominee.Client.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telerik.Blazor.Components;

namespace Menominee.Client.Pages
{
    public partial class Users : ComponentBase
    {
        [Inject]
        public IUserDataService UserDataService { get; set; }

        [Inject]
        public ILogger<Users> Logger { get; set; }

        public IReadOnlyList<UserToRead> UsersList;
        public TelerikGrid<OrganizationToReadInList> Grid { get; set; }
        public long Id { get; set; }
        private string TenantName { get; set; } = "Jane's Automotive";
        private bool Editing { get; set; } = false;
        private bool Adding { get; set; } = false;
        protected override async Task OnInitializedAsync()
        {
            UsersList = (await UserDataService.GetAll()).ToList();
            //TenantName = Context.User.First(claim => claim.Type == "tenantName").Value;
        }

        private RegisterUser registerUser { get; set; }

        private void Add()
        {
            Adding = true;
            UsersList = null;
            registerUser = new();
        }

        private async Task HandleRegistration()
        {
            registerUser.ShopRole = "Technician";
            var result = await UserDataService.Register(registerUser);

            if (result.Successful)
            {
                Adding = false;
                Editing = false;
                UsersList = (await UserDataService.GetAll()).ToList();
            }
        }
    }
}
