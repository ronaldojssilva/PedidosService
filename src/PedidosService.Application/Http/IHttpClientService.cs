using System.Diagnostics.CodeAnalysis;
using System.Net.Http.Json;

namespace PedidosService.Application.Http;

public interface IHttpClientService
{
    Task<HttpResponseMessage> PostAsync(string url, HttpContent content, CancellationToken cancellationToken = default);
}

public class HttpClientService : IHttpClientService
{
    private readonly HttpClient _httpClient;

    public HttpClientService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public Task<HttpResponseMessage> PostAsync(string url, HttpContent content, CancellationToken cancellationToken = default)
    {
        return _httpClient.PostAsync(url, content, cancellationToken);
    }
}

public static class HttpClientServiceExtensions
{
    public static Task<HttpResponseMessage> PostAsJsonAsync<TValue>(this IHttpClientService client, [StringSyntax("Uri")] string? requestUri, TValue value, CancellationToken cancellationToken = default)
    {
        if (client is null)
        {
            throw new ArgumentNullException(nameof(client));
        }
        JsonContent content = JsonContent.Create(value, mediaType: null, options: null);
        return client.PostAsync(requestUri, content, cancellationToken);
    }
}
