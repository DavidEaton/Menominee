using Menominee.Shared.Models;
using Menominee.Client.Services;
using Menominee.Common.Enums;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using System;
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
        public TelerikGrid<UserToRead> Grid { get; set; }
        public string Id { get; set; }
        private bool Editing { get; set; } = false;
        private bool Adding { get; set; } = false;
        protected override async Task OnInitializedAsync()
        {
            UsersList = (await UserDataService.GetAll())?.ToList();

            foreach (ShopRole item in Enum.GetValues(typeof(ShopRole)))
            {
                ShopRoleEnumData.Add(new ShopRoleEnumModel { DisplayText = item.ToString(), Value = item });
            }

            base.OnInitialized();
        }

        private RegisterUser registerUser { get; set; }

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
            await UserDataService.UpdateUser(registerUser, Id);
            await EndEditAsync();
        }

        protected async Task EndAddAsync()
        {
            Adding = false;
            Editing = false;
            UsersList = (await UserDataService.GetAll()).ToList();
        }

        protected async Task EndEditAsync()
        {
            Editing = false;
            UsersList = (await UserDataService.GetAll()).ToList();
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
            if (await UserDataService.Register(registerUser))
            {
                Adding = false;
                Editing = false;
                UsersList = (await UserDataService.GetAll()).ToList();
            }
        }

        List<ShopRoleEnumModel> ShopRoleEnumData { get; set; } = new List<ShopRoleEnumModel>();

    }
    public class ShopRoleEnumModel
    {
        public ShopRole Value { get; set; }
        public string DisplayText { get; set; }
    }
}
