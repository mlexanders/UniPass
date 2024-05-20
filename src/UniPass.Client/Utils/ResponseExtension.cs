using System.Net.Http.Json;

namespace UniPass.Client.Utils;

public static class ResponseExtension
{
    public static async Task<T> GetResult<T>(this HttpResponseMessage? response)
    {
        if (response is null) throw new UniPassClientException("Сервер не вернул ответ");
        
        if (!response.IsSuccessStatusCode)
            throw new UniPassClientException(response.RequestMessage?.ToString());
        
        var content = await response.Content.ReadFromJsonAsync<T>();
        return content;
    }
}