using Blazored.Toast.Services;
using CustomerVehicleManagement.Shared.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Menominee.Client.Services
{
    public class UserDataService : IUserDataService
    {
        private readonly HttpClient httpClient;
        private const string UriSegment = "api/user";
        private const string MediaType = "application/json";
        private readonly IToastService toastService;

        public UserDataService(HttpClient httpClient, IToastService toastService)
        {
            this.httpClient = httpClient;
            this.toastService = toastService;
        }

        public async Task<IReadOnlyList<UserToRead>> GetAll()
        {
            try
            {
                return await httpClient.GetFromJsonAsync<IReadOnlyList<UserToRead>>($"{UriSegment}");
            } 
            catch (Exception ex)
            {
                // TODO: log exception
            }

            return null;
        }

        public async Task<UserToRead> GetUser(string id)
        {
            try
            {
                return await httpClient.GetFromJsonAsync<UserToRead>(UriSegment + $"/{id}");
            }
            catch (Exception)
            {
                // TODO: log exception
            }
            return null;
        }

        public async Task<bool> Register(RegisterUser registerModel)
        {
            var content = new StringContent(JsonSerializer.Serialize(registerModel), Encoding.UTF8, MediaType);
            var response = await httpClient.PostAsync(UriSegment, content);

            if (response.IsSuccessStatusCode)
            {
                toastService.ShowSuccess($"{registerModel.Email} added successfully", "Added");
                return true;
            }

            toastService.ShowError($"{registerModel.Email} failed to add. {response.ReasonPhrase}.", "Add Failed");
            return false;
        }

        public async Task UpdateUser(RegisterUser registerUser, long id)
        {
            var content = new StringContent(JsonSerializer.Serialize(registerUser), Encoding.UTF8, MediaType);
            var response = await httpClient.PutAsync(UriSegment + $"/{id}", content);

            if (response.IsSuccessStatusCode)
            {
                toastService.ShowSuccess($"{registerUser.Email} updated successfully", "Saved");
                return;
            }

            toastService.ShowError($"{registerUser.Email} failed to update", "Save Failed");
        }

        public Task UpdateUser(RegisterUser registerUser, string id)
        {
            throw new NotImplementedException();
        }
    }
}
