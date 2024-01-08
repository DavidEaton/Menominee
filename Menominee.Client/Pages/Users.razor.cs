using Menominee.Client.Services;
using Menominee.Domain.Enums;
using Menominee.Shared.Models.Users;
using Microsoft.AspNetCore.Components;
using Telerik.Blazor.Components;

namespace Menominee.Client.Pages
{
    public partial class Users : ComponentBase
    {
        [Inject]
        public IUserDataService UserDataService { get; set; }

        [Inject]
        public ILogger<Users> Logger { get; set; }

        public IReadOnlyList<UserResponse> UsersList;
        public TelerikGrid<UserResponse> Grid { get; set; }
        public string Id { get; set; }
        private bool Editing { get; set; } = false;
        private bool Adding { get; set; } = false;
        protected override async Task OnInitializedAsync()
        {
            await GetUsers();

            foreach (ShopRole item in Enum.GetValues(typeof(ShopRole)))
            {
                ShopRoleEnumData.Add(new ShopRoleEnumModel { DisplayText = item.ToString(), Value = item });
            }

            base.OnInitialized();
        }

        private RegisterUserRequest registerUser { get; set; }

        private void Add()
        {
            Adding = true;
            UsersList = null;
            registerUser = new();
        }

        private async Task EditAsync(GridRowClickEventArgs args)
        {
            //var user = args.Item as UserToRead;
            //Id = user.Id;

            //registerUser = new RegisterUser
            //{
            //    Id = user.Id,
            //    Email = user.Email,
            //    ShopRole = user.ShopRole
            //};

            //Editing = true;
            //UsersList = null;
        }



        protected async Task HandleAddSubmit()
        {
            await HandleRegistration();
        }

        protected async Task HandleEditSubmit()
        {
            await UserDataService.UpdateAsync(registerUser);
            await EndEditAsync();
        }

        protected async Task EndAddAsync()
        {
            Adding = false;
            Editing = false;
            await GetUsers();
        }

        private async Task GetUsers()
        {
            var result = await UserDataService.GetAllAsync();

            if (result.IsSuccess)
                UsersList = result.Value.ToList();

            if (result.IsFailure)
            {
                // TODO: notify user
                Logger.LogError(result.Error);
            }
        }

        protected async Task EndEditAsync()
        {
            Editing = false;
            await GetUsers();
        }

        protected async Task SubmitHandlerAsync()
        {
            if (Adding)
                await HandleAddSubmit();

            if (Editing)
                await HandleEditSubmit();
        }

        private async Task HandleRegistration()
        {
            var result = await UserDataService.RegisterAsync(registerUser);

            if (result.IsSuccess)
            {
                Adding = false;
                Editing = false;
                await GetUsers();
            }

            if (result.IsFailure)
                Logger.LogError(result.Error);
        }

        List<ShopRoleEnumModel> ShopRoleEnumData { get; set; } = new List<ShopRoleEnumModel>();

    }
    public class ShopRoleEnumModel
    {
        public ShopRole Value { get; set; }
        public string DisplayText { get; set; }
    }
}
