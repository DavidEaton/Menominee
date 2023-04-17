using System.Text.Json;

namespace CustomerVehicleManagement.Tests.Helpers
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
            using (JsonDocument doc = JsonDocument.Parse(json))
            {
                JsonElement root = doc.RootElement;
                int id = root.GetProperty("id").GetInt32();
                return id;
            }
        }
    }
}
