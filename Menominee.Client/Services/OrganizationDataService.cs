using Menominee.Shared.Models;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Blazored.Toast.Services;
using Menominee.Shared.Models.Organizations;

namespace Menominee.Client.Services
{
    public class OrganizationDataService : IOrganizationDataService
    {
        private readonly HttpClient httpClient;
        private readonly ILogger<OrganizationDataService> logger;
        private readonly IToastService toastService;
        private const string MediaType = "application/json";
        private const string UriSegment = "api/organizations";

        public OrganizationDataService(HttpClient httpClient, ILogger<OrganizationDataService> logger, IToastService toastService)
        {
            this.httpClient = httpClient;
            this.logger = logger;
            this.toastService = toastService;
        }

        public async Task<OrganizationToRead> AddOrganization(OrganizationToWrite organization)
        {
            var content = new StringContent(JsonSerializer.Serialize(organization), Encoding.UTF8, MediaType);
            var response = await httpClient.PostAsync(UriSegment, content);

            if (response.IsSuccessStatusCode)
            {
                toastService.ShowSuccess($"{organization.Name} added successfully", "Added");
                return await JsonSerializer.DeserializeAsync<OrganizationToRead>(await response.Content.ReadAsStreamAsync());
            }

            toastService.ShowError($"{organization.Name} failed to add. {response.ReasonPhrase}.", "Add Failed");

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
                logger.LogError(ex, "Failed to get all organizations");
            }

            return null;
        }

        public async Task<OrganizationToRead> GetOrganization(long id)
        {
            try
            {
                return await httpClient.GetFromJsonAsync<OrganizationToRead>(UriSegment + $"/{id}");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to get organization with id {id}", id);
            }

            return null;
        }

        public async Task UpdateOrganization(OrganizationToWrite organization, long id)
        {
            var content = new StringContent(JsonSerializer.Serialize(organization), Encoding.UTF8, MediaType);
            var response = await httpClient.PutAsync(UriSegment + $"/{id}", content);

            if (response.IsSuccessStatusCode)
            {
                toastService.ShowSuccess($"{organization.Name} updated successfully", "Saved");
                return;
            }

            toastService.ShowError($"{organization.Name} failed to update", "Save Failed");
        }
    }
}
