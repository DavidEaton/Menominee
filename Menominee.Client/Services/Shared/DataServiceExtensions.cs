using CSharpFunctionalExtensions;
using Menominee.Shared.Models.Http;
using System.Net.Http.Json;

namespace Menominee.Client.Services.Shared
{
    public static class DataServiceExtensions
    {
        public static async Task<Result> UpdateAsync<T>(
            this HttpClient httpClient,
            string uriSegment,
            T itemToUpdate,
            ILogger logger,
            Func<T, string> getName,
            Func<T, long> getId) where T : class
        {
            return await UpdateAsyncInternal(httpClient, uriSegment, itemToUpdate, logger, getName, getId);
        }

        public static async Task<Result> UpdateAsync<T>(
            this HttpClient httpClient,
            string uriSegment,
            T itemToUpdate,
            ILogger logger,
            Func<T, string> getName,
            Func<T, string> getId) where T : class
        {
            return await UpdateAsyncInternal(httpClient, uriSegment, itemToUpdate, logger, getName, getId);
        }

        private static async Task<Result> UpdateAsyncInternal<T, TId>(
            HttpClient httpClient,
            string uriSegment,
            T itemToUpdate,
            ILogger logger,
            Func<T, string> getName,
            Func<T, TId> getId) where T : class
        {
            const string operation = "update";
            try
            {
                var response = await httpClient.PutAsJsonAsync($"{uriSegment}/{getId(itemToUpdate).ToString()}", itemToUpdate);

                if (response.IsSuccessStatusCode)
                    return Result.Success();

                var serverMessage = await response.Content.ReadAsStringAsync();
                var errorMessage = $"Failed to {operation} item with id {getId(itemToUpdate)}. Server responded with: {serverMessage}";
                logger.LogError(errorMessage);
                return Result.Failure(errorMessage);
            }
            catch (Exception ex)
            {
                var errorMessage = $"Exception occurred while attempting to {operation} {getName(itemToUpdate)} with id {getId(itemToUpdate)}";
                logger.LogError(ex, errorMessage);
                return Result.Failure(errorMessage);
            }
        }

        public static async Task<Result<PostResponse>> AddAsync<T>(
                    this HttpClient httpClient,
                    string uriSegment,
                    T itemToAdd,
                    ILogger logger) where T : class
        {
            var operation = "Add";

            try
            {
                var response = await httpClient.PostAsJsonAsync(uriSegment, itemToAdd);

                if (response.IsSuccessStatusCode)
                {
                    var postResponse = await response.Content.ReadFromJsonAsync<PostResponse>();
                    return Result.Success(postResponse);
                }
                else
                {
                    var serverMessage = await response.Content.ReadAsStringAsync();
                    var errorMessage = $"Failed to {operation} item. Server responded with: {serverMessage}";
                    logger.LogError(errorMessage);
                    return Result.Failure<PostResponse>(errorMessage);
                }
            }
            catch (Exception ex)
            {
                var errorMessage = $"Exception occurred while attempting to {operation} item";
                logger.LogError(ex, errorMessage);
                return Result.Failure<PostResponse>(errorMessage);
            }
        }
    }
}
