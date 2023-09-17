using CSharpFunctionalExtensions;
using Menominee.Common.Http;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Menominee.Common.Extensions
{
    public static class HttpResponseMessageExtensions
    {
        public static async Task<Result<HttpResponseMessage>> CheckResponse(this HttpResponseMessage response)
        {
            return response.IsSuccessStatusCode
                ? Result.Success(response)
                : Result.Failure<HttpResponseMessage>(await response.Content.ReadAsStringAsync());
        }

        public static async Task<Result<PostResponse>> ReadPostResult(this HttpResponseMessage response)
        {
            try
            {
                var data = await response.Content.ReadFromJsonAsync<PostResponse>();
                return data is not null
                    ? Result.Success(data)
                    : Result.Failure<PostResponse>("Empty result");
            }
            catch (Exception)
            {
                return Result.Failure<PostResponse>("Failed to read the post result.");
            }
        }
    }
}
