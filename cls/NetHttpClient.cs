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
    // Die Lazy-Initialisierung stellt sicher, dass der Client erst erstellt wird, wenn er das erste Mal gebraucht wird.
    private static readonly Lazy<HttpClient> _lazyClient = new(() => CreateClient());

    /// <summary>
    /// Zugriff auf die globale HttpClient-Instanz.
    /// </summary>
    public static HttpClient Instance => _lazyClient.Value;

    private static HttpClient CreateClient()
    {
        // SocketsHttpHandler ist der moderne Handler in .NET Core/.NET 5+
        var handler = new SocketsHttpHandler
        {
            // WICHTIG: Begrenzt die Lebensdauer einer Verbindung auf 2 Minuten.
            // Das zwingt den Client, DNS-Einträge regelmäßig neu aufzulösen.
            // Löst das Problem, dass statische Clients IP-Änderungen von Servern nicht mitbekommen.
            PooledConnectionLifetime = TimeSpan.FromMinutes(2),

            // Optional: Automatische Dekompression (spart Bandbreite bei APIs)
            AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate,

            // Optional: Cookies deaktivieren, falls nicht benötigt (spart Speicher)
            UseCookies = false
        };

        var client = new HttpClient(handler)
        {
            // Ein vernünftiges Timeout für Streaming/Downloads. 
            // Für Stream-Wiedergabe (nicht Download) ist Timeout oft irrelevant, 
            // aber für API-Calls wichtig.
            Timeout = TimeSpan.FromSeconds(30)
        };

        // Standard-Header setzen
        client.DefaultRequestHeaders.UserAgent.ParseAdd("NetRadio/2.10 (Windows)");

        // Verhindert, dass der Server denkt, wir wollen die Verbindung nach einem Request schließen
        client.DefaultRequestHeaders.ConnectionClose = false;

        return client;
    }
}