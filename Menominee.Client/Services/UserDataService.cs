using CustomerVehicleManagement.Shared.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Menominee.Client.Services
{
    public class UserDataService : IUserDataService
    {
        private readonly HttpClient httpClient;
        private const string UriSegment = "users";

        public UserDataService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<IReadOnlyList<UserToRead>> GetAllUsers()
        {
            try
            {
                return await httpClient.GetFromJsonAsync<IReadOnlyList<UserToRead>>($"{UriSegment}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Message :{ex.Message}");
            }

            return null;
        }
    }
}
