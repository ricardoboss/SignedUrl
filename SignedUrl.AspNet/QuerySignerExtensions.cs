using Microsoft.AspNetCore.Http;
using SignedUrl.Abstractions;

namespace SignedUrl.AspNet;

/// <summary>
/// Extensions for <see cref="IQuerySigner"/>.
/// </summary>
public static class QuerySignerExtensions
{
    /// <summary>
    /// Generates a fully qualified URL for the given endpoint using an existing <see cref="HttpRequest"/>.
    /// </summary>
    /// <param name="querySigner">The <see cref="IQuerySigner"/> to use.</param>
    /// <param name="request">
    /// The base <see cref="HttpRequest"/> whose scheme, host and port will be used for the new fully qualified URL.
    /// </param>
    /// <param name="endpoint">The endpoint to generate a fully qualified URL for.</param>
    /// <param name="queryParams">Optional query parameters to include in the URL.</param>
    /// <param name="signed">Whether or not to sign the URL.</param>
    /// <returns>A fully qualified URL for the given endpoint.</returns>
    public static string GenerateFullyQualified(this IQuerySigner querySigner, HttpRequest request, string endpoint,
        IDictionary<string, string?>? queryParams = null, bool signed = false)
    {
        var builder = new UriBuilder
        {
            Scheme = request.Scheme,
            Host = request.Host.Host,
            Path = endpoint,
        };

        if (request.Host.Port.HasValue)
            builder.Port = request.Host.Port.Value;

        if (queryParams is not null)
        {
            builder.Query = signed
                ? querySigner.GenerateSignature(queryParams)
                : QueryString.Create(queryParams).ToUriComponent();
        }

        return builder.ToString();
    }
}