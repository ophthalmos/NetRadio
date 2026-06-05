using System;
using System.Net;
using System.Net.Http;

namespace NetRadio.cls;

/// <summary>
/// Verwaltet eine Singleton-Instanz des HttpClient für die gesamte Anwendung.
/// Verhindert Socket-Exhaustion und löst DNS-Caching-Probleme durch PooledConnectionLifetime.
/// </summary>
public static class NetHttpClient
{
    private static readonly Lazy<HttpClient> _lazyClient = new(CreateClient);

    public static HttpClient Instance => _lazyClient.Value;

    private static HttpClient CreateClient()
    {
        var handler = new SocketsHttpHandler
        {
            PooledConnectionLifetime = TimeSpan.FromMinutes(2),
            PooledConnectionIdleTimeout = TimeSpan.FromMinutes(1),
            AutomaticDecompression = DecompressionMethods.All, // .NET 10: "All" statt GZip | Deflate
            UseCookies = false,
            // Verbessert die Performance bei vielen kleinen Requests (z.B. Cover-Art laden)
            MaxConnectionsPerServer = 10
        };

        var client = new HttpClient(handler)
        {
            Timeout = TimeSpan.FromSeconds(30)
        };

        var version = typeof(NetHttpClient).Assembly.GetName().Version?.ToString(2) ?? "1.0";
        client.DefaultRequestHeaders.UserAgent.ParseAdd($"NetRadio/{version} (Windows)");

        // Manche Server blockieren Requests ohne Accept-Header
        client.DefaultRequestHeaders.Accept.ParseAdd("*/*");

        return client;
    }
}