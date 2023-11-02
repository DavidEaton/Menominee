using Menominee.Api.Common;
using System.Text.Json;

namespace TestingHelperLibrary
{
    public static class JsonSerializerHelper
    {
        public static JsonSerializerOptions DefaultDeserializerOptions => new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
        };

        public static JsonSerializerOptions DefaultSerializerOptions
        {
            get
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    WriteIndented = true,
                };

                return options;
            }
        }
        public static int GetIdFromString(string json)
        {
            using var doc = JsonDocument.Parse(json);
            return doc.RootElement.GetProperty("id").GetInt32();
        }

        public static (bool, ApiError) DeserializeApiError(string errorContent)
        {
            try
            {
                var apiError = JsonSerializer.Deserialize<ApiError>(errorContent, DefaultDeserializerOptions);
                return (true, apiError ?? new() { Message = $"ApiError.Error unknown: ApiError is unavailable" });
            }
            catch (Exception ex)
            {
                return (false, new() { Message = ex.Message });
            }
        }
    }
}
