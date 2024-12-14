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

    //public class ApiClientBase : IDisposable
    //{
    //    private readonly HttpClient _httpClient;

    //    public ApiClientBase()
    //    {
    //        _httpClient = httpClient;
    //    }

    //    protected async Task<HttpResponseMessage> SendRequestAsync(HttpRequestMessage request)
    //    {
    //        try
    //        {
    //            return await _httpClient.SendAsync(request);
    //        }
    //        catch (HttpRequestException ex)
    //        {
    //            // Обработка ошибок сети
    //            throw new ApiException($"Network error: {ex.Message}", (int)HttpStatusCode.InternalServerError, null, null, ex);
    //        }
    //    }

    //    public void Dispose()
    //    {
    //        _httpClient?.Dispose();
    //    }
    //}
}
