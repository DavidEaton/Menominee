using Blazored.Toast.Services;
using CSharpFunctionalExtensions;
using Menominee.Client.Services.Shared;
using Menominee.Common.Http;
using Menominee.Shared.Models.Persons;
using System.Net.Http.Json;

namespace Menominee.Client.Services
{
    public class PersonDataService : DataServiceBase<PersonDataService>, IPersonDataService
    {
        private readonly HttpClient httpClient;
        private readonly IToastService toastService;
        private const string UriSegment = "api/persons";

        public PersonDataService(HttpClient httpClient,
            ILogger<PersonDataService> logger,
            IToastService toastService,
            UriBuilderFactory uriBuilderFactory)
            : base(uriBuilderFactory, logger)
        {
            this.httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            this.toastService = toastService ?? throw new ArgumentNullException(nameof(toastService));
        }

        public async Task<Result<PostResponse>> AddAsync(PersonToWrite fromCaller)
        {
            var entityType = "Person";
            try
            {
                var result = await httpClient.AddAsync(
                    UriSegment,
                    fromCaller,
                    Logger);

                if (result.IsSuccess)
                    toastService.ShowSuccess($"{entityType} added successfully", "Saved");

                if (result.IsFailure)
                    toastService.ShowError($"{fromCaller.Name} failed to update", "Save Failed");

                return result;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, $"Failed to add {entityType}");
                return Result.Failure<PostResponse>("An unexpected error occurred");
            }
        }

        public async Task<Result<IReadOnlyList<PersonToReadInList>>> GetAllAsync()
        {
            var errorMessage = "Failed to get Persons";

            try
            {
                var result = await httpClient.GetFromJsonAsync<IReadOnlyList<PersonToReadInList>>($"{UriSegment}/list");
                return result is not null
                    ? Result.Success(result)
                    : Result.Failure<IReadOnlyList<PersonToReadInList>>(errorMessage);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, errorMessage);
                return Result.Failure<IReadOnlyList<PersonToReadInList>>(errorMessage);
            }
        }

        public async Task<Result<PersonToRead>> GetAsync(long id)
        {
            var errorMessage = $"Failed to get person with id {id}";

            try
            {
                var result = await httpClient.GetFromJsonAsync<PersonToRead>(UriSegment + $"/{id}");
                return result is not null
                    ? Result.Success(result)
                    : Result.Failure<PersonToRead>(errorMessage);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, errorMessage);
                return Result.Failure<PersonToRead>(errorMessage);
            }
        }

        public async Task<Result> UpdateAsync(PersonToWrite fromCaller)
        {
            return await httpClient.UpdateAsync(
                UriSegment,
                fromCaller,
                Logger,
                person => $"{fromCaller.Name.FirstName} {fromCaller.Name.LastName}",
                person => person.Id);
        }

        public async Task DeletePerson(long id)
        {
            try
            {
                await httpClient.DeleteAsync($"{UriSegment}/{id}");
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Failed to delete person with id {id}", id);
            }
        }
    }
}
