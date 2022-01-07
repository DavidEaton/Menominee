﻿using CustomerVehicleManagement.Shared.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Menominee.Client.Services
{
    public interface IUserDataService
    {
        Task<IReadOnlyList<UserToRead>> GetAll();
        Task<RegisterUserResult> Register(RegisterUser registerModel);

    }
}
