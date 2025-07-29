using Newtonsoft.Json.Converters;
using Newtonsoft.Json;

namespace Service_ApiGateway
{
    public abstract class ApiClientBase
    {
        void UpdateJsonSerializerSettings(JsonSerializerSettings settings)
        {
            settings.NullValueHandling = NullValueHandling.Include;
            settings.Converters = new List<JsonConverter> { new StringEnumConverter() };
            settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            settings.DefaultValueHandling = DefaultValueHandling.Include;
        }
    }
}
