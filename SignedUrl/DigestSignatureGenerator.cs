using System.Security.Cryptography;
using System.Text;
using System.Web;
using SignedUrl.Abstractions;

namespace SignedUrl;

/// <summary>
/// Uses <see cref="SHA256"/> to generate a digest of the query string and then uses <see cref="ISignatureProtector"/>
/// to protect the digest against brute force attacks.
///
/// The <see cref="ISignatureProtector"/> MUST NOT return the bytes as-is, but rather encrypt them.
/// </summary>
/// <param name="protector">The <see cref="ISignatureProtector"/> to use.</param>
public class DigestSignatureGenerator(ISignatureProtector protector) : ISignatureGenerator
{
    /// <inheritdoc />
    public string GenerateSignature(IDictionary<string, string?> data)
    {
        // kinda hacky, but MS doesn't publish the HttpUtility.HttpQSCollection class that contains the ToString() method
        var queryCollection = HttpUtility.ParseQueryString(string.Empty);

        string queryString;
        if (data is { Count: > 0 })
        {
            foreach (var (key, value) in data)
                queryCollection[key] = value;

            queryString = queryCollection.ToString() ?? throw new InvalidOperationException();
        }
        else
            queryString = string.Empty;

        var queryStringBytes = Encoding.UTF8.GetBytes(queryString);
        var digestBytes = SHA256.HashData(queryStringBytes);

        byte[] securedQueryStringBytes;
        try
        {
            securedQueryStringBytes = protector.Protect(digestBytes);
        }
        catch (ProtectorException e)
        {
            throw new SignatureGenerationException("Failed to generate signature.", e);
        }

        return Convert.ToBase64String(securedQueryStringBytes);
    }
}
