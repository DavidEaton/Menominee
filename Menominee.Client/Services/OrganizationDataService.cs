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
    public class OrganizationDataService : IOrganizationDataService
    {
        private readonly HttpClient httpClient;
        private const string MediaType = "application/json";
        private const string UriSegment = "api/organizations";

        public OrganizationDataService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<OrganizationToRead> AddOrganization(OrganizationToWrite organization)
        {
            var content = new StringContent(JsonSerializer.Serialize(organization), Encoding.UTF8, MediaType);
            var response = await httpClient.PostAsync(UriSegment, content);

            if (response.IsSuccessStatusCode)
            {
                return await JsonSerializer.DeserializeAsync<OrganizationToRead>(await response.Content.ReadAsStreamAsync());
            }

            return null;
        }

        public async Task<IReadOnlyList<OrganizationToReadInList>> GetAllOrganizations()
        {
            try
            {
                return await httpClient.GetFromJsonAsync<IReadOnlyList<OrganizationToReadInList>>($"{UriSegment}/list");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Message :{ex.Message}");
            }

            return null;
        }

        public async Task<OrganizationToRead> GetOrganizationDetails(long id)
        {
            try
            {
                return await httpClient.GetFromJsonAsync<OrganizationToRead>(UriSegment + $"/{id}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Message :{ex.Message}");
            }
            return null;
        }
    }
}
