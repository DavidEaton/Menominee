using System.Text.Json;
using System.Text.Json.Serialization;

namespace CustomerVehicleManagement.Api.IntegrationTests
{
    public static class JsonSerializerHelper
    {
        public static JsonSerializerOptions DefaultSerializationOptions => new()
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.Always
        };

        public static JsonSerializerOptions DefaultDeserializationOptions => new()
        {
            PropertyNameCaseInsensitive = true
        };
    }
}
