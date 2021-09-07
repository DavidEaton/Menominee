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
        private const string UriSegment = "organizations";

        public OrganizationDataService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<OrganizationReadDto> AddOrganization(OrganizationAddDto organization)
        {
            var content = new StringContent(JsonSerializer.Serialize(organization), Encoding.UTF8, MediaType);
            var response = await httpClient.PostAsync(UriSegment, content);

            if (response.IsSuccessStatusCode)
            {
                return await JsonSerializer.DeserializeAsync<OrganizationReadDto>(await response.Content.ReadAsStreamAsync());
            }

            return null;
        }

        public async Task<IReadOnlyList<OrganizationInListDto>> GetAllOrganizations()
        {
            try
            {
                return await httpClient.GetFromJsonAsync<IReadOnlyList<OrganizationInListDto>>($"{UriSegment}/list");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Message :{ex.Message}");
            }

            return null;
        }

        public async Task<OrganizationReadDto> GetOrganizationDetails(int id)
        {
            try
            {
                return await httpClient.GetFromJsonAsync<OrganizationReadDto>(UriSegment + $"/{id}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Message :{ex.Message}");
            }
            return null;
        }
    }
}
