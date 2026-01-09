using System;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace NetRadio.cls;

public static class HttpClientProgressExtensions
{
    public static async Task DownloadDataAsync(this HttpClient client, string requestUrl, Stream destination, IProgress<float>? progress = null, CancellationToken cancellationToken = default)
    {
        // ResponseHeadersRead ist wichtig für Performance: Wir wollen nicht erst warten, bis alles im RAM ist
        using var response = await client.GetAsync(requestUrl, HttpCompletionOption.ResponseHeadersRead, cancellationToken);

        response.EnsureSuccessStatusCode(); // Wirft Exception bei 404, 500 etc.

        var contentLength = response.Content.Headers.ContentLength;

        using var download = await response.Content.ReadAsStreamAsync(cancellationToken);

        // Wenn kein Progress gewünscht oder Größe unbekannt, direkt kopieren (schneller)
        if (progress is null || !contentLength.HasValue)
        {
            await download.CopyToAsync(destination, cancellationToken);
            return;
        }

        // Wrapper, um Bytes in Prozent umzurechnen
        var progressWrapper = new Progress<long>(totalBytesRead =>
        {
            if (contentLength.Value > 0)
            {
                var percentage = (float)totalBytesRead / contentLength.Value * 100f;
                progress.Report(percentage);
            }
        });

        await download.CopyToAsync(destination, 81920, progressWrapper, cancellationToken);
    }

    /// <summary>
    /// Kopiert Stream-Daten mit Fortschrittsmeldung.
    /// </summary>
    private static async Task CopyToAsync(this Stream source, Stream destination, int bufferSize, IProgress<long>? progress = null, CancellationToken cancellationToken = default)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(bufferSize); // .NET 8+ Syntax
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(destination);

        if (!source.CanRead) { throw new InvalidOperationException($"'{nameof(source)}' is not readable."); }

        if (!destination.CanWrite) { throw new InvalidOperationException($"'{nameof(destination)}' is not writable."); }

        var buffer = new byte[bufferSize];
        long totalBytesRead = 0;
        int bytesRead;

        while ((bytesRead = await source.ReadAsync(buffer, cancellationToken).ConfigureAwait(false)) != 0)
        {
            await destination.WriteAsync(buffer.AsMemory(0, bytesRead), cancellationToken).ConfigureAwait(false);

            totalBytesRead += bytesRead;
            progress?.Report(totalBytesRead);
        }
    }
}